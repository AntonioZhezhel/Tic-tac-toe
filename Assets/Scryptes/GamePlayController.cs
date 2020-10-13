using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayController : MonoBehaviour
{
    //Префаб-триггер
    public Transform cell;

    //Объект-папка для ячеек
    public Transform cellsParent;

    //Массив ячеек поля
    private Transform[,] sprites;
    public Transform line;
    public Image win;
    public Image fail;
    public Image standoff;
    private float x = -1.5f;
    private float y = 1.15f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Awake()
    {
        InitField();
    }

    void InitField()
    {
        int arrColl = 0;
        int arrCell = 0;
        sprites = new Transform[3, 3];
        for (int i = 1; i <= 9; i++)
        {
            Transform tempObj = (Transform) Instantiate(cell, new Vector3(x, y, 0), Quaternion.identity);
            sprites[arrColl, arrCell] = tempObj;
            tempObj.transform.SetParent(cellsParent.transform);
            tempObj.GetComponent<CellClass>().posColl = arrColl;
            tempObj.GetComponent<CellClass>().posCell = arrCell;

            arrColl++;
            x += 1.5f;

            //Смещаемся на строку вниз, сбрасываем колонку

            if (i % 3 == 0)
            {
                arrColl = 0;
                arrCell++;
                x = -1.5f;
                y -= 1.5f;
            }
        }
    }

    int StepByField(Vector2 vector, Vector2 currentPosition)
    {
        int coincide = 0;

        Vector2 step = currentPosition + vector;
        string figure = sprites[(int) currentPosition.x, (int) currentPosition.y].GetComponent<SpriteRenderer>().sprite
            .name;
        int stop = 0;
        while (stop < 2)
        {
            //  Блок проверок:                
            // -Проверка выхода за пределы массива
            // -Проверка на наличии спрайта в "ячейке"
            // -Проверка соответствует ли следующий спрайт, "походившему"
            if ((step.x > -1 && step.y > -1 && step.x < 3 && step.y < 3)
                && (sprites[(int) step.x, (int) step.y].GetComponent<SpriteRenderer>().sprite)
                && sprites[(int) step.x, (int) step.y].GetComponent<SpriteRenderer>().sprite.name == figure)
            {
                coincide++;
                step += vector;
            }
            else
            {
                vector *= -1;
                step = currentPosition + vector;
                stop++;
            }
        }

        return coincide;
    }

    void DrawLine(Vector2 position, string orientation)
    {
        Vector3 posLine = position;
         Transform tr;
        switch (orientation)
        {
            // Центрируем линию по вертикали
            case "vertical":
                posLine.y = -0.04f;
                Instantiate(line, posLine, Quaternion.identity);
                break;
            // Центрируем линию по горизонтали
            case "horizontal":
                posLine.x = 0;
                Instantiate(line, posLine, Quaternion.Euler(0, 0, 90));
                break;
            case "left_diagonal":
                tr = (Transform) Instantiate(line, new Vector3(-0.04f, -0.39f, 0), Quaternion.Euler(0, 0, -45));
                tr.localScale += new Vector3(0, 0.3f, 0);
                break;
            case "right_diagonal":
                tr = (Transform) Instantiate(line, new Vector3(-0.04f, -0.39f, 0), Quaternion.Euler(0, 0, 45));
                tr.localScale += new Vector3(0, 0.3f, 0);
                break;

        }
    }

    IEnumerator ShowMessage(string winner)
    {
        yield return new WaitForSeconds(1.0f);

        GameObject.Find("field_spr").GetComponent<SpriteRenderer>().enabled = false;
        Destroy(GameObject.Find("Line(Clone)"));
        foreach (Transform sprite in sprites)
        {
            sprite.GetComponent<SpriteRenderer>().sprite = null;
        }


        if (winner != null)
            {
                if (GameObject.Find("Player").GetComponent<Player>().namePlayer == winner)
                {
                    win.enabled = true;
                }
                else
                {
                    fail.enabled = true;
                }
            }
            else
            {
                standoff.enabled = true;
            }
        
    }

    public bool CheckWin(Transform callerCell, string callerFigure)
    {
        bool isWin = false;
        if (callerCell)
        {
            Vector2 currenPos = new Vector2();
            currenPos.x = callerCell.GetComponent<CellClass>().posColl;
            currenPos.y = callerCell.GetComponent<CellClass>().posCell;

            if (StepByField(new Vector2(0, -1), currenPos) == 2)
            {
                DrawLine(sprites[(int) currenPos.x, (int) currenPos.y].position, "vertical");
                StartCoroutine(ShowMessage(callerFigure));
                isWin = true;
            }
            else if (StepByField(new Vector2(1, -1), currenPos) == 2)
            {
                DrawLine(sprites[(int) currenPos.x, (int) currenPos.y].position, "left_diagonal");
                StartCoroutine(ShowMessage(callerFigure));
                isWin = true;
            }
            else if (StepByField(new Vector2(1, 0), currenPos) == 2)
            {
                DrawLine(sprites[(int) currenPos.x, (int) currenPos.y].position, "horizontal");
                StartCoroutine(ShowMessage(callerFigure));
                isWin = true;
            }
            else if (StepByField(new Vector2(1, 1), currenPos) == 2)
            {
                DrawLine(sprites[(int) currenPos.x, (int) currenPos.y].position, "right_diagonal");
                StartCoroutine(ShowMessage(callerFigure));
                isWin = true;
            }
        }
        else
        {
            StartCoroutine(ShowMessage(null));

        }

        return isWin;
    }

    public void Restart()
    {
        Turn.turn = false;
        GameObject.Find("field_spr").GetComponent<SpriteRenderer>().enabled = true;
        GameObject.Find("Enemy").GetComponent<Enemy>().ReInitEnemy();
        GameObject.Find("Player").GetComponent<Player>().ReinitPlayer();

        if (!win.enabled && !fail.enabled && !standoff.enabled)
        {
            foreach (Transform sprite in sprites)
            {
                sprite.GetComponent<SpriteRenderer>().sprite = null;
            }
            
        }

        win.enabled = false;
        fail.enabled = false;
        standoff.enabled = false;
    }
}



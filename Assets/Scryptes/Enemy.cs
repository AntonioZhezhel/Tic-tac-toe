using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
	public string namePlayer = "Round";
	bool enable = true;
	//Коллекция свободных ячеек
	private List<Transform> freeCells;
 	public Sprite round;

    void TurnEnemy()
    {
        while (enable && Turn.turn)
        {
	        //Цикл работает пока не будет передан ход или не отключен И
            int randomIndex = (int) Random.Range(0.0f, freeCells.Count - 1);
            if (!freeCells[randomIndex].GetComponent<SpriteRenderer>().sprite)
            {
	            freeCells[randomIndex].GetComponent<SpriteRenderer>().sprite = round;
	            Transform hitCell = freeCells[randomIndex];
	            if (!GameObject.Find("GamePlay").GetComponent<GamePlayController>().CheckWin(hitCell, namePlayer))
	            {
		            Turn.turn = false;
		            StartCoroutine(Turn.SetPouse());
	            }
	            else
	            {
		            enable = false;
	            }
            }

            freeCells.RemoveAt(randomIndex);


                if (freeCells.Count < 1 )
                {
	                enable = false;
	                GameObject.Find("GamePlay").GetComponent<GamePlayController>().CheckWin(null, null);
                }
            }
        }
    
		void InitArrayCells()
	{
		freeCells = new List<Transform>();
		GameObject[] tempArr = GameObject.FindGameObjectsWithTag("cell");

		foreach (GameObject obj in tempArr)
		{
			freeCells.Add(obj.GetComponent<Transform>());
		}
	}
		
		public void ReInitEnemy()
		{
			InitArrayCells();
			enable = true;
		}
    // Start is called before the first frame update
    void Start()
    {
        InitArrayCells();
    }

    // Update is called once per frame
    void Update()
    {
        if (enable && Turn.turn && !Turn.Pause)
        {TurnEnemy();} 
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	public Sprite cross;
	public string namePlayer = "Cross";
	private bool enable = true;
	void ClickPlayer()
	{
		Vector2 clickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		RaycastHit2D hit = Physics2D.Raycast(clickPoint, Vector2.zero);
		if (hit.collider)
		{
			Transform hitCell = hit.transform;
			if (!hitCell.GetComponent<SpriteRenderer>().sprite && !Turn.turn && !Turn.Pause)
			{
				hitCell.GetComponent<SpriteRenderer>().sprite = cross;

				if (!GameObject.Find("GamePlay").GetComponent<GamePlayController>().CheckWin(hitCell, namePlayer))
				{
					Turn.turn = true;
					StartCoroutine(Turn.SetPouse());
				}
				else
				{
					enable = false;
				}
				
			}
		}
	}

	
	public void ReinitPlayer()
	{
		enable = true;
	}
	

	

    // Start is called before the first frame update
    void Start()
    {
	   
    }

    // Update is called once per frame
    void Update()
    {
	    if (enable && Input.GetMouseButtonDown(0))
	    {
		     ClickPlayer();
	    }
		   
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{

    public void RestartButton_onClick()
    {
        GameObject.Find("GamePlay").GetComponent<GamePlayController>().Restart();
        
    }
    
    
    
}

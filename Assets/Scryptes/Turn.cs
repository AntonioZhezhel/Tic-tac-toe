using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Turn
{
    public static bool turn = false;
    private static bool pause = false;

    public static bool Pause
    {
        get { return pause; }
    }

    public static IEnumerator SetPouse()
    {
        pause = true;
        yield return new WaitForSeconds(0.6f);
        pause = false;
    }
}
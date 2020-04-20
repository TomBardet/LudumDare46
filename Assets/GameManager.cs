using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameManager
{
    public static Vector2 exit;//Tempo

    static GameManager()
    {
        exit = (Vector2)Warrior.instance.transform.position + Vector2.up * 10;
    }

    static void Start()
    {

    }

    public static void Defeat()//Called by warrior on death
    {
        GameController.instance.ResetLevel();
    }
}

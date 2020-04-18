using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public UIController UIController;
    public int CurentLevel = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPlay() {
        UIController.OnPlayPress();
    }

    public void OnNextLevel() {
        UIController.OnNextLevelPress();
    }

    public void EndGame() {
        UIController.TriggerEndGame();
    }

    public void GameStart() {
        UIController.TriggerGameStart();
    }
}

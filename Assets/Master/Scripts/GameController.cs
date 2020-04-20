using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Controller")]
    public UIController UIController;
    private LevelController LevelController;

    [Header("KeyHole")]
    public float KeyHoleSpeed = 200;
    public Rigidbody2D KeyHole;

    [Header("Boolean")]
    public bool IsLevelLoaded = false;

    public GameObject buttonWin;

    private enum GamePhaseType{Menu, Observation, OnGame, EndGame, Cinematic };
    private GamePhaseType GamePhase = GamePhaseType.Menu;

    // Start is called before the first frame update
    void Start()
    {
        GamePhase = GamePhaseType.Menu;
        LevelController = GetComponent<LevelController>();
    }   

    // Update is called once per frame
    void FixedUpdate() {
        if (GamePhase == GamePhaseType.Observation) {
            KeyHoleControlles(KeyHoleSpeed);
        }
    }
    private void Update() {
        if (GamePhase == GamePhaseType.Observation && Input.GetButtonDown("Jump")) {
            GameStart();
        }
    }

    /******************************** GAME PHASE ***************************************/
    public void OnPlay() {
        UIController.OnPlayPress();
        MusicController.instance.musicMenu.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        MusicController.instance.musicLvl.setParameterByName("Player", 0);
        MusicController.instance.musicLvl.start();
        GamePhase = GamePhaseType.Observation;
        KeyHole.gameObject.SetActive(true);
        LevelController.OpenLevel(LevelController.CurentLevel);
    }

    public void OnNextLevel() {
        //Transition de level suivit de la phase d'observation ?
        MusicController.instance.musicLvl.setParameterByName("Player", 0);
        UIController.OnNextLevelPress();
        GamePhase = GamePhaseType.Observation;
        LevelController.OpenLevel(LevelController.CurentLevel);
    }

    public void OnWin()
    {
        buttonWin.SetActive(true);
    }
    
    public void EndGame() {
        UIController.TriggerEndGame();
        GamePhase = GamePhaseType.EndGame;
        LevelController.ClearLevelContainer(LevelController.CurentLevel);
        LevelController.IncreaseCurentLevel();
        buttonWin.SetActive(false);

    }

    public void GameStart() {
        UIController.TriggerGameStart();
        MusicController.instance.musicLvl.setParameterByName("Player", 1);
        MusicController.instance.musicLvl.setParameterByName("Fight", 0);
        GamePhase = GamePhaseType.OnGame;
        KeyHole.gameObject.SetActive(false);

    }

    /*************************** KEYHOLE ***************************/
    private void KeyHoleControlles(float speed) {

        Camera cam = Camera.main;
        Vector2 bottomLeft = cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector2 topRight = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, cam.nearClipPlane));
        float borderModifier = 0.5f;

        Vector2 moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        
        if (moveDirection.x > 0.1f || moveDirection.x < -0.1f || moveDirection.y > 0.1f || moveDirection.y < -0.1f) {
            KeyHole.velocity = moveDirection * speed * Time.deltaTime;
        }
        else
            KeyHole.velocity = Vector2.zero;

        //Move Right
        if (KeyHole.transform.position.x + borderModifier > topRight.x) {
            KeyHole.transform.position = new Vector2(KeyHole.transform.position.x - 0.1f, KeyHole.transform.position.y);
        }
        //Move Left
        if (KeyHole.transform.position.x - borderModifier < bottomLeft.x) {
            KeyHole.transform.position = new Vector2(KeyHole.transform.position.x + 0.1f, KeyHole.transform.position.y);
        }
        //Move Top
        if (KeyHole.transform.position.y + borderModifier*3f > topRight.y) {
            KeyHole.transform.position = new Vector2(KeyHole.transform.position.x, KeyHole.transform.position.y - 0.1f);
        }
        //Move Bottom
        if (KeyHole.transform.position.y - borderModifier*3f < bottomLeft.y) {
            KeyHole.transform.position = new Vector2(KeyHole.transform.position.x, KeyHole.transform.position.y + 0.1f);
        }
    }
}

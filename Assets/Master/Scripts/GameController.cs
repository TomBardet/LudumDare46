using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEngine.UI;
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

    private enum GamePhaseType { Menu, Observation, OnGame, EndGame, Cinematic };
    private GamePhaseType GamePhase = GamePhaseType.Menu;

    public static GameController instance;
    bool doOnce;

    public List<string> Dialogue1 = new List<string>();
    public List<string> Dialogue2 = new List<string>();
    public PlayableDirector TimelineDialogue;
    public GameObject dialogue1;
    public GameObject dialogue2;
    float TimeKey;
    public float MaxTimeKey;
    public GameObject MenuObj;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        GamePhase = GamePhaseType.Menu;
        LevelController = GetComponent<LevelController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GamePhase == GamePhaseType.Observation)
        {
            KeyHoleControlles(KeyHoleSpeed);
        }
        if(GamePhase == GamePhaseType.EndGame)
        {
            if(!doOnce)
            {
                doOnce = true;
                Invoke("OnNextLevel", 1f);
            }
        }
    }
    private void Update()
    {
        if (GamePhase == GamePhaseType.Observation && TimeKey < 0)
        {
            dialogue1.SetActive(false);
            dialogue2.SetActive(false);
            GameStart(false);
            Debug.Log("HERE");

        }
        if (TimeKey >= 0) TimeKey -= Time.deltaTime;
    }

    /******************************** GAME PHASE ***************************************/
    public void OnPlay()
    {
        MenuObj.SetActive(false);
        UIController.OnPlayPress();
        MusicController.instance.musicMenu.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        MusicController.instance.musicLvl.setParameterByName("Player", 0);
        MusicController.instance.musicLvl.start();
        GamePhase = GamePhaseType.Observation;
        TimeKey = MaxTimeKey;
        KeyHole.gameObject.SetActive(true);
        Debug.Log("key");
        LevelController.OpenLevel(LevelController.CurentLevel);

        Invoke("PlayDialogue", 0.2f);
        Debug.Log("playDialogue");

    }

    public void OnNextLevel()
    {
        //Transition de level suivit de la phase d'observation ?
        MusicController.instance.musicLvl.setParameterByName("Player", 0);
        UIController.OnNextLevelPress();
        GamePhase = GamePhaseType.Observation;
        TimeKey = MaxTimeKey;

        KeyHole.gameObject.SetActive(true);
        LevelController.OpenLevel(LevelController.CurentLevel);
        doOnce = false;

        Invoke("PlayDialogue", 0.2f);
        Debug.Log("playDialogue");

    }

    void PlayDialogue()
    {
        dialogue1.SetActive(true);
        dialogue2.SetActive(true);
        dialogue1.GetComponent<Text>().text = Dialogue1[LevelController.CurentLevel];
        dialogue2.GetComponent<Text>().text = Dialogue2[LevelController.CurentLevel];
        TimelineDialogue.Play();
    }

    public void OnWin()
    {
        //buttonWin.SetActive(true);
        EndGame();
    }

    public void EndGame()
    {
        UIController.TriggerEndGame();
        

        Invoke("UnloadWait", 3f);
    }

    void UnloadWait()
    {
        GamePhase = GamePhaseType.EndGame;
        LevelController.ClearLevelContainer(LevelController.CurentLevel);
        LevelController.IncreaseCurentLevel();
        buttonWin.SetActive(false);
    }

    public void GameStart(bool isReset) {
        UIController.TriggerGameStart(isReset);
        MusicController.instance.musicLvl.setParameterByName("Player", 1);
        MusicController.instance.musicLvl.setParameterByName("Fight", 0);
        GamePhase = GamePhaseType.OnGame;
        KeyHole.gameObject.SetActive(false);
        

    }

    public void ResetLevel()
    {
        LevelController.ClearLevelContainer(LevelController.CurentLevel);
        LevelController.OpenLevel(LevelController.CurentLevel);

        GameStart(true);
    }

    /*************************** KEYHOLE ***************************/
    private void KeyHoleControlles(float speed)
    {

        Camera cam = Camera.main;
        Vector2 bottomLeft = cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector2 topRight = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, cam.nearClipPlane));
        float borderModifier = 0.5f;

        Vector2 moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (moveDirection.x > 0.1f || moveDirection.x < -0.1f || moveDirection.y > 0.1f || moveDirection.y < -0.1f)
        {
            KeyHole.velocity = moveDirection * speed * Time.deltaTime;
        }
        else
            KeyHole.velocity = Vector2.zero;

        //Move Right
        if (KeyHole.transform.position.x + borderModifier > topRight.x)
        {
            KeyHole.transform.position = new Vector2(KeyHole.transform.position.x - 0.1f, KeyHole.transform.position.y);
        }
        //Move Left
        if (KeyHole.transform.position.x - borderModifier < bottomLeft.x)
        {
            KeyHole.transform.position = new Vector2(KeyHole.transform.position.x + 0.1f, KeyHole.transform.position.y);
        }
        //Move Top
        if (KeyHole.transform.position.y + borderModifier * 3f > topRight.y)
        {
            KeyHole.transform.position = new Vector2(KeyHole.transform.position.x, KeyHole.transform.position.y - 0.1f);
        }
        //Move Bottom
        if (KeyHole.transform.position.y - borderModifier * 3f < bottomLeft.y)
        {
            KeyHole.transform.position = new Vector2(KeyHole.transform.position.x, KeyHole.transform.position.y + 0.1f);
        }
    }
}

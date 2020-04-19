using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public UIController UIController;
    public int CurentLevel = 0;
    public float KeyHoleSpeed = 200;
    public List<Level> LevelList = new List<Level>();
    public Rigidbody2D KeyHole;

    private enum GamePhaseType{Menu, Observation, OnGame, EndGame, Cinematic };
    private GamePhaseType GamePhase = GamePhaseType.Menu;

    // Start is called before the first frame update
    void Start()
    {
        GamePhase = GamePhaseType.Menu;
    }   

    // Update is called once per frame
    void FixedUpdate() {
        if (GamePhase == GamePhaseType.Observation) {
            KeyHoleControlles(KeyHoleSpeed);
            AddCameraBorderCollider();
        }
    }
    private void Update() {
        if (GamePhase == GamePhaseType.Observation && Input.GetButtonDown("Jump")) {
            GameStart();
        }
    }
    public void OnPlay() {
        UIController.OnPlayPress();
        GamePhase = GamePhaseType.Observation;
        //dissable player control
    }

    public void OnNextLevel() {
        //Transition de level suivit de la phase d'observation ?
        UIController.OnPlayPress();
        GamePhase = GamePhaseType.Observation;
    }

    public void EndGame() {
        UIController.TriggerEndGame();
        GamePhase = GamePhaseType.EndGame;
        CurentLevel++;
    }

    public void GameStart() {
        UIController.TriggerGameStart();
        GamePhase = GamePhaseType.OnGame;
    }

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

    //Create Border at screen size to contain keyhole
    void AddCameraBorderCollider() {
        Camera cam = Camera.main;
        Vector2 bottomLeft = cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector2 topRight = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, cam.nearClipPlane));
        
        if(KeyHole.transform.position.y > topRight.y || KeyHole.transform.position.y < bottomLeft.y) {
            KeyHole.velocity = new Vector2(KeyHole.velocity.x,0);
        }
        if (KeyHole.transform.position.x > topRight.x || KeyHole.transform.position.x < bottomLeft.x) {
            KeyHole.velocity = new Vector2(0, KeyHole.velocity.y);
        }
    }
}

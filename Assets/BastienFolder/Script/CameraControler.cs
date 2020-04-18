using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    public bool IsMoving = false;
    private Vector3 CurrentLevelPosition = new Vector3(0,0,0);
    private Vector3 NextLevelPosition = new Vector3(0,0,0);

    public float TransitionSpeed = 1;
    public GameObject CurrentLevel;
    public GameObject NextLevel;

    private void Start() {
        NextLevelPosition = NextLevel.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsMoving) {
            MoveBetween(CurrentLevelPosition, NextLevelPosition, TransitionSpeed);
        }
        if (Vector2.Distance(transform.position, NextLevelPosition) < 0.02 && IsMoving) {
            transform.position = NextLevelPosition + new Vector3(0, 0, -10);
            IsMoving = false;
            //Modifier les positions des levels
        }
    }


    //Smoothly move between two points
    void MoveBetween(Vector3 start,Vector3 end, float speed) {
        transform.position = Vector3.Lerp(transform.position, end+new Vector3(0,0,-10), Time.deltaTime * speed);
    }
}

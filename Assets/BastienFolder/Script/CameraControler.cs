using System.Collections;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    //private variable
    public bool IsMoving = false;
    public bool IsScaling = false;
    private Vector3 nextLevelPosition = new Vector3(0,0,0);
    private Rigidbody2D body;
    private float startingSize = 5;

    //public variable
    public float TransitionSpeed = 1;
    public float ScalingSpeed = 1;
    public GameObject NextLevel;

    private void Start() {
        nextLevelPosition = NextLevel.transform.position;
        body = GetComponent<Rigidbody2D>();
        startingSize = GetComponent<Camera>().orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        //Movement Pour l'instant sur un boolean, plus tard check si le niveau est finit 
        if (IsMoving) {
            nextLevelPosition = NextLevel.transform.position;
            StartCoroutine(MoveTo(nextLevelPosition));
            IsMoving = false;
            // changer le next level
        }

        //Scaling Pour l'instant sur un boolean, plus tard check si le niveau est finit 
        if (IsScaling) {
            //When the camera need do scale up
            if(GetComponent<Camera>().orthographicSize <= NextLevel.GetComponent<Level>().levelSize * (int)startingSize)
                StartCoroutine(ReScale(NextLevel.GetComponent<Level>().levelSize * (int)startingSize, true));
            //When the camera need do scale down
            else
                StartCoroutine(ReScale(NextLevel.GetComponent<Level>().levelSize * (int)startingSize, false));
            IsScaling = false;
        }
    }

    //smoothly rescale the camera
    IEnumerator ReScale(int LevelSize, bool IsIncreasing) {
        //When the camera need do scale up
        if (IsIncreasing) {
            while (GetComponent<Camera>().orthographicSize <= LevelSize-0.05f) {
                float curentSize = GetComponent<Camera>().orthographicSize;
                GetComponent<Camera>().orthographicSize = Mathf.Lerp(curentSize, LevelSize, ScalingSpeed * Time.deltaTime);
                yield return null;
            }
            GetComponent<Camera>().orthographicSize = LevelSize;
        }
        //When the camera need do scale down
        else {
            while (GetComponent<Camera>().orthographicSize >= LevelSize+0.05f) {
                float curentSize = GetComponent<Camera>().orthographicSize;
                GetComponent<Camera>().orthographicSize = Mathf.Lerp(curentSize, LevelSize, ScalingSpeed * Time.deltaTime);
                yield return null;
            }
            GetComponent<Camera>().orthographicSize = LevelSize;
        }
    }

    //Smoothly move to a position
    IEnumerator MoveTo(Vector3 end) {
        while (Vector2.Distance(transform.position, nextLevelPosition) > 0.02f) {
            Vector2 dir = (end - transform.position).normalized;
            body.MovePosition((Vector2)transform.position + dir * TransitionSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = new Vector3(nextLevelPosition.x, nextLevelPosition.y, -10);
    }
}

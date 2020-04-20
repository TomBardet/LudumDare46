using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {

    [Header("Levels")]
    public GameObject LevelContainer;
    public int CurentLevel = 0;
    public int LevelsNumber = 10;
    public List<string> LevelList = new List<string>();

    public DoorEntrance Entrance;

    // Start is called before the first frame update
    void Start()
    {
        //initialize levels list
        for (int i = 1; i < LevelsNumber + 1; i++) {
            LevelList.Add("Level" + i);
        }
    }

    public void OpenLevel(int index) {
        StartCoroutine(LoadAsyncScene(LevelList[index]));
    }

    public void IncreaseCurentLevel() {
        if (CurentLevel + 1 < LevelsNumber) CurentLevel++;
    }

    public void ClearLevelContainer(int index) {
        StartCoroutine(UnloadAsyncScene(LevelList[index]));
        foreach (Transform child in LevelContainer.transform) {
            Destroy(child.gameObject);
        }
    }

    private void MoveLevel() {
        GameObject newLevel = GameObject.Find("LEVEL");
        newLevel.transform.SetParent(LevelContainer.transform);
        newLevel.transform.position = Vector2.zero;
        Entrance = newLevel.GetComponentInChildren<DoorEntrance>();

        CameraControler camera = Camera.main.GetComponent<CameraControler>();
        camera.NextLevel = newLevel;
        camera.IsMoving = true;
        camera.IsScaling = true;
    }

    IEnumerator LoadAsyncScene(string SceneName) {

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone) {
            yield return null;
        }
        MoveLevel();
    }

    IEnumerator UnloadAsyncScene(string SceneName) {

        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(SceneName,UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

        // Wait until the asynchronous scene fully loads
        while (!asyncUnload.isDone) {
            yield return null;
        }
    }
}

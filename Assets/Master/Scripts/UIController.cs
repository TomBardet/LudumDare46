using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private Animator UIAnimator;
    LevelController LC;
    // Start is called before the first frame update
    void Start()
    {
        UIAnimator = GetComponent<Animator>();
        LC = FindObjectOfType<LevelController>();
    }

    public void OnPlayPress() {
        UIAnimator.SetTrigger("Play");
    }

    public void OnNextLevelPress() {
        UIAnimator.SetTrigger("NextLevel");
    }

    public void TriggerEndGame() {
        UIAnimator.SetTrigger("EndGame");
    }

    public void TriggerGameStart() {
        UIAnimator.SetTrigger("GameStart");
        Debug.Log("Animation pétage de porte");
        LC.Entrance.PlayEntrance();
    }
}

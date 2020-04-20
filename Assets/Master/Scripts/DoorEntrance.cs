using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
public class DoorEntrance : MonoBehaviour
{
    public PlayableAsset Entrance;
    public PlayableAsset Exit;
    PlayableDirector TimelineEntrance;
    public Transform SpawnWarriorPos;
    public Transform SpawnHealerPos;
    public Transform parent;
    public GameObject prefabWarrior;
    public GameObject prefabHealer;
    void Awake()
    {
        TimelineEntrance = GetComponent<PlayableDirector>();
    }

    [ContextMenu("PlayEntrance")]

    public void PlayEntrance()
    {
        Debug.Log(TimelineEntrance);
        TimelineEntrance.Stop();
        TimelineEntrance.Play(Entrance);
        MusicController.instance.PlayAnSFX(MusicController.instance.WarriorStart);
        MusicController.instance.PlayAnSFX(MusicController.instance.DoorBreak);
    }
    public void PlayExit()
    {
        TimelineEntrance.Stop();
        TimelineEntrance.Play(Exit);
        MusicController.instance.PlayAnSFX(MusicController.instance.WarriorExit);
        MusicController.instance.PlayAnSFX(MusicController.instance.DoorBreak);
    }
    //Utilisé dans des timelines
    public void SpawnWarrior()
    {
        GameObject.Instantiate(prefabWarrior, parent);
    }
    
    public void SpawnHealer()
    {
        GameObject.Instantiate(prefabHealer, parent);
    }
}
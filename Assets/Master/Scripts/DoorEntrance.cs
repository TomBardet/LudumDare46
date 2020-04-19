using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
public class DoorEntrance : MonoBehaviour
{

    PlayableDirector TimelineEntrance;
    public Transform SpawnWarriorPos;
    public Transform SpawnHealerPos;
    public Transform parent;
    public GameObject prefabWarrior;
    public GameObject prefabHealer;
    void Start()
    {
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

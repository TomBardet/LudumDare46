using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    //FMOD Parameters
    [Header("---------------------- Music ----------------------")]
    [FMODUnity.EventRef]
    public string MusicLvl = "";
    public FMOD.Studio.EventInstance musicLvl;
    [FMODUnity.EventRef]
    public string MusicMenu = "";
    public FMOD.Studio.EventInstance musicMenu;
    [FMODUnity.EventRef]
    public string MusicFight = "";
    public FMOD.Studio.EventInstance musicFight;
    [FMODUnity.EventRef]
    public string MusicDanger = "";
    public FMOD.Studio.EventInstance musicDanger;

    [Header("----------------------- SFX -----------------------")]
    [FMODUnity.EventRef]
    public string RandomSfx = "";

    private void Start() {
        musicLvl = FMODUnity.RuntimeManager.CreateInstance(MusicLvl);
        //musicMenu = FMODUnity.RuntimeManager.CreateInstance(MusicMenu);
        //musicFight = FMODUnity.RuntimeManager.CreateInstance(MusicFight);
        //musicDanger = FMODUnity.RuntimeManager.CreateInstance(MusicDanger);
    }

    public void PlayAnSFX(string SfxPath) {
        FMODUnity.RuntimeManager.PlayOneShot(SfxPath);
    }
}

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
    

    [Header("----------------------- SFX -----------------------")]
    [FMODUnity.EventRef]
    public string Resurection = "";

    private void Start() {
        musicLvl = FMODUnity.RuntimeManager.CreateInstance(MusicLvl);
        musicMenu = FMODUnity.RuntimeManager.CreateInstance(MusicMenu);
	musicMenu.start();
    }

    public void PlayAnSFX(string SfxPath) {
        FMODUnity.RuntimeManager.PlayOneShot(SfxPath);
    }
}

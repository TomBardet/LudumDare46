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

    [Header("-------------------- Parameters -------------------")]
    public int FightLife = 100;

    [Header("----------------------- SFX -----------------------")]
    [FMODUnity.EventRef]
    public string Resurection = "";

    private void Start() {
        musicLvl = FMODUnity.RuntimeManager.CreateInstance(MusicLvl);
        musicMenu = FMODUnity.RuntimeManager.CreateInstance(MusicMenu);
	    musicMenu.start();
    }

    public void SetLifeParameters(float warriorLife) {
        FightLife =(int)warriorLife / Warrior.instance.maxHp * 100;
        musicLvl.setParameterByName("Life", FightLife);
    }

    public void PlayAnSFX(string SfxPath) {
        FMODUnity.RuntimeManager.PlayOneShot(SfxPath);
    }
}

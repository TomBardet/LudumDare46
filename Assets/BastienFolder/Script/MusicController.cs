using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public static MusicController instance;

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

    [Header("--------------------- Warrior ---------------------")]
    [FMODUnity.EventRef]
    public string WarriorDeath = "event:/Personnage/Guerrier_Death";
    [FMODUnity.EventRef]
    public string WarriorEat = "event:/Personnage/Guerrier_Eat";
    [FMODUnity.EventRef]
    public string WarriorExit = "event:/Personnage/Guerrier_End";
    [FMODUnity.EventRef]
    public string WarriorFind = "event:/Personnage/Guerrier_Find";
    [FMODUnity.EventRef]
    public string WarriorHit = "event:/Personnage/Guerrier_Hit";
    [FMODUnity.EventRef]
    public string WarriorStart = "event:/Personnage/Guerrier_Start";

    [Header("---------------------- Enemy ----------------------")]
    [FMODUnity.EventRef]
    public string GoblinDeath = "event:/Personnage/Goblin_Death";
    [FMODUnity.EventRef]
    public string GoblinHit = "event:/Personnage/Goblin_Hit";
    [FMODUnity.EventRef]
    public string GoblinTrigger = "event:/Personnage/Goblin_Trigger";

    [Header("--------------------- Object ---------------------")]
    [FMODUnity.EventRef]
    public string DoorBreak = "event:/Sfx/PorteBreak";
    [FMODUnity.EventRef]
    public string Lever = "event:/Sfx/Levier";
    [FMODUnity.EventRef]
    public string Manivelle = "event:/Sfx/Manivelle";
    public FMOD.Studio.EventInstance manivelle;
    [FMODUnity.EventRef]
    public string DragObject = "event:/Sfx/ObjetDrag";
    [FMODUnity.EventRef]
    public string DropObject = "event:/Sfx/ObjetDrop";
    [FMODUnity.EventRef]
    public string PressurePlateOn = "event:/Sfx/PlaqueOn";
    [FMODUnity.EventRef]
    public string PressurePlateOff = "event:/Sfx/PlaqueOff";
    [FMODUnity.EventRef]
    public string TurretShot = "event:/Sfx/TourelleTir";
    [FMODUnity.EventRef]
    public string Chest = "event:/Sfx/Coffre";
    [FMODUnity.EventRef]
    public string ChestExplosion = "event:/Sfx/CoffreBoom";

    [Header("--------------------- Healer ---------------------")]
    [FMODUnity.EventRef]
    public string Resurection = "event:/Sfx/Resurection";
    [FMODUnity.EventRef]
    public string Heal = "event:/Sfx/Heal";
    [FMODUnity.EventRef]
    public string HealerDeath = "event:/Personnage/Healer_Death";

    private void Awake() {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start() {
        musicLvl = FMODUnity.RuntimeManager.CreateInstance(MusicLvl);
        musicMenu = FMODUnity.RuntimeManager.CreateInstance(MusicMenu);
        manivelle = FMODUnity.RuntimeManager.CreateInstance(Manivelle);
        musicMenu.start();
    }

    public void SetLifeParameters(float warriorLife, int warriorMaxHp) {
        FightLife =(int)warriorLife / warriorMaxHp * 100;
        musicLvl.setParameterByName("Life", FightLife);
    }

    public void PlayAnSFX(string SfxPath) {
        FMODUnity.RuntimeManager.PlayOneShot(SfxPath);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRef : MonoBehaviour
{
    public static UIRef instance;

    public Slider manaBar;
    public Slider healthBar;

    void Awake()
    {
        instance = this;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Editor_Dalles : MonoBehaviour
{
    public Sprite[] spr_dalles;
    public Material[] m_dalles;

    public Sprite selected_spr;
    public Material selected_m;

    public SpriteRenderer spr;

    [ContextMenu("RandomizeDalles")]
    void RandomizeDalles()
    {
        int d = Random.Range(0, spr_dalles.Length);

        selected_m = m_dalles[d];
        selected_spr = spr_dalles[d];

        spr.sprite = selected_spr;
        spr.material = selected_m;
    }



}

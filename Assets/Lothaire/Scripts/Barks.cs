using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum E_Barks { Chest, Sandswitch, Enemies, Scanning };

public class Barks : MonoBehaviour
{
    public float barkDuration;
    public string[] ChestBarks;
    public string[] EnemiesBarks;
    public string[] SandswitchBarks;
    public string[] scanningBarks;

    TextMeshProUGUI barksText;

    public void Awake()
    {
        barksText = GetComponent<TextMeshProUGUI>();
    }

    public void ScreamBark(E_Barks type)
    {
        StopAllCoroutines();
        switch(type)
        {
            case E_Barks.Chest:
                StartCoroutine(Barking(ChestBarks[Random.Range(0, ChestBarks.Length)]));
                break;
            case E_Barks.Enemies:
                StartCoroutine(Barking(EnemiesBarks[Random.Range(0, EnemiesBarks.Length)]));
                break;
            case E_Barks.Sandswitch:
                StartCoroutine(Barking(SandswitchBarks[Random.Range(0, SandswitchBarks.Length)]));
                break;
            case E_Barks.Scanning:
                StartCoroutine(Barking(scanningBarks[Random.Range(0, scanningBarks.Length)]));
                break;
            default:
                break;
        }
    }

    IEnumerator Barking(string bark)
    {
        float time = barkDuration;
        barksText.enabled = true;
        barksText.text = bark;
        while (time > 0)
        {
            time -= 1 * Time.deltaTime;
            yield return null;
        }
        barksText.enabled = false;
    }
}

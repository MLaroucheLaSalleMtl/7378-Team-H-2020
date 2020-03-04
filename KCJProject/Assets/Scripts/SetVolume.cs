using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// THIS SCRIPT WAS TAKEN FROM AN EXCERCISE BY MARC-ANDRE LAROUCHE IN THE FALL 2019 SEMESTER

[RequireComponent(typeof(Slider))]
public class SetVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMix;
    [SerializeField] private string parameterName;

    public void SetVol(float vol)
    {
        Slider slide = GetComponent<Slider>();
        audioMix.SetFloat(parameterName, vol);
        slide.value = vol;
        PlayerPrefs.SetFloat(parameterName, vol);
        PlayerPrefs.Save();
    }

    // Start is called before the first frame update
    void Start()
    {
        Slider slide = GetComponent<Slider>();
        float v = PlayerPrefs.GetFloat(parameterName, 0);
        slide.value = v;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

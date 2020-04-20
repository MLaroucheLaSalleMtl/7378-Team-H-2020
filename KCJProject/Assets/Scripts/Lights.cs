using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lights : MonoBehaviour
{
    public void LightOn() {
        Debug.Log("Lights");
        Light[] lights = GetComponentsInChildren<Light>();
        foreach (Light l in lights) l.enabled = true;
    }
}

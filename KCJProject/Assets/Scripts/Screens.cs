using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screens : MonoBehaviour
{
    
    [SerializeField] private Color color;
    

    private void LightOn() {

        Debug.Log("screens");
        Material screen = GetComponents<Material>()[1];

        Color brighter = color * 0.4f;
        screen.SetColor("_EmissionColor", brighter);
    }
}

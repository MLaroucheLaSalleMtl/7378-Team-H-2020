///Summary
///Saves and Recover player position and rotation
///by Caue Pizzol
/// 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPos : MonoBehaviour
{   
    public void SetPosition(Vector3 pos) {
        PlayerPrefs.SetFloat("posX", pos.x);
        PlayerPrefs.SetFloat("posY", pos.y);
        PlayerPrefs.SetFloat("posZ", pos.z);
    }

    public void SetRotation(Quaternion rot) {
        PlayerPrefs.SetFloat("rotX", rot.x);
        PlayerPrefs.SetFloat("rotY", rot.y);
        PlayerPrefs.SetFloat("rotZ", rot.z);
        PlayerPrefs.SetFloat("rotW", rot.w);
    }

    //Recovers player position from disk
    public Vector3 GetPosition() {        
        return new Vector3(PlayerPrefs.GetFloat("posX", 0), PlayerPrefs.GetFloat("posY", 0), PlayerPrefs.GetFloat("posZ", 0));
    }
    //Recovers player rotation from disk
    public Quaternion GetRotation() {
        return new Quaternion(PlayerPrefs.GetFloat("rotX", 0), PlayerPrefs.GetFloat("rotY", 0), PlayerPrefs.GetFloat("rotZ", 0), PlayerPrefs.GetFloat("rotW", 0));
    }

    //Writes player position and rotation in disk
    public void Write() {
        PlayerPrefs.Save();
    }

}

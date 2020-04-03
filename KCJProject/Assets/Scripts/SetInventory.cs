///Summary
///Write and read the inventory in string form
///Items are represented in integer form, separated by a ';'
///by caue
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetInventory : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetInv(List<GameObject> inv) {
        string invS = "";
        foreach (GameObject i in inv) {
            try {
                invS = i.GetComponent<Grabbable>().GetID() + ";";
            }
            catch {
                Debug.Log("Error saving inventory");
            }
        }
        PlayerPrefs.SetString("inventory", invS);
    }
    public int[] GetInv() {
        string inv = PlayerPrefs.GetString("inventory", "");
        string[] invS = inv.Split(';');
        int[] invI = new int[invS.Length];
        for(int i = 0; i < invS.Length; i++) {
            try {
                invI[i] = System.Convert.ToInt32(invS[i]);
            }
            catch {
                Debug.Log("Error retrieving inventory");
            }
        }
        return invI;
    }
    public void Write() {
        PlayerPrefs.Save();
    }
}

///List of Itens by ID: (for reference)
///0 - Wrench
///1 - Paddle

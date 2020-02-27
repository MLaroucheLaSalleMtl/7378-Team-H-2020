using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuMain : MonoBehaviour
{
    [SerializeField] private GameObject[] panels;
    [SerializeField] private Selectable[] defaultButtons;

    public void PanelToggle(int position)
    {
        Input.ResetInputAxes();
        for(int i = 0; i< panels.Length; i++)
        {
            panels[i].SetActive(position == i); 
            if(position == i)
            {
                defaultButtons[i].Select();
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Invoke("PanelToggle", 0.01f);
    }
    void PanelToggle()
    {
        PanelToggle(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SavePrefs()
    {
        PlayerPrefs.Save();
    }
}

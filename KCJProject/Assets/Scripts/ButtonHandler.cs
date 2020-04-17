using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

// This script was modified to fit an excercise given by marc-andre larouche in the fall 2019 semester
// This script was modified on APRIL 11 by Julien Gagnier because it was not working properly -- Had to get it 
// working in order to properly implement gamepad support

public class ButtonHandler : MonoBehaviour, IPointerEnterHandler, IDeselectHandler, IPointerDownHandler
{
    private GameManager code;


    void Start() {
        code = GameManager.instance;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        GetComponent<Selectable>().OnPointerExit(null);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GetComponent<Button>() != null)
        {
            GetComponent<Button>().onClick.Invoke();
            Input.ResetInputAxes();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Selectable>().Select();
    }

    public void UnPause() {
        try {
            code.PauseGame(false);
            SceneManager.UnloadSceneAsync("PauseMenu");
        }
        catch {
            Debug.Log("Scene not loaded");
        }
    }
}

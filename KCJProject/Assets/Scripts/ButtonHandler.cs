using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    public void OnDeselect(PointerEventData eventData)
    {
        Debug.Log("Deselected");
        GetComponent<Selectable>().OnPointerExit(null);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.selectedObject.GetComponent<Button>() != null)
        {
            GetComponent<Button>().onClick.Invoke();
            Input.ResetInputAxes();
        }
        Debug.Log(this.gameObject.name + "Clicked");
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Selectable>().Select();
    }

    public void UnPause() {
        try {
            SceneManager.UnloadSceneAsync("PauseMenu");
        }
        catch {
            Debug.Log("Scene not loaded");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToGame : MonoBehaviour
{
    protected static GameManager code;
    [SerializeField] private GameObject exitPanel;
    // Start is called before the first frame update

    void Start()
    {
        code = GameManager.instance;
    }

    public void StayOnIsland()
    { 
        code.PauseGame(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        exitPanel.gameObject.SetActive(false);
    }
}

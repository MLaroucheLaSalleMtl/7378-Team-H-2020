using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EndGame : InteractableHandler
{
    private static GameManager code;
    [SerializeField] private GameObject FinalExitMenu;
   

    new void Start() {
        base.Start();
        code = GameManager.instance;
        FinalExitMenu.SetActive(false);
    }

    public override void DoInteraction() {
        base.DoInteraction();
        CreditPanel();
    }

    void CreditPanel()
    {
        code.ActivateController(false);
        FinalExitMenu.SetActive(true);
        code.PauseGame(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}

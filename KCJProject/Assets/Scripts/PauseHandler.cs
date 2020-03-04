using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseHandler : MonoBehaviour
{
    private bool isPaused = false;
    private GameManager code;
    private AsyncOperation async;
    // Start is called before the first frame update
    void Start()
    {
        code = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEscape(InputAction.CallbackContext context) {
        if (code.isInventory) return;
        if (context.performed) {
            isPaused = !isPaused;
            PauseGame(isPaused);
        }
    }
    // handles pause game
    public void PauseGame(bool pause) {
        code.PauseGame(pause);
        if (pause) {
            async = SceneManager.LoadSceneAsync("PauseMenu", LoadSceneMode.Additive); // load the new scene
            async.allowSceneActivation = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else {
            code.PauseGame(false);
            try {
                SceneManager.UnloadSceneAsync("PauseMenu");
            }
            catch {
                isPaused = !isPaused;
                PauseGame(isPaused);
            }
        }
    }
}

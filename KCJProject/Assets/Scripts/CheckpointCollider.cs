using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointCollider : MonoBehaviour
{
    private CheckpointHandler checkPoint;
    private GameManager code;

    // Start is called before the first frame update
    void Start()
    {
        checkPoint = CheckpointHandler.instance;
        code = GameManager.instance;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            checkPoint.checkPointEnable = true;
            //checkPoint.lastCheckPointPos = transform.position;
            checkPoint.lastCheckPointPos = code.Player.transform.position; // * modified to get player position when passing through, not checkpoint position -- Caue
            checkPoint.lastCheckPointRot = code.Player.GetComponentInChildren<Camera>().transform.localRotation; // also grabs rotation --caue
            checkPoint.GetCurrentHealth();
            checkPoint.Save();

            checkPoint.checkPointAlert.Play();
            checkPoint.checkpointPanel.SetActive(true);
            Invoke("DeactivatePanel", 4f);

            Debug.Log("CHECKPOINT REACHED");
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            gameObject.SetActive(false);
    }

    private void DeactivatePanel()
    {
        checkPoint.checkpointPanel.SetActive(false);
    }
}

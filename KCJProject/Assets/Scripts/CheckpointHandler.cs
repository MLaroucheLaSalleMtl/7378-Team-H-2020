using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointHandler : MonoBehaviour
{
    public static CheckpointHandler instance = null;

    [SerializeField] private GameObject spawnPoint;
    [Tooltip("Attach the prefab checkpoint panel")] [SerializeField] public GameObject checkpointPanel;
    [Tooltip("Attach the check point alert sound here")] [SerializeField] public AudioSource checkPointAlert;
    public Vector3 lastCheckPointPos;
    public Quaternion lastCheckPointRot;
    public bool checkPointEnable = false;
    public float curHealth;

    private static GameManager code;
    private static InventoryHandler ivn;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        code = GameManager.instance;
        ivn = InventoryHandler.instance;
    }


    void Start()
    {
        checkpointPanel.SetActive(false);
        code = GameManager.instance;
        ivn = InventoryHandler.instance;
    }

    public void Spawn() // spawning the player at the beginning of the level
    {
        code.Player.transform.position = spawnPoint.transform.position;
        code.Player.SetActive(true);
        lastCheckPointPos = code.Player.transform.position;
        lastCheckPointRot = code.Player.GetComponentInChildren<Camera>().transform.localRotation;
        Save();
    }

    public void CheckPointSpawn()
    {
        if (checkPointEnable) //respawn in checkpoint
        {
            code.isDeath = false;
            Read();//every time the player respawns, grab saved position
            code.DeactivateDeath();
            code.Player.SetActive(false);
            code.Player.transform.position = lastCheckPointPos;
            code.Player.transform.localRotation = lastCheckPointRot;
            code.Player.SetActive(true);
            Invoke("TurnResetOff", 2f); // Had to wait some time before turning the game manager reset off or it happens between frames and does not stop the update inside flowerdamage

        }
        else if (!checkPointEnable) //respawn in first spawn
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            code.isDeath = false;
        }
    }

    public void GetCurrentHealth()
    {
        curHealth = code.Health;
    }

    public void TurnResetOff()
    {
        code.reset = false;
    }
    //Uses player prefs to save positioning,rotation and inventory on checkpoint - caue
    public void Save() {
        code.setPos.SetPosition(lastCheckPointPos);
        code.setPos.SetRotation(lastCheckPointRot);
        code.setInventory.SetInv(ivn.inventory);
        code.setLevel.SetHealth(curHealth);
        BroadcastMessage("Write", SendMessageOptions.DontRequireReceiver);
    }

    //Reads player prefs for last saved checkpoint - caue
    public void Read() {
        lastCheckPointPos = code.setPos.GetPosition();
        lastCheckPointRot = code.setPos.GetRotation();
        ivn.AddFromDisk(code.setInventory.GetInv());
        code.GiveHealth(code.setLevel.GetHealth());
    }
}

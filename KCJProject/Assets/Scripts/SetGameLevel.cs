using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGameLevel : MonoBehaviour
{
    private static GameManager code;
    // Start is called before the first frame update
    void Start()
    {
        code = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetLevel() {
        PlayerPrefs.SetInt("level", code.Level);
    }
    public void SetHealth(float health) {
        PlayerPrefs.SetFloat("hp", health);
    }
    public int GetLevel() {
        return PlayerPrefs.GetInt("level", 1);
    }
    public float GetHealth() {
        return PlayerPrefs.GetFloat("hp", 100f);
    }
    public void Write() {
        PlayerPrefs.Save();
    }
}

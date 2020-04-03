using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class FlowerDamage : MonoBehaviour
{
    private GameManager code;
    private bool flowerCollision = false;

    [Header("Damage Grunt Sound Effects")] [SerializeField] private AudioSource playerAudioSource;
    [SerializeField] private AudioClip[] damageGruntClips;

    // Start is called before the first frame update
    void Start()
    {
        code = GameManager.instance;
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine("DamageOverTime");

        if (code.reset == true)
        {
            flowerCollision = false;
            playerAudioSource.Stop();
            code.damagePanel.SetActive(false);
            code.StopDamageAnim();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        code.damagePanel.SetActive(true);
        flowerCollision = true;
    }

    private void OnTriggerExit(Collider other)
    {
        code.damagePanel.SetActive(false);
        code.StopDamageAnim();
        flowerCollision = false;
    }

    private void DamageSound()
    {
        int rand = Random.Range(0, damageGruntClips.Length);
        
        if (playerAudioSource.isPlaying)
        {
            // Nothing happens if it is already playing
            // Need to find a way to add a delay
            
        } else
        {
            playerAudioSource.PlayOneShot(damageGruntClips[rand]);
        }
    }

    IEnumerator DamageOverTime()
    {
        while (flowerCollision == true)
        {
            DamageSound();
            code.FlowerDamage(30f);
            yield return new WaitForSeconds(0.9f);
        }
    }
}

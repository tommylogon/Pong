using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] hitSounds;
    [SerializeField] AudioClip scoreSound;
    [SerializeField] AudioClip countDownSound;
    [SerializeField] AudioClip gameOverSound;

    private void Start()
    {
        instance = this;
    }



    public void PlayHitSound()
    {
        if(hitSounds.Length > 0)
        {
            int index = Random.Range(0, hitSounds.Length - 1);
            AudioClip sound = hitSounds[index];
            if(sound != null ) 
            {
                audioSource.PlayOneShot(sound);
            }
            else
            {
                Debug.Log("Hitsound missing at index " + index);
            }
            
        }
    }

    public void PlayScoreSound()
    {
        if(scoreSound != null)
        {
            audioSource.PlayOneShot(scoreSound);
        }
    }
    public void PlayCountDownSound()
    {
        if(countDownSound != null)
        {
            audioSource.PlayOneShot(countDownSound);
        }
    }

    public void PlayGameOverSound()
    {
        if (gameOverSound != null)
        {
            audioSource.PlayOneShot(gameOverSound);
        }
    }
}

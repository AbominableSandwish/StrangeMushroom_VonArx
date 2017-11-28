using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerPlayer : MonoBehaviour {

    [SerializeField] private AudioClip[] tableSound;
    private AudioSource audioSource;

    enum State
    {
        HURT = 0
    }

    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(int state)
    {
        int indexSoundRandom = 0;
        switch (state)
        {
            case 0:
                indexSoundRandom = 0;
                break;
        }

        audioSource.clip = tableSound[indexSoundRandom];
        audioSource.Play();
        //audioSource.Stop();
    }
}

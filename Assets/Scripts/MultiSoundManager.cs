using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiSoundManager : MonoBehaviour {

    [SerializeField] private AudioClip[] tableSound;
    private AudioSource audioSource;

    enum State
    {
        HURT = 0,
        SPELL = 1,
        BOW = 2,
        SWORD = 3,
        WIN= 4,
        DIE = 5
    }

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
	}
	
	public void PlaySound(int state)
    {
        int indexSoundRandom=0;
        switch (state)
        {
            case 1:
                indexSoundRandom = 0;
                break;
            case 2:
                indexSoundRandom = Random.Range(0, 2) + 1;
                break;
            case 0:
                indexSoundRandom = Random.Range(0, 2) + 4;
                break;
        }
        
        audioSource.clip = tableSound[indexSoundRandom];
        audioSource.Play();
        //audioSource.Stop();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiSoundManager : MonoBehaviour {

    [SerializeField] private AudioClip[] tableSound;
    private AudioSource audioSource;

    enum State
    {
        IN_WAIT,
        HURT,
        ATTACK,
        WIN,
        LOST
    }
    State state = State.IN_WAIT;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
	}
	
	public void PlaySound()
    {
        //int indexSoundRandom = Random.Range(0, tableSound.Length);
        //audioSource.clip = tableSound[indexSoundRandom];
        //audioSource.Play();
        //audioSource.Stop();
    }
}

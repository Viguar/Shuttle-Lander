using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFader2D : MonoBehaviour
{
    private AudioSource audioSource;
    private float minDist;
    private float maxDist;
    private float volumeAtStart;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        minDist = audioSource.minDistance;
        maxDist = audioSource.maxDistance;
        volumeAtStart = audioSource.volume;
    }

    private void Update()
    {
        float distance = Vector3.Distance(GameObject.FindAnyObjectByType<AudioListener>().gameObject.transform.position, transform.position);
        if (distance > maxDist) { audioSource.volume = 0 * volumeAtStart; }
        else if (distance < maxDist) { audioSource.volume = 1 * volumeAtStart; }
        else { audioSource.volume = (1 - ((distance - minDist) / (maxDist - minDist))) * volumeAtStart;  }
    }

}

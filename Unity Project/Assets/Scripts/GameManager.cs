using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{   
    public GameObject player;

    // Pickup and Level Completion Logic
    public int currentPickups = 0;
    public int totalPickups = 5;
    public bool levelComplete = false;
    public Text pickupText;


    //Audio Proximity Logic
    public AudioSource[] audioSources;
    public float audioProximity = 100.0f;



    // Update is called once per frame
    void Update()
    {
        LevelCompleteCheck();
        UpdateGUI();
        PlayAudioSamples();
    }

    private void LevelCompleteCheck()
    {
        if (currentPickups >= totalPickups)
        {
            levelComplete = true;
        }
        else
        {
            levelComplete = false;
        }
    }

    private void UpdateGUI()
    {
        pickupText.text = "Pickups: " + currentPickups + "/" + totalPickups;

        if (levelComplete)
        {
            levelComplete = true;
        }
        else
        {
            levelComplete = false;
        }
    }

    private void PlayAudioSamples()
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            float distance = Vector3.Distance(player.transform.position, audioSources[i].transform.position);
            if (distance <= audioProximity)
            {
                if (!audioSources[i].isPlaying)
                {
                    audioSources[i].Play();
                    Debug.Log($"Playing audio {i} at distance {distance}");
                }
            }
            else
            {
                if (audioSources[i].isPlaying)
                {
                    audioSources[i].Stop();
                    Debug.Log($"Stopping audio {i} at distance {distance}");
                }
            }
        }
    }
}

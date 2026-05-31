using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TopherGameManager : MonoBehaviour
{   
    public GameObject player;

    // Pickup and Level Completion Logic
    public int currentPickups = 0;
    public bool doorOpen = false;
    public bool exitOpen = false;
    public bool levelComplete = false;
    public int totalPickups = 5;
    
    public Text pickupText;
    public Text objectText;


    //Audio Proximity Logic
    public AudioSource[] audioSources;
    public float audioProximity = 50.0f;

    private GameObject currentFocusTarget;

    private bool levelCompleteMessageShown = false;


    void Update()
    {
        LevelCompleteCheck();
        UpdateGUI();
        PlayAudioSamples();
        doorOpen = currentPickups > 0;
        exitOpen = currentPickups >= totalPickups;
    }
    private void LevelCompleteCheck()
    {
        if (currentPickups == totalPickups)
        {
            if (!levelCompleteMessageShown)
            {
                levelComplete = true;
                exitOpen = true;
                objectText.text = "All pickups collected! Find the exit!";
                StartCoroutine(WaitAndClearText(3));
                levelCompleteMessageShown = true;
            }
        }
        else
        {
            levelComplete = false;
            levelCompleteMessageShown = false;
            exitOpen = false;
        }
    }

    private IEnumerator WaitAndClearText(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (objectText != null)
            objectText.text = "";
    }
    public void ShowObjectText(GameObject source, string message)
    {
        if (objectText == null) return;
        currentFocusTarget = source;
        objectText.text = message;
    }

    public void ClearObjectText(GameObject source)
    {
        if (objectText == null) return;
        if (currentFocusTarget == source)
        {
            objectText.text = "";
            currentFocusTarget = null;
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
            if (audioSources[i] != null)
            {
                float distance = Vector3.Distance(player.transform.position, audioSources[i].transform.position);
                if (distance <= audioProximity)
                {
                    if (!audioSources[i].isPlaying )
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
}

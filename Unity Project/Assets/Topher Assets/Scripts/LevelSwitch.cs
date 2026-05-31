using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TopherLevelSwitch : MonoBehaviour
{
    TopherGameManager gameManager;
    public string nextLevel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    var managerObject = GameObject.FindGameObjectWithTag("TopherGameManager");
    if (managerObject == null)
    {
        Debug.LogError("TopherGameManager tag not found");
        return;
    }

    gameManager = managerObject.GetComponent<TopherGameManager>();
    if (gameManager == null)
    {
        Debug.LogError("TopherGameManager component not found");
    }
}

    private void OnTriggerEnter(Collider otherObject){
        if (gameManager == null)
        {
            Debug.LogError("gameManager is null");
            return;
        }

        Debug.Log($"Hit: {otherObject.tag}, exitOpen: {gameManager.exitOpen}");
    
        if (otherObject.transform.tag == "Player" && gameManager.exitOpen)
        {
            SceneManager.LoadScene(nextLevel);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticCameraMovement : MonoBehaviour{

    public GameObject[] cameraNodes;
    private int cameraIndex = 0;

    public GameObject[] objects;

    private float proximity = 0.1f;
    public float moveSpeed = 5.0f;
    public float rotSpeed = 5.0f;
    private float adjRotSpeed;
    private Quaternion targetRotation;
    public float waitTime = 0.50f; // Time to wait at each node before moving to the next
    private bool isWaiting = false;

    // Start is called before the first frame update
    void Start(){
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update(){

        Movement();
    }

    private void Movement() {

        // Check if camera is close to the current node
        if (Vector3.Distance(transform.position, cameraNodes[cameraIndex].transform.position) < proximity) {
            if (!isWaiting) {
                isWaiting = true;
                StartCoroutine(WaitAndMove());
            }
        } else {
            isWaiting = false; // Reset if not close (though unlikely)
        }

        // Move Camera towards Camera Index and Rotate Towards Object Index
        if (!isWaiting) {
            // Translation
            transform.position = Vector3.MoveTowards(transform.position, cameraNodes[cameraIndex].transform.position, moveSpeed * Time.deltaTime);

            // Rotation
            if (objects[cameraIndex]) {
                targetRotation = Quaternion.LookRotation(objects[cameraIndex].transform.position - transform.position);
                adjRotSpeed = Mathf.Min(rotSpeed * Time.deltaTime, 1);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, adjRotSpeed);
            }

            // Play Audio if contains Audio Source and is not playing
            if (objects[cameraIndex].GetComponent<AudioSource>() != null){
                if (!objects[cameraIndex].GetComponent<AudioSource>().isPlaying)
                    objects[cameraIndex].GetComponent<AudioSource>().Play();
            }
        }
    }

    private IEnumerator WaitAndMove() {
        yield return new WaitForSeconds(waitTime);
        // Move to the next node only if not at the last
        if (cameraIndex < cameraNodes.Length - 1) {
            cameraIndex++;
        }
        isWaiting = false;
    }
}
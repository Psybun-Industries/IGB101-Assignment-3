using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float rotateSpeed = 100.0f;
    // Update is called once per frame
    void Update()
    {
        RotateArtefact();
    }

    private void RotateArtefact()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }
}
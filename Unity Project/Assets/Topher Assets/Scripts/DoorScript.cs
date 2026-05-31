using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private Animation doorAnim;
    private bool doorOpen = false;
    private MeshCollider doorMesh;
    private Light doorLight;
    private Camera playerCamera;
    private TopherGameManager gm;
    private Transform playerTransform;

    void Start()
    {
        doorAnim = GetComponent<Animation>();
        doorLight = GetComponentInChildren<Light>();
        doorMesh = GetComponent<MeshCollider>();
        if (doorLight != null)
            doorLight.enabled = false;

        playerCamera = Camera.main;
        gm = FindObjectOfType<TopherGameManager>();
        playerTransform = gm != null ? gm.player?.transform : null;
    }

    void Update()
    {
        bool doorUnlocked = gm != null && gm.doorOpen;

        if (Input.GetKeyDown(KeyCode.F) && doorUnlocked)
        {
            if (!doorOpen)
            {
                doorAnim.Play("DoorOpen");
                doorOpen = true;
                doorMesh.enabled = false;
            }
            else
            {
                doorAnim.Play("DoorClose");
                doorOpen = false;
                doorMesh.enabled = true;
            }
        }

        if (doorLight != null)
            doorLight.enabled = doorUnlocked;

        if (gm == null || playerCamera == null || gm.objectText == null)
            return;

        if (doorUnlocked)
        {
            gm.ClearObjectText(gameObject);
            return;
        }

        Vector3 screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
        Ray ray = playerCamera.ScreenPointToRay(screenCenter);
        Debug.DrawRay(ray.origin, ray.direction * 20f, Color.red);

        RaycastHit[] hits = Physics.RaycastAll(ray, 35f);
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        bool hitDoor = false;
        foreach (var hit in hits)
        {
            if (playerTransform != null && hit.collider.transform.IsChildOf(playerTransform))
                continue;

            if (hit.collider.gameObject == gameObject || hit.collider.transform.IsChildOf(transform))
            {
                hitDoor = true;
                break;
            }

            break;
        }

        if (hitDoor)
            gm.ShowObjectText(gameObject, "This door is locked");
        else
            gm.ClearObjectText(gameObject);
    }
}
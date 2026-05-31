using UnityEngine;

public class ExitScript : MonoBehaviour
{
    public string lockedMessage = "The exit remains closed.";
    public string unlockedMessage = "Press F to open the exit.";
    public string openMessage = "Press F to close the exit.";

    private Animation exitAnim;
    private bool exitOpen = false;
    private MeshCollider exitMesh;
    private Light exitLight;
    private Camera playerCamera;
    private GameManager gm;
    private Transform playerTransform;

    void Start()
    {
        exitAnim = GetComponent<Animation>();
        exitLight = GetComponentInChildren<Light>();
        exitMesh = GetComponent<MeshCollider>();
        if (exitLight != null)
            exitLight.enabled = false;

        playerCamera = Camera.main;
        gm = FindObjectOfType<GameManager>();
        playerTransform = gm != null ? gm.player?.transform : null;
    }

    void Update()
    {
        if (gm == null || playerCamera == null || gm.objectText == null)
            return;

        bool exitUnlocked = gm.exitOpen;

        if (exitUnlocked)
        {
            if (exitLight != null)
                exitLight.enabled = true;
        }
        else
        {
            if (exitLight != null)
                exitLight.enabled = false;
        }

        Vector3 screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
        Ray ray = playerCamera.ScreenPointToRay(screenCenter);
        Debug.DrawRay(ray.origin, ray.direction * 20f, Color.red);

        RaycastHit[] hits = Physics.RaycastAll(ray, 35f);
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        bool hitExit = false;
        foreach (var hit in hits)
        {
            if (playerTransform != null && hit.collider.transform.IsChildOf(playerTransform))
                continue;

            if (hit.collider.gameObject == gameObject || hit.collider.transform.IsChildOf(transform))
            {
                hitExit = true;
                break;
            }

            break;
        }

        if (hitExit)
        {
            string msg = !exitUnlocked
                ? lockedMessage
                : exitOpen ? openMessage : unlockedMessage;

            gm.ShowObjectText(gameObject, msg);

            if (exitUnlocked && Input.GetKeyDown(KeyCode.F))
            {
                if (!exitOpen)
                {
                    exitAnim.Play("DoorOpen");
                    exitMesh.enabled = false;
                    exitOpen = true;
                }
                else
                {
                    exitAnim.Play("DoorClose");
                    exitMesh.enabled = true;
                    exitOpen = false;
                }
            }
        }
        else
        {
            gm.ClearObjectText(gameObject);
        }
    }
}
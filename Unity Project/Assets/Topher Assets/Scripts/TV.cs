using UnityEngine;

public class TV : MonoBehaviour
{
    private Camera playerCamera;
    private TopherGameManager gm;
    private Transform playerTransform;
  
    public static bool tvOFF = false;

    void Start()
    {
        playerCamera = Camera.main;
        gm = FindObjectOfType<TopherGameManager>();
        playerTransform = gm != null ? gm.player?.transform : null;
    }

    void Update()
    {
        if (playerCamera == null || gm == null || gm.objectText == null)
            return;

        Vector3 screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
        Ray ray = playerCamera.ScreenPointToRay(screenCenter);
        Debug.DrawRay(ray.origin, ray.direction * 20f, Color.red);

        RaycastHit[] hits = Physics.RaycastAll(ray, 100f);
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        bool hitTV = false;
        foreach (var hit in hits)
        {
            if (playerTransform != null && hit.collider.transform.IsChildOf(playerTransform))
                continue;

            if (hit.collider.gameObject == gameObject || hit.collider.transform.IsChildOf(transform))
            {
                hitTV = true;
                break;
            }

            // if the ray hit something else first, stop looking
            break;
        }

            if (hitTV){
            string message = tvOFF ? "The TV is off." : "The TV is broken. Unplug it? Press F";
            gm.ShowObjectText(gameObject, message);

            if (Input.GetKeyDown(KeyCode.F))
            {
                gm.currentPickups += 1;
                Destroy(this.gameObject);
            }
        }
        else
        {
            gm.ClearObjectText(gameObject);
        }
    }
}


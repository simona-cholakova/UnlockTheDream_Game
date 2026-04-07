using UnityEngine;
using System.Collections;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;
    private AudioSource hitSound;

    private AudioSource shootSound;

    float xRotation;
    float yRotation;

    public bool isPaused = false;

    public ParticleSystem hitEffect;

    [Header("Shooting")]
    public float shootDistance = 1000f;
    public LayerMask hitLayers;
    

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (hitEffect != null)
        {
            hitSound = hitEffect.GetComponent<AudioSource>();
            hitEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    void Update()
    {
        //click to lock cursor again
        if (Input.GetMouseButtonDown(0) && !isPaused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        //ESC unlocks cursor anytime
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (isPaused) return;

        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

        if (Input.GetKeyDown(KeyCode.G))
        {
            TryShoot();
        }
    }


    public void PlayHitEffect()
    {
        if (hitEffect != null)
        {
            hitEffect.transform.localPosition = new Vector3(0, 0, 1f);

            hitEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            hitEffect.Play();

            if (hitSound != null)
            {
                hitSound.PlayOneShot(hitSound.clip);
            }

            Invoke(nameof(StopHitEffect), 2f);
        }
    }

    void StopHitEffect()
    {
        if (hitEffect != null)
        {
            hitEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }


    void TryShoot()
    {
        if (!PlayerInventory.instance.hasPotion)
        {
            Debug.Log("NO POTION");
            return;
        }

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        Vector3 startPoint = transform.position
                           + transform.right * 0.4f
                           + transform.up * -0.3f;

        Vector3 endPoint = transform.position + transform.forward * shootDistance;

        if (Physics.Raycast(ray, out hit, shootDistance, ~0, QueryTriggerInteraction.Collide))
        {
            Debug.Log("Ray hit: " + hit.collider.name);

            FallingEnemy enemy = hit.collider.GetComponentInParent<FallingEnemy>();
            if (enemy != null)
            {
                //Debug.Log("Falling enemy hit!");
                Debug.Log("TRYING TO COUNT KILL");
                FinalKeyProgress.instance?.AddStormcallerKill();

                Destroy(enemy.gameObject);
                endPoint = hit.point;
            }
        }

        StartCoroutine(ShootLine(startPoint, endPoint));
    }

    IEnumerator ShootLine(Vector3 start, Vector3 end)
    {
        GameObject lineObj = new GameObject("ShotLine");
        LineRenderer lr = lineObj.AddComponent<LineRenderer>();

        lr.useWorldSpace = true;
        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        lr.startWidth = 0.15f;
        lr.endWidth = 0.1f;

        Material mat = new Material(Shader.Find("Sprites/Default"));
        lr.material = mat;

        float duration = 0.8f;
        float time = 0;

        //Color beamColor = new Color(1f, 0f, 0f); - red 
        Color beamColor = new Color(1f, 0.3f, 0.3f);

        while (time < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, time / duration);
            Color c = new Color(beamColor.r, beamColor.g, beamColor.b, alpha);
            lr.startColor = c;
            lr.endColor = c;

            time += Time.unscaledDeltaTime;
            yield return null;
        }

        Destroy(lineObj);
    }

}

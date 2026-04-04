using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;
    private AudioSource hitSound;

    float xRotation;
    float yRotation;

    public bool isPaused = false;

    public ParticleSystem hitEffect;

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
}
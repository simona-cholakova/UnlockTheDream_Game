using UnityEngine;

public class HintUIManager : MonoBehaviour
{
    public GameObject hintPanel;
    public PlayerCam playerCam;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (hintPanel.activeSelf)
                CloseHint();
            else
                OpenHint();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && hintPanel.activeSelf)
            CloseHint();
    }

    public void OpenHint()
    {
        hintPanel.SetActive(true);
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        playerCam.isPaused = true;
    }

    public void CloseHint()
    {
        hintPanel.SetActive(false);
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerCam.isPaused = false;
    }
}
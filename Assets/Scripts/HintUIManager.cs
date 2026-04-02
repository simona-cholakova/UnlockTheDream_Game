using UnityEngine;

public class HintUIManager : MonoBehaviour
{
    public GameObject hintPanel;
    public PlayerCam playerCam;

    // void Start()
    // {
    //     Cursor.lockState = CursorLockMode.Locked;
    //     Cursor.visible = false;
    // }

    public void OpenHint()
    {
        hintPanel.SetActive(true);
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        playerCam.isPaused = true; //stop camera
    }
    public void CloseHint()
    {
        hintPanel.SetActive(false);
        Time.timeScale = 1f;

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        playerCam.isPaused = false; // resume camera
    }
    public void ClickTest()
    {
        Debug.Log("UI CLICK WORKS");
    }
}
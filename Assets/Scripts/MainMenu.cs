using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject creditsPanel;
    public GameObject hintsPanel;

    void Start()
    {
        if (creditsPanel != null) creditsPanel.SetActive(false);
        if (hintsPanel != null) hintsPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePanel();
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ShowCredits()
    {
        creditsPanel.SetActive(true);
        hintsPanel.SetActive(false);
    }

    public void ShowHints()
    {
        hintsPanel.SetActive(true);
        creditsPanel.SetActive(false);
    }

    public void ClosePanel()
    {
        if (creditsPanel != null) creditsPanel.SetActive(false);
        if (hintsPanel != null) hintsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
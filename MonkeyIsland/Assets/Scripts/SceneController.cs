using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void ReturnToMainMenu()
    {
        Debug.Log("Returning to Main Menu...");
        SceneManager.LoadScene("MainMenu");
    }

    public void StartNewGame()
    {
        Debug.Log("Starting new game...");
        SceneManager.LoadScene("Game");
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }
}

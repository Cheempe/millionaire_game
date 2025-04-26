using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene"); 
    }
    public void GameInstructions()
    {
        SceneManager.LoadScene("GameInstructionsScene"); 
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}

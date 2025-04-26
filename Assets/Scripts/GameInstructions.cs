using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInstructions : MonoBehaviour
{
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

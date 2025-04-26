using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoseSceneManager : MonoBehaviour
{
    public TextMeshProUGUI loseMessageText;
    public Button playAgainButton;
    public Button exitButton;
    void Start()
    {
        int guaranteedPrize = PlayerPrefs.GetInt("GuaranteedPrize", 0);
        Debug.Log($"Игрок проиграл, гарантированная сумма: {guaranteedPrize}");

        loseMessageText.text = $"Вы проиграли! Ваш выигрыш: ${guaranteedPrize:N0}";

        PlayerPrefs.SetInt("GuaranteedPrize", 0);
        PlayerPrefs.Save();

    playAgainButton.onClick.AddListener(() =>
        {
            PlayerPrefs.SetInt("GuaranteedPrize", 0);
            PlayerPrefs.Save();
            SceneManager.LoadScene("GameScene");
        });
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}

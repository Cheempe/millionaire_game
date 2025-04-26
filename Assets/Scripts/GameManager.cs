using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public Button[] answerButtons;
    public TextMeshProUGUI prizeText;

    public QuestionData[] questions;
    public Image[] prizeBlocks;

    public Button fiftyFiftyButton;
    public Button audienceHelpButton;
    public Button callFriendButton;
    public Button takeMoneyButton;
    public TextMeshProUGUI helpText;

    private int currentQuestionIndex = 0;
    private int prize = 0;
    private int lastPassedPrize = 0;

    private int[] prizeSteps = { 100, 200, 300, 500, 1000, 2000, 4000, 8000, 16000, 32000, 64000, 125000, 250000, 500000, 1000000 };
    private int[] guaranteedPrizes = { 1000, 32000 };

    public Color activeColor = Color.yellow;
    public Color passedColor = Color.green;
    public Color defaultColor = Color.white;

    void Start()
    {
        PlayerPrefs.SetInt("GuaranteedPrize", 0);
        PlayerPrefs.SetInt("CurrentPrize", 0);
        PlayerPrefs.Save();

        if (questions.Length > 0 && prizeBlocks.Length == prizeSteps.Length)
        {
            LoadQuestion();
            UpdatePrizeTracker();

            fiftyFiftyButton.onClick.AddListener(UseFiftyFifty);
            audienceHelpButton.onClick.AddListener(UseAudienceHelp);
            callFriendButton.onClick.AddListener(CallFriend);
            takeMoneyButton.onClick.AddListener(TakeMoney);
        }
        else
        {
            Debug.LogError("Массив вопросов или блоков шкалы некорректный!");
        }
    }

    void LoadQuestion()
    {
        if (currentQuestionIndex < questions.Length)
        {
            var q = questions[currentQuestionIndex];

            questionText.text = q.questionText;

            for (int i = 0; i < answerButtons.Length; i++)
            {
                var buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.text = q.answers[i];
                }

                int index = i;
                answerButtons[i].onClick.RemoveAllListeners();
                answerButtons[i].onClick.AddListener(() => CheckAnswer(index));
                answerButtons[i].gameObject.SetActive(false);
            }

            helpText.gameObject.SetActive(false);
            UpdatePrizeDisplay();
            StartCoroutine(AnimateAnswerButtons());
        }
        else
        {
            SceneManager.LoadScene("WinScene");
        }
    }

    IEnumerator AnimateAnswerButtons()
    {
        foreach (var btn in answerButtons)
        {
            btn.transform.localScale = Vector3.zero;
            btn.gameObject.SetActive(true);
        }

        foreach (var btn in answerButtons)
        {
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * 5;
                btn.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
                yield return null;
            }
        }
    }

    void CheckAnswer(int index)
    {
        if (index == questions[currentQuestionIndex].correctAnswer)
        {
            lastPassedPrize = prizeSteps[currentQuestionIndex];
            prize = lastPassedPrize;

            PlayerPrefs.SetInt("CurrentPrize", prize);
            PlayerPrefs.Save();

            if (currentQuestionIndex == 4)
            {
                prize = guaranteedPrizes[0];
            }
            else if (currentQuestionIndex == 9)
            {
                prize = guaranteedPrizes[1];
            }

            currentQuestionIndex++;

            if (currentQuestionIndex >= questions.Length)
            {
                SceneManager.LoadScene("WinScene");
            }
            else
            {
                LoadQuestion();
                UpdatePrizeTracker();
            }
        }
        else
        {
            if (currentQuestionIndex >= 4)
            {
                int guaranteedPrize = (currentQuestionIndex >= 9) ? guaranteedPrizes[1] : guaranteedPrizes[0];
                PlayerPrefs.SetInt("GuaranteedPrize", guaranteedPrize);
                PlayerPrefs.Save();
            }

            SceneManager.LoadScene("LoseScene");
        }
    }

    void UpdatePrizeDisplay()
    {
        prizeText.text = $"Prize: ${prize:N0}";
    }

    void UpdatePrizeTracker()
    {
        for (int i = 0; i < prizeBlocks.Length; i++)
        {
            if (i < currentQuestionIndex)
            {
                prizeBlocks[i].color = passedColor;
            }
            else if (i == currentQuestionIndex)
            {
                prizeBlocks[i].color = activeColor;
                StartCoroutine(AnimateActivePrizeBlock(prizeBlocks[i]));
            }
            else
            {
                prizeBlocks[i].color = defaultColor;
            }
        }
    }

    IEnumerator AnimateActivePrizeBlock(Image block)
    {
        Vector3 originalScale = block.transform.localScale;
        Vector3 targetScale = originalScale * 1.1f;

        float time = 0f;
        while (time < 0.3f)
        {
            time += Time.deltaTime;
            block.transform.localScale = Vector3.Lerp(originalScale, targetScale, Mathf.PingPong(time * 3f, 1f));
            yield return null;
        }

        block.transform.localScale = originalScale;
    }

    void UseFiftyFifty()
    {
        var q = questions[currentQuestionIndex];

        var wrongAnswers = Enumerable.Range(0, answerButtons.Length)
                                     .Where(i => i != q.correctAnswer)
                                     .OrderBy(x => Random.value)
                                     .Take(2)
                                     .ToArray();

        foreach (var index in wrongAnswers)
        {
            answerButtons[index].gameObject.SetActive(false);
        }

        fiftyFiftyButton.interactable = false;
    }

    void UseAudienceHelp()
    {
        var q = questions[currentQuestionIndex];

        for (int i = 0; i < answerButtons.Length; i++)
        {
            var buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text += (i == q.correctAnswer) ? " (70%)" : " (10%)";
        }

        audienceHelpButton.interactable = false;
    }

    void CallFriend()
    {
        var q = questions[currentQuestionIndex];

        if (helpText != null)
        {
            int suggestion = Random.value > 0.7f ? Random.Range(0, answerButtons.Length) : q.correctAnswer;
            string[] possibleResponses = { $"Друг думає, що це: {questions[currentQuestionIndex].answers[suggestion]}" };
            helpText.text = possibleResponses[Random.Range(0, possibleResponses.Length)];

            StartCoroutine(FadeInHelpText());
        }

        callFriendButton.interactable = false;
    }

    IEnumerator FadeInHelpText()
    {
        CanvasGroup canvasGroup = helpText.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = helpText.gameObject.AddComponent<CanvasGroup>();
        }

        helpText.gameObject.SetActive(true);
        canvasGroup.alpha = 0;

        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime;
            yield return null;
        }
    }

    void TakeMoney()
    {
        PlayerPrefs.SetInt("FinalPrize", lastPassedPrize);
        PlayerPrefs.Save();

        SceneManager.LoadScene("TakeMoneyScene");
    }
}

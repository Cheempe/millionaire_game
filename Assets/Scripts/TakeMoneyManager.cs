using UnityEngine;
using TMPro;

public class TakeMoneyManager : MonoBehaviour
{
    public TextMeshProUGUI prizeText;

    void Start()
    {
        int takeMoneyPrize = PlayerPrefs.GetInt("FinalPrize", 0);

        if (prizeText != null)
        {
            prizeText.text = $"YOU WON: ${takeMoneyPrize:N0}";
        }
        else
        {
            Debug.LogWarning("PrizeText не призначений в інспекторі.");
        }
    }
}

using UnityEngine;
using TMPro;
using System.Collections;

public class WinSceneManager : MonoBehaviour
{
    public TextMeshProUGUI winText;

    void Start()
    {
        int finalPrize = 1000000;
        StartCoroutine(AnimatePrize(finalPrize));
    }

    IEnumerator AnimatePrize(int prize)
    {
        int displayed = 0;
        while (displayed < prize)
        {
            displayed += Mathf.Max(1000, (prize - displayed) / 10);
            displayed = Mathf.Min(displayed, prize);
            winText.text = $"YOU WON ${displayed:N0}!";
            yield return new WaitForSeconds(0.05f);
        }
    }
}

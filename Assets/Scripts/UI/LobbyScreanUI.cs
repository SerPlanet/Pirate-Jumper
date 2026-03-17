using UnityEngine;
using TMPro;
using System.Globalization;
using UnityEngine.UI;

public class LobbyScreanUI : MonoBehaviour
{
    [Header ("TextFields")]
    [SerializeField] private TextMeshProUGUI highScoreTextField;
    [SerializeField] private TextMeshProUGUI moneyScoreTextField;

    [Header ("Sounds")]
    [SerializeField] private AudioClip buttonSound;

    [Header ("Button")]
    [SerializeField] private Button openChest;

    public void PlayButtonSound()
    {
        AudioManager.Instance.PlayUIAudi(buttonSound,0.9f,1);
    }

    public void SetMoney(ulong money)
    {
       if (money < 1000)
            moneyScoreTextField.text = money.ToString("D3"); // 005, 042, 999
        else
            moneyScoreTextField.text = money.ToString("N0", CultureInfo.GetCultureInfo("de-DE"));
    }

    public void SetHighScore(ulong highscore)
    {
        highScoreTextField.text = highscore.ToString();
    }

    public void DeactivateBackground()
    {
        CameraManager.Instance.TriggerBackgreoundSwitch();
    }

    public void ShowCredits()
    {
        UIManager.Instance.ShowCreditUI();
    }

    public void ShowOptionsUI()
    {
        UIManager.Instance.ShowOptiontUI();
    }
    public void HideUI()
    {
        gameObject.SetActive(false);
    }

    public void ShowUI()
    {
        if (GameManager.Instance.GetMoney() >= 5000)
        {
            openChest.interactable = true;
        }
        else
        {
            openChest.interactable = false;
        }
        gameObject.SetActive(true);
    }

    public void StartGame(){GameManager.Instance.StartGame();}

    public void OpenCharachter(){GameManager.Instance.OpenCharachterSelectScrean();}

    public void OpenChestScrean(){GameManager.Instance.OpenChestScrean();}

    public void Reset()
    {
        
        moneyScoreTextField.text = "000";
        highScoreTextField.text = "0000";
        
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
public class InGameUI : MonoBehaviour
{
    [Header ("TextFields")]
    [SerializeField] private TextMeshProUGUI scoreTextField;
    [SerializeField] private TextMeshProUGUI moneyScoreTextField;

    [Header("Item")]

    [SerializeField] private GameObject itemObj;
    [SerializeField] private Image circleImage;
    [SerializeField] private Image itemImage;

    [SerializeField] private Image timerImage;
    [SerializeField] private Button pauseButton;

    [SerializeField] private GameObject reviveOptionAfterDeath;

    private float duration;

    [Header ("Sounds")]
    [SerializeField] private AudioClip PauseButton;


    public void PlayAudio()
    {
        AudioManager.Instance.PlayUIAudi(PauseButton,1,1);
    }
    public void SetScore(ulong score)
    {
         if (score < 1000)
            scoreTextField.text = score.ToString("D4"); // 0005
        else
            scoreTextField.text = score.ToString("N0", CultureInfo.GetCultureInfo("de-DE")); // 1.000
    }

    public void SetMoney(ulong money)
    {
        if (money < 1000)
            moneyScoreTextField.text = money.ToString("D3"); // 005
        else
            moneyScoreTextField.text = money.ToString("N0", CultureInfo.GetCultureInfo("de-DE")); // 1.000
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }

    public void ShowUI()
    {
        HideItem();
        reviveOptionAfterDeath.SetActive(false);
        gameObject.SetActive(true);
    }

    public void ShowAfterDeathReviveOption()
    {
        timerImage.fillAmount = 1f;
        reviveOptionAfterDeath.SetActive(true);
        pauseButton.interactable = false;
    }

    public void HideAfterDeathReviveOption()
    {
         pauseButton.interactable = true;
        reviveOptionAfterDeath.SetActive(false);
    }

    public void SetTimerOfClock(float time)
    {
        timerImage.fillAmount = time;
    }

    public void RevivePlayer()
    {
        if (GameManager.Instance.PurchaseRevive(1000))
        {
            UIManager.Instance.PlayerRevived();
            HideAfterDeathReviveOption();
            GameManager.Instance.GetPlayerScript().RevivePlayer();
        }
    }
    public void PerformJump()
    {
        InputManager.Instance.JumpPressed();
    }

    public void SetUpItem(Sprite iconItem, float itemDuration)
    {
        itemImage.sprite = iconItem;
        duration = itemDuration;
        circleImage.fillAmount = 1;
        ShowItem();
    }

    public void SetTimeItem(float time)
    {
        circleImage.fillAmount = 1 - (time/duration);
    }

    public void ShowItem(){itemObj.SetActive(true);}
    public void HideItem(){itemObj.SetActive(false);}

    public void PauseGame(){GameManager.Instance.PauseGame();}
    public void Reset()
    {
        scoreTextField.text = "0000";
        moneyScoreTextField.text = "000";
    }

}

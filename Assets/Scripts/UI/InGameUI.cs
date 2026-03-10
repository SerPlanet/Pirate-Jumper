using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [Header ("TextFields")]
    [SerializeField] private TextMeshProUGUI scoreTextField;
    [SerializeField] private TextMeshProUGUI moneyScoreTextField;

    [Header("Item")]

    [SerializeField] private GameObject itemObj;
    [SerializeField] private Image circleImage;
    [SerializeField] private Image itemImage;

    private float duration;

    [Header ("Sounds")]
    [SerializeField] private AudioClip PauseButton;


    public void PlayAudio()
    {
        AudioManager.Instance.PlayUIAudi(PauseButton,1,1);
    }
    public void SetScore(ulong score)
    {
        scoreTextField.text = score.ToString();
    }

    public void SetMoney(ulong money)
    {
        moneyScoreTextField.text = money.ToString();
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }

    public void ShowUI()
    {
        HideItem();
        gameObject.SetActive(true);
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

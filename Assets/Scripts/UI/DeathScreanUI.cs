using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathScreanUI : MonoBehaviour
{
    [Header ("TextFields")]
    [SerializeField] private TextMeshProUGUI scoreTextField;
    [SerializeField] private TextMeshProUGUI highScoreTextField;
    [SerializeField] private TextMeshProUGUI moneyScoreTextField;

    [Header ("Sounds")]
    [SerializeField] private AudioClip buttonSound;

    public void PlayButtonSound()
    {
        AudioManager.Instance.PlayUIAudi(buttonSound,0.9f,1);
    }
    public void SetScore(ulong score)
    {
        scoreTextField.text = score.ToString();
    }

    public void SetMoney(ulong money)
    {
        moneyScoreTextField.text = money.ToString();
    }

    public void SetHighScore(ulong highscore)
    {
        highScoreTextField.text = highscore.ToString();
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }

    public void ShowUI()
    {
        gameObject.SetActive(true);
    }

     public void HomeButton()
    {
        PlayButtonSound();
        GameManager.Instance.GameLobby();
    }
    public void PlayAgainButton()
    {
        PlayButtonSound();
        GameManager.Instance.LoadGame();
    }

    public void Reset()
    {
        scoreTextField.text = "0000";
        moneyScoreTextField.text = "000";
        highScoreTextField.text = "0000";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    [Header ("Sounds")]
    [SerializeField] private AudioClip buttonSound;

     public void PlayButtonSound()
    {
        AudioManager.Instance.PlayUIAudi(buttonSound,0.9f,1);
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }

    public void ShowUI()
    {
        gameObject.SetActive(true);
    }

    public void ResumeGame()
    {
        PlayButtonSound();
        GameManager.Instance.GameRunning();
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

    public void ToggleSFX()
    {
        AudioManager.Instance.ToggleSound();
    }

    public void ToggleMusic()
    {
        AudioManager.Instance.ToggleMusik();
    }

    public void ToggleUISound()
    {
        AudioManager.Instance.ToggleUISound();
    }

    public void Reset()
    {
        
    }

}

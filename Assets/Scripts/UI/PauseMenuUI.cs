using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    [Header ("Sounds")]
    [SerializeField] private AudioClip buttonSound;

    [SerializeField] private Animator sfxButtonAnimator;
    [SerializeField] private Animator musicButtonAnimator;

     public void PlayButtonSound()
    {
        AudioManager.Instance.PlayUIAudi(buttonSound,0.9f,1);
        (bool sfx, bool music) var = AudioManager.Instance.GetSoundOptions();
        SetUpSound(var.sfx, var.music);
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }

    public void ShowUI()
    {
        (bool sfx, bool music) var = AudioManager.Instance.GetSoundOptions();
        SetUpSound(var.sfx, var.music);
        gameObject.SetActive(true);
    }

    private void SetUpSound(bool sfx, bool music)
    {
        SetSFXButton(sfx);
        SetMusicButton(music);
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
        sfxButtonAnimator.SetTrigger("Pressed");
        AudioManager.Instance.ToggleSound();
    }

    public void ToggleMusic()
    {
        musicButtonAnimator.SetTrigger("Pressed");
        AudioManager.Instance.ToggleMusik();
    }

    public void ToggleUISound()
    {
        sfxButtonAnimator.SetTrigger("Pressed");
        AudioManager.Instance.ToggleUISound();
    }

    public void SetSFXButton(bool state)
    {
        Debug.Log(state);
        sfxButtonAnimator.SetBool("IsEnabled", state);
        sfxButtonAnimator.SetTrigger("Pressed");
    }

    public void SetMusicButton(bool state)
    {
        Debug.Log(state);
        musicButtonAnimator.SetBool("IsEnabled", state);
        musicButtonAnimator.SetTrigger("Pressed");
    }


    public void Reset()
    {
        
    }

}

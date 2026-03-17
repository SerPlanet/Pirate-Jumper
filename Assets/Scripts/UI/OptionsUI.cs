using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsUI : MonoBehaviour
{
    [Header ("Sounds")]
    [SerializeField] private AudioClip buttonSound;

    [SerializeField] private Animator sfxButtonAnimator;
    [SerializeField] private Animator musicButtonAnimator;

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
        (bool sfx, bool music) var = AudioManager.Instance.GetSoundOptions();
        SetUpSound(var.sfx, var.music);
        gameObject.SetActive(true);
    }

    private void SetUpSound(bool sfx, bool music)
    {
        SetSFXButton(sfx);
        SetMusicButton(music);
    }

    public void Reset()
    {
        
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
    public void GoBack()
    {
        PlayButtonSound();
        HideUI();
    }
}

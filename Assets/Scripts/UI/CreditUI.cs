using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditUI : MonoBehaviour
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

    public void Reset()
    {
        
    }

    public void GoBack()
    {
        PlayButtonSound();
        HideUI();
    }
}

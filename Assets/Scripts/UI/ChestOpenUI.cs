using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestOpenUI : MonoBehaviour
{

    [Header ("Sounds")]
    [SerializeField] private AudioClip buttonSound;

    [SerializeField] private ChestOpen openChest;
    [SerializeField] private Button goBackButton;

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
        goBackButton.interactable = false;
        (Sprite sprite, Rarity rarity, bool isUnlocked) = CharachterManager.Instance.RoleNewCharachter();
        if(sprite != null)
        {
            openChest.SetupInitialState();
            openChest.SetUpChest(rarity, sprite, isUnlocked);
        }
        else
        {
            return;
        }
        gameObject.SetActive(true);
        openChest.DropChest();
        goBackButton.interactable = true;
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

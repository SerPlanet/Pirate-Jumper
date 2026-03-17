using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingStartUI : MonoBehaviour
{
    [Header ("Sounds")]
    [SerializeField] private TextMeshProUGUI LoadingTxt;
    [SerializeField] private TextMeshProUGUI PressToContinueTxt;
    [SerializeField] private Button blockerImage;

    [SerializeField] private Slider loadingSlider;

    private void Start()
    {
        LoadingTxt.enabled = true;
        PressToContinueTxt.enabled = false;
        blockerImage.interactable = false;
        Reset();
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
         loadingSlider.value = 0;
    }


    public void SetLoadingSlider(float i)
    {
        loadingSlider.value = i;
    }
    public void CanContinue()
    {
        LoadingTxt.enabled = false;
         blockerImage.interactable = true;
        PressToContinueTxt.enabled = true;
    }

    public void StartGame()
    {
        HideUI();
        GameManager.Instance.GameLobby();
    }
}

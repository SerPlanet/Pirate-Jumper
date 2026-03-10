using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CharachterSelectUI : MonoBehaviour
{
    [SerializeField] private GameObject pageObject;
    [SerializeField] private Transform charachterPagesObj;

    [SerializeField] private ScrollSnap scrollSnapView;

    private List<Image> charachterImage = new List<Image>();

    #region SetUpUI

    public void CreateAllCharachters()
    {
        List<ScriptableCharachter> allCharachter = CharachterManager.Instance.GetAllCharachter();
        scrollSnapView.SetMaxPages(allCharachter.Count);
        foreach(ScriptableCharachter charachter in allCharachter)
        {
            GameObject currPage = Instantiate(pageObject, charachterPagesObj);
            Image currImage = currPage.GetComponentInChildren<Image>();
            charachterImage.Add(currImage);
            currImage.sprite = charachter.icon;
            if (CharachterManager.Instance.IsUnlocked(charachter.charachterName))
            {
                currImage.color = new Color(1,1,1);
            }
            else
            {
                currImage.color = new Color(0,0,0);
            }
        }
    }

    public void UpdateCharachterUnlock(int i)
    {
        charachterImage[i].color = new Color(1,1,1);
    }

    #endregion

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
        MoveToCurrentSelectedCharachter();
    }

    public void SelectCurrentCharachter()
    {
        PlayButtonSound();
        CharachterManager.Instance.SetCurrentCharachterByNr(scrollSnapView.GetCurrenPageNr()-1);
        GoBack();
    }

    public void MoveToCurrentSelectedCharachter()
    {
        int i = CharachterManager.Instance.GetPosInAllListOfCurrSelected();
        scrollSnapView.SetCurrentPage(i+1);
    }

    public void GoBack()
    {
        PlayButtonSound();
        GameManager.Instance.GameLobby();
    }
}

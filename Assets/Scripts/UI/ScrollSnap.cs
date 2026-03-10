using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollSnap : MonoBehaviour, IEndDragHandler
{
     [Header ("SideScroll")]
    [SerializeField] private int maxPage;
    [SerializeField] private Vector3 pageStep;
    [SerializeField] private RectTransform charachterPagesRect;
    [SerializeField] private float tweenTime;
    [SerializeField] private Ease tweenType;

    [SerializeField] private Button nextButton, prevButton, selectButton;

    private int currentPage;
    private Vector3 targetPos;

    private float dragThreshould;


    private void Awake()
    {
        currentPage = 1;
        targetPos = charachterPagesRect.anchoredPosition;
        dragThreshould = Screen.width /15;
        UpdateArrowButton();
    }
    #region SideScroll

    public void Next()
    {
        if(currentPage < maxPage)
        {
            currentPage++;
            targetPos += pageStep;
            MovePage();
        }
    }

    public void Previous()
    {
        if(currentPage > 1)
        {
            currentPage--;
            targetPos -= pageStep;
            MovePage();
        }
    }

    private void MovePage()
    {
        charachterPagesRect.DOAnchorPos(targetPos, tweenTime)
                   .SetEase(tweenType);
        UpdateArrowButton();
        UpdateSelectButton();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(Mathf.Abs(eventData.position.x - eventData.pressPosition.x) > dragThreshould)
        {
            if (eventData.position.x > eventData.pressPosition.x)
            {
                Debug.Log("Prevous");
                Previous();
            }
            else
            {
                Debug.Log("Next");
                Next();
            }
        }
        else
        {
              Debug.Log("PageSwap");
            
        }
        MovePage();
    }

    private void UpdateArrowButton()
    {
        nextButton.interactable = true;
        prevButton.interactable = true;
        if(currentPage == 1) prevButton.interactable = false;
        else if(currentPage == maxPage) nextButton.interactable = false;
    }

    private void UpdateSelectButton()
    {
        if (CharachterManager.Instance.IsUnlocked(CharachterManager.Instance.GetCharachterByNr(currentPage - 1).charachterName))
        {
            selectButton.interactable = true;
        }
        else
        {
             selectButton.interactable = false;
        }
    }

    #endregion

    #region Getter/Setter
    public void SetMaxPages(int i)
    {
        maxPage = i;
    }

    public void SetCurrentPage(int i)
    {
            // Clamp zwischen 1 und maxPage
        i = Mathf.Clamp(i, 1, maxPage);

        // Berechne die Differenz zur aktuellen Page
        int diff = i - currentPage;
        if(diff == 0) return; // nichts zu tun
        if(i>maxPage) return;

        // Setze die neue Page
        currentPage = i;

        // Berechne neue Zielposition
        targetPos = (Vector3)charachterPagesRect.anchoredPosition + pageStep * diff;

        // Tween einmal zur neuen Position
        MovePage();
    }
    
    public int GetCurrenPageNr(){return currentPage;}
    #endregion
}

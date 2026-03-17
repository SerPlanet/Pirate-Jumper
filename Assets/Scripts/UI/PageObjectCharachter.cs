using System;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UI;

public class PageObjectCharachter : MonoBehaviour
{
    [SerializeField] private Image glowImg;
    [SerializeField] private Image charachterImg;

    [SerializeField] private TextMeshProUGUI nameTxt;

    private String nameSave;

    public void SetUpPageObject(Sprite charachter, String charName, Rarity rarity)
    {
        nameSave = charName;
        charachterImg.sprite = charachter;
        switch(rarity){
            case(Rarity.common):
            glowImg.color = Color.grey;
            break;
            case(Rarity.rare):
            glowImg.color = Color.blue;
            break;
            case(Rarity.epic):
            glowImg.color = Color.magenta;;
            break;
            case(Rarity.legendary):
            glowImg.color = new Color (1,0.7529413f,0.3019608f);
            break;
        }
    }

    public void IsUnlocked(bool IsUnlocked)
    {
        if (IsUnlocked)
        {
            charachterImg.color = Color.white;
            nameTxt.text = nameSave;
        }
        else
        {
             charachterImg.color = Color.black;
            nameTxt.text = "?";
        }
    }
}

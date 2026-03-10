using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharachterManager : MonoBehaviour
{
    public static CharachterManager Instance;

    [SerializeField] private List<ScriptableCharachter> allCharachters; 

    [SerializeField] private List<ScriptableCharachter> commonCharachters;
    [SerializeField] private List<ScriptableCharachter> rareCharachters;
    [SerializeField] private List<ScriptableCharachter> epicCharachters;
    [SerializeField] private List<ScriptableCharachter> legendaryCharachters;

    [SerializeField] private ulong amountPerChest;

    private HashSet<CharachterName> unlockedCharacters = new HashSet<CharachterName>();

    public bool resetCharachters;

    private ScriptableCharachter currentCharachterPlayed;
     private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (resetCharachters)
        {
            SaveManager.Instance.ResetCurrentCharacter();
            SaveManager.Instance.ResetUnlockedCharacters();
        }
        LoadUnlockedCharacters();
        LoadCurrentCharacter();
        CreateListForRarity();
        UIManager.Instance.SetUpCharachterUI();
        SpawnManager.Instance.CharachterManagerRdy();
    }

    #region SelectSystem
    private void LoadCurrentCharacter()
    {
        CharachterName savedName = SaveManager.Instance.LoadCurrentCharacter();
        currentCharachterPlayed = GetCharacter(savedName);
        if(currentCharachterPlayed == null)
        {
            currentCharachterPlayed = allCharachters[0];
        }
        Debug.Log(savedName);
        GameManager.Instance.GetPlayerScript().ChangeCharachter(currentCharachterPlayed.animatorController);
    }

    public void SetCurrentCharacter(CharachterName name)
    {
        Debug.Log(name);
        if (!IsUnlocked(name))
            return;

        Debug.Log("IsUnlocked");
        currentCharachterPlayed = GetCharacter(name);
        GameManager.Instance.GetPlayerScript().ChangeCharachter(currentCharachterPlayed.animatorController);
        SaveManager.Instance.SaveCurrentCharacter(name);
    }

    public void SetCurrentCharachterByNr(int i)
    {
        SetCurrentCharacter(GetCharachterByNr(i).charachterName);
    }

    public ScriptableCharachter GetCurrentCharacter()
    {
        return currentCharachterPlayed;
    }

    #endregion

    #region Unlock System

    public (Sprite,Rarity) RoleNewCharachter()
    {
        if (GameManager.Instance.OpenChestWithAmount(amountPerChest))
        {
            int randomNr = Random.Range(0,100);

            if (randomNr < 70)
            {
                 randomNr = Random.Range(0,commonCharachters.Count);
                UnlockCharacter(commonCharachters[randomNr].charachterName);
                Debug.Log("common" + commonCharachters[randomNr].charachterName);
                return (commonCharachters[randomNr].icon, Rarity.common); 
            }
            else if (randomNr < 90)
            {
                randomNr = Random.Range(0,rareCharachters.Count);
                UnlockCharacter(rareCharachters[randomNr].charachterName);
                Debug.Log("rare");
                 return (rareCharachters[randomNr].icon, Rarity.rare);
            }
            
            else if (randomNr < 99)
            {
                randomNr = Random.Range(0,epicCharachters.Count);
                UnlockCharacter(epicCharachters[randomNr].charachterName);
                Debug.Log("Epic");
                return (epicCharachters[randomNr].icon, Rarity.epic);
            }
            else
            {
               randomNr = Random.Range(0,legendaryCharachters.Count);
                UnlockCharacter(legendaryCharachters[randomNr].charachterName);
                Debug.Log("Legendary");
                return (legendaryCharachters[randomNr].icon, Rarity.legendary);
            }
        }
        else
        {
            return (null,Rarity.common);
        }
        
    }

    public void UnlockCharacter(CharachterName name)
    {
        if (unlockedCharacters.Contains(name))
            return;

        unlockedCharacters.Add(name);
        UIManager.Instance.SetNewUnlockedCharachter(GetPosOfCharachter(GetScriptableCharachterByName(name)));
        SaveManager.Instance.SaveUnlockedCharacters(new List<CharachterName>(unlockedCharacters));
    }

    public bool IsUnlocked(CharachterName name)
    {
        return unlockedCharacters.Contains(name);
    }

    #endregion

    #region Access Character

    public ScriptableCharachter GetCharacter(CharachterName name)
    {
        return allCharachters.Find(c => c.charachterName == name);
    }

    #endregion

     #region Save / Load

    private void LoadUnlockedCharacters()
    {
        List<CharachterName> loaded = SaveManager.Instance.LoadUnlockedCharacters();
        unlockedCharacters = new HashSet<CharachterName>(loaded);

        // Falls nichts gespeichert → ersten Character freischalten
        if (unlockedCharacters.Count == 0)
        {
            unlockedCharacters.Add(allCharachters[0].charachterName);
        }
    }

    private void CreateListForRarity()
    {
        var sortedList = allCharachters.OrderBy(c => c.rarity).ToList();
        allCharachters = sortedList;
        foreach(ScriptableCharachter charachter in allCharachters)
        {
            switch (charachter.rarity)
            {
                case(Rarity.common):
                commonCharachters.Add(charachter);
                break;
                case(Rarity.rare):
                rareCharachters.Add(charachter);
                break;
                case(Rarity.epic):
                epicCharachters.Add(charachter);
                break;
                case(Rarity.legendary):
                legendaryCharachters.Add(charachter);
                break;
            }
        }
    }

    #endregion
    #region Getter
    public List<ScriptableCharachter> GetAllCharachter()
    {
        return allCharachters;
    }

    public int GetPosInAllListOfCurrSelected()
    {
        return allCharachters.IndexOf(currentCharachterPlayed);
    }

    public int GetPosOfCharachter(ScriptableCharachter charachter)
    {
        return allCharachters.IndexOf(charachter);
    }

    private ScriptableCharachter GetScriptableCharachterByName(CharachterName name)
    {
        foreach(ScriptableCharachter charachter in allCharachters)
        {
            if(charachter.charachterName == name)
            {
                return charachter;
            }
        }
        return null;
    }

    public ScriptableCharachter GetCharachterByNr(int i)
    {
        return allCharachters[i];
    }

    #endregion

}

public enum CharachterName
{
    Pirate,
    Englisch,
    Astronaut,
    Animal,
    Women,
    GameDevCharachter
}
public enum Rarity
{
    common,
    rare,
    epic,
    legendary
}

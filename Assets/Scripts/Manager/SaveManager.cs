using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private const string HighscoreKey = "Highscore";
    private const string MoneyKey = "Money";
    private const string UnlockedCharactersKey = "UnlockedCharacters";
    private const string CurrentCharacterKey = "CurrentCharacter";

    private const string MusicSoundKey = "MusicSound";
    private const string SFXSoundKey = "SFXSound";
    private const string UISoundKey = "UISound";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    #region Highscore

    public void SaveHighscore(ulong highscore)
    {
        PlayerPrefs.SetString(HighscoreKey, highscore.ToString());
        PlayerPrefs.Save();
    }

    public ulong LoadHighscore()
    {
        if (!PlayerPrefs.HasKey(HighscoreKey))
            return 0;

        return ulong.Parse(PlayerPrefs.GetString(HighscoreKey));
    }

    public void ResetHighscore()
    {
        PlayerPrefs.DeleteKey("Highscore");
        PlayerPrefs.Save();
    }

    #endregion

    #region Money

    public void SaveMoney(ulong money)
    {
        PlayerPrefs.SetString(MoneyKey, money.ToString());
        PlayerPrefs.Save();
    }

    public ulong LoadMoney()
    {
        if (!PlayerPrefs.HasKey(MoneyKey))
            return 0;

        return ulong.Parse(PlayerPrefs.GetString(MoneyKey));
    }

    public void ResetMoney()
    {
        PlayerPrefs.DeleteKey("Money");
        PlayerPrefs.Save();
    }

    #endregion

    #region Charachter
    public void SaveUnlockedCharacters(List<CharachterName> characters)
    {
        List<int> values = new List<int>();

        foreach (var c in characters)
            values.Add((int)c);

        string data = string.Join(",", values);
        PlayerPrefs.SetString("UnlockedCharacters", data);
        PlayerPrefs.Save();
    }

    public List<CharachterName> LoadUnlockedCharacters()
    {
        List<CharachterName> result = new List<CharachterName>();

        if (!PlayerPrefs.HasKey("UnlockedCharacters"))
            return result;

        string data = PlayerPrefs.GetString("UnlockedCharacters");
        string[] split = data.Split(',');

        foreach (string s in split)
        {
            if (int.TryParse(s, out int value))
                result.Add((CharachterName)value);
        }

        return result;
    }

    public void SaveCurrentCharacter(CharachterName name)
    {
        PlayerPrefs.SetInt(CurrentCharacterKey, (int)name);
        PlayerPrefs.Save();
    }

    public CharachterName LoadCurrentCharacter()
    {
        if (!PlayerPrefs.HasKey(CurrentCharacterKey))
            return CharachterName.Hook; // Default

        int value = PlayerPrefs.GetInt(CurrentCharacterKey);
        return (CharachterName)value;
    }

    public void ResetCurrentCharacter()
    {
        PlayerPrefs.DeleteKey(CurrentCharacterKey);
        PlayerPrefs.Save();
    }

    public void ResetUnlockedCharacters()
    {
        PlayerPrefs.DeleteKey("UnlockedCharacters");
        PlayerPrefs.Save();
    }
    #endregion

    #region Sound
    public void SaveMusicSound(bool value)
    {
        PlayerPrefs.SetInt(MusicSoundKey, value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SaveSFXSound(bool value)
    {
        PlayerPrefs.SetInt(SFXSoundKey, value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SaveUISound(bool value)
    {
        PlayerPrefs.SetInt(UISoundKey, value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public bool LoadMusicSound()
    {
        if (!PlayerPrefs.HasKey(MusicSoundKey))
            return true; // Default an

        return PlayerPrefs.GetInt(MusicSoundKey) == 1;
    }

    public bool LoadSFXSound()
    {
        if (!PlayerPrefs.HasKey(SFXSoundKey))
            return true;

        return PlayerPrefs.GetInt(SFXSoundKey) == 1;
    }

    public bool LoadUISound()
    {
        if (!PlayerPrefs.HasKey(UISoundKey))
            return true;

        return PlayerPrefs.GetInt(UISoundKey) == 1;
    }
    #endregion
    #region ResetAll
    public void ResetAllData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
    #endregion
}

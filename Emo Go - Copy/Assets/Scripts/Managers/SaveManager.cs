using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    public SaveState state;
    public PlayerSettings settings;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        
        

        Load();

    }

    public void SaveGame()
    {
        PlayerPrefs.SetString("save", SerializerScript.Serialize<SaveState>(state));
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetString("settings", SerializerScript.Serialize<PlayerSettings>(settings));
    }

    public void Load()
    {
        if(PlayerPrefs.HasKey("save"))
        {
            state = SerializerScript.Deserialize<SaveState>(PlayerPrefs.GetString("save"));
        }
        else
        {
            state = new SaveState();
            SaveGame();
            Debug.Log("No Save Found, Created New One");
        }

        if (PlayerPrefs.HasKey("settings"))
        {
            settings = SerializerScript.Deserialize<PlayerSettings>(PlayerPrefs.GetString("settings"));
        }
        else
        {
            settings = new PlayerSettings();
            SaveSettings();
            Debug.Log("No Settings Found, Created New One");
        }
    }

    public void ResetSave()
    {
        PlayerPrefs.DeleteKey("save");
        Load();
        SaveGame();
    }

    public bool IsColorOwned(int index)
    {
        return (state.colorOwned & (1 << index)) != 0;
    }

    public bool IsEmojiOwned(int index)
    {
        return (state.emojiOwned & (1 << index)) != 0;
    }

    public void UnlockColor(int index)
    {
        state.colorOwned |= 1 << index;
    }

    public void UnlockEmoji(int index)
    {
        state.emojiOwned |= 1 << index;
    }

    public bool BuyColor(int index, int cost)
    {
        if (state.coins >= cost)
        {
            state.coins -= cost;
            UnlockColor(index);
            SaveGame();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool BuyEmoji(int index, int cost)
    {
        if (state.coins >= cost)
        {
            state.coins -= cost;
            UnlockEmoji(index);
            SaveGame();
            return true;
        }
        else
        {
            return false;
        }
    }
}

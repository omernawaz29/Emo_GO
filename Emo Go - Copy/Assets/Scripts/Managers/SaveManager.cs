using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    public SaveState state;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;
        Load();
    }

    public void Save()
    {
        PlayerPrefs.SetString("save", SerializerScript.Serialize<SaveState>(state));
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
            Save();
            Debug.Log("No Save Found, Created New One");
        }
    }

    public void ResetSave()
    {
        PlayerPrefs.DeleteKey("save");
        Load();
        Save();
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
            Save();
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
            Save();
            return true;
        }
        else
        {
            return false;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class SceneMenu : MonoBehaviour
{
    CanvasGroup fade;
    float fadeSpeed = 0.3f;


    [SerializeField] RectTransform menuContainer;
    [SerializeField] Transform levelPanel;
    [SerializeField] Transform colorPanel;
    [SerializeField] Transform emojiPanel;


    [SerializeField] Material playerMat;

    [SerializeField] Color currentLevelColor = Color.blue;
    [SerializeField] Color completedLevelColor = Color.green;


    [SerializeField] TextMeshProUGUI colorBuySetText;
    [SerializeField] TextMeshProUGUI emojiBuySetText;
    [SerializeField] TextMeshProUGUI coinsText;

    [SerializeField] GameObject showCaseEmoji;

    private int[] colorCost = new int[] { 0, 500, 500, 500, 1000, 1000, 1000, 1500, 1500 };
    private int[] emojiCost = new int[] { 0, 1000, 1000, 1000, 1500, 1500, 1500, 2000, 2000 };

    private Color[] colors = new Color[] { Color.yellow, Color.green, Color.blue, Color.red, Color.cyan, Color.magenta, Color.gray, Color.black, Color.white };

    private int selectedColorIndex;
    private int selectedEmojiIndex;
    private int activeColorIndex;
    private int activeEmojiIndex;


    [SerializeField] GameObject[] Faces;


    Vector3 desiredMenuPos;
    int currentMenu;
    void Start()
    {

        UpdateCoinText();
        UpdateFaces();

        fade = FindObjectOfType<CanvasGroup>();
        fade.alpha = 1.0f;

        SaveManager.instance.state.levelsCompleted = 18;

        SetCameraTo(GameManagerScript.instance.menuFocus);
        AudioManager.instance.StopAll();
        AudioManager.instance.Play("menu");


        InitLevel();
        InitShop();

        GameManagerScript.instance.currentLevel = SaveManager.instance.state.levelsCompleted;
    }

    void Update()
    {
        fade.alpha = 1 - Time.timeSinceLevelLoad * fadeSpeed;
        menuContainer.anchoredPosition3D = Vector3.Lerp(menuContainer.anchoredPosition3D, desiredMenuPos, 0.1f);
    }

    private void SetCameraTo(int menuIndex)
    {
        Navigate(menuIndex);
        menuContainer.anchoredPosition3D = desiredMenuPos;
    }
    private void Navigate(int menuIndex)
    {
        switch(menuIndex)
        {
            default:
            case 0:
                desiredMenuPos = Vector3.zero;
                break;
            case 1:
                desiredMenuPos = Vector3.right * -720;
                break;
            case 2:
                desiredMenuPos = Vector3.right * 720;
                break;
        }
    }



    void InitLevel()
    {
        if (levelPanel == null)
            Debug.Log("Level Panel not assigned");

        int i = 0;
        foreach (Transform t in levelPanel)
        {
            int currentIndex = i;
            Button b = t.GetComponent<Button>();

            TextMeshProUGUI text = b.GetComponentInChildren<TextMeshProUGUI>();
            text.text = (currentIndex + 1).ToString();

            b.onClick.AddListener(() => OnLevelSelect(currentIndex));

            Image img = t.GetComponent<Image>();

            if (i <= SaveManager.instance.state.levelsCompleted)
            {
                if (i == SaveManager.instance.state.levelsCompleted)
                {
                    img.color = currentLevelColor;
                }
                else
                    img.color = completedLevelColor;
            }
            else
            {
                b.interactable = false;
                img.color = Color.gray;
            }

            i++;

        }
    }
    public void ResetSave()
    {
        //SaveManager.instance.ResetSave();
        //GameManagerScript.instance.menuFocus = 1;
        //SceneManager.LoadScene("Menu");
    }
    void InitShop()
    {
        if (colorPanel == null || emojiPanel == null)
            Debug.Log("Did not assign color/emoji panel in the inspector");

        int i = 0;
        foreach(Transform t in colorPanel)
        {
            int current = i;

            Image img = t.GetComponent<Image>();
            img.color = colors[i];

            Button b = t.GetComponent<Button>();

            b.onClick.AddListener(() => OnColorSelect(current));
            i++;
        }

        i = 0;

        foreach (Transform t in emojiPanel)
        {
            int current = i;
            Button b = t.GetComponent<Button>();
            b.onClick.AddListener(() => OnEmojiSelect(current));
            i++;
        }
    }
    

    void SetColor(int index)
    {

        activeColorIndex = index;
        playerMat.color = colors[index];
        colorBuySetText.text = "Current";
    }

    void SetEmoji(int index)
    {
        activeEmojiIndex = index;
        SaveManager.instance.state.activeEmo = index;
        emojiBuySetText.text = "Current";
    }


    //Buttons
    private void OnColorSelect(int current)
    {
        selectedColorIndex = current;

        if (SaveManager.instance.IsColorOwned(current))
        {
            colorBuySetText.text = "Select";
        }
        else
        {
            colorBuySetText.text = "$" + colorCost[current].ToString();
        }
    }

    private void OnEmojiSelect(int current)
    {
        selectedEmojiIndex = current;

        if (SaveManager.instance.IsEmojiOwned(current))
        {
            emojiBuySetText.text = "Select";
        }
        else
        {
            emojiBuySetText.text = "$" + emojiCost[current].ToString();
        }
    }

    void OnLevelSelect(int currentIndex)
    {
       // if (SaveManager.instance.state.levelsCompleted >= 10)
         //   ResetSave();

        GameManagerScript.instance.currentLevel = currentIndex;
        SceneManager.LoadScene(currentIndex + 2);

        Debug.Log("Select Level: " + currentIndex);
    }


    public void OnPlayClick()
    {
        var newSceneIndex = GameManagerScript.instance.currentLevel + 2;

        if (newSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Navigate(2);
        }
        else
            SceneManager.LoadScene(newSceneIndex);
    }

    public void OnLevelClick()
    {
        Navigate(2);
    }

    public void OnShopClick()
    {
        showCaseEmoji.SetActive(true);
        Navigate(1);
    }

    public void OnBackClick()
    {
        showCaseEmoji.SetActive(false);
        Navigate(0);
    }

    public void OnColorBuySet()
    {
        if(SaveManager.instance.IsColorOwned(selectedColorIndex))
        {
            SetColor(selectedColorIndex);
        }
        else
        {
            if(SaveManager.instance.BuyColor(selectedColorIndex, colorCost[selectedColorIndex]))
            {
                SetColor(selectedColorIndex);
                UpdateCoinText();
            }
            else
            {
                Debug.Log("No Money");
            }
        }
    }

    public void OnEmojiBuySet()
    {
        if (SaveManager.instance.IsEmojiOwned(selectedEmojiIndex))
        {
            SetEmoji(selectedEmojiIndex);
            UpdateFaces();

        }
        else
        {
            if (SaveManager.instance.BuyEmoji(selectedEmojiIndex, emojiCost[selectedEmojiIndex]))
            {
                SetEmoji(selectedEmojiIndex);
                UpdateFaces();
                UpdateCoinText();
            }
            else
            {
                Debug.Log("No Money");
            }
        }
    }

    private void UpdateCoinText()
    {
        coinsText.text = "$" + SaveManager.instance.state.coins.ToString();
    }

    void UpdateFaces()
    {
        foreach (GameObject t in Faces)
        {
            t.SetActive(false);
        }
        Faces[SaveManager.instance.state.activeEmo].SetActive(true);
    }
}

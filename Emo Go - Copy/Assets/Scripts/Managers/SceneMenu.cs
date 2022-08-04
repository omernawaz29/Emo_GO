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

    [Space]
    [SerializeField] GameObject buyButton;
    [SerializeField] TextMeshProUGUI buySetText;

    [Space]
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

    private Outline previousOutline;

    private AudioManager _audioManager;

    void Start()
    {
        buyButton.SetActive(false);
        _audioManager = FindObjectOfType<AudioManager>();

        UpdateCoinText();
        UpdateFaces();
        SetColor(SaveManager.instance.state.activeColor);

        fade = FindObjectOfType<CanvasGroup>();
        fade.alpha = 1.0f;

        SetCameraTo(GameManagerScript.instance.menuFocus);
        AudioManager.instance.StopAll();
        AudioManager.instance.Play("menu");


        InitLevel();
        InitShop();
        GameManagerScript.instance.currentLevel = SaveManager.instance.state.levelsCompleted + 1;
        
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
            // getting the lock image in UI
            GameObject lockImgObj = t.GetChild(1).GetComponent<Image>().gameObject;

            if (i <= SaveManager.instance.state.levelsCompleted)
            {
                lockImgObj.SetActive(false);

                if (i == SaveManager.instance.state.levelsCompleted)
                {
                    img.color = currentLevelColor;
                }
                else
                    img.color = completedLevelColor;
            }
            else
            {
                GameObject tLevelText = t.GetChild(0).GetComponent<TextMeshProUGUI>().gameObject;
                tLevelText.SetActive(false);

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

            // getting the lock image in UI
            Image lockImg = t.GetChild(0).GetComponent<Image>();
            if (SaveManager.instance.IsColorOwned(i))
            {
                // if (SaveManager.instance.state.activeEmo == i)
                //     t.GetComponent<Outline>().enabled = true;

                Color c = lockImg.color;
                c.a = 0;
                lockImg.color = c;
            }

            i++;
        }

        i = 0;

        foreach (Transform t in emojiPanel)
        {
            int current = i;
            Button b = t.GetComponent<Button>();
            b.onClick.AddListener(() => OnEmojiSelect(current));

            // getting the lock image in UI
            Image lockImg = t.GetChild(0).GetComponent<Image>();
            if (SaveManager.instance.IsEmojiOwned(i))
            {
                Color c = lockImg.color;
                c.a = 0;
                lockImg.color = c;
            }

            i++;
        }
    }
    

    void SetColor(int index)
    {
        SaveManager.instance.state.activeColor = index;
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
            if (previousOutline != null)
                previousOutline.enabled = false;
            previousOutline = null;
            buyButton.SetActive(false);

            SetColor(selectedColorIndex);
            colorBuySetText.text = "Select";
        }
        else
        {
            int i = 0;
            foreach (Transform t in colorPanel)
            {
                if (i == selectedColorIndex)
                {
                    if (previousOutline != null)
                        previousOutline.enabled = false;
                    previousOutline = t.GetComponent<Outline>();
                    previousOutline.enabled = true;
                    break;
                }

                i++;
            }
            buyButton.SetActive(true);
            buySetText.text = colorCost[current].ToString();

            colorBuySetText.text = colorCost[current].ToString();
        }
    }

    private void OnEmojiSelect(int current)
    {
        selectedEmojiIndex = current;

        if (SaveManager.instance.IsEmojiOwned(current))
        {
            if (previousOutline != null)
                previousOutline.enabled = false;
            previousOutline = null;
            buyButton.SetActive(false);

            SetEmoji(selectedEmojiIndex);
            UpdateFaces();

            emojiBuySetText.text = "Select";
        }
        else
        {
            int i = 0;
            foreach (Transform t in emojiPanel)
            {
                if (i == selectedEmojiIndex)
                {
                    if (previousOutline != null)
                        previousOutline.enabled = false;
                    previousOutline = t.GetComponent<Outline>();
                    previousOutline.enabled = true;
                    break;
                }

                i++;
            }

            buyButton.SetActive(true);
            buySetText.text = emojiCost[current].ToString();

            emojiBuySetText.text = emojiCost[current].ToString();
        }
    }

    void OnLevelSelect(int currentIndex)
    {
       // if (SaveManager.instance.state.levelsCompleted >= 10)
         //   ResetSave();

        GameManagerScript.instance.currentLevel = currentIndex + 1;
        SceneManager.LoadScene(currentIndex + 2);

        Debug.Log("Select Level: " + currentIndex + 1);
    }


    public void OnPlayClick()
    {
        var newSceneIndex = GameManagerScript.instance.currentLevel + 1;
        Debug.Log("Level: " + newSceneIndex);
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
        coinsText.text = SaveManager.instance.state.coins.ToString();
    }

    void UpdateFaces()
    {
        foreach (GameObject t in Faces)
        {
            t.SetActive(false);
        }
        Faces[SaveManager.instance.state.activeEmo].SetActive(true);
    }

    public void OnBuyButtonClick()
    {
        if (previousOutline.tag == "ColorShop")
        {
            // Debug.Log("BUYING color");
            if (SaveManager.instance.BuyColor(selectedColorIndex, colorCost[selectedColorIndex]))
            {
                _audioManager.Play("ChaChing");
                previousOutline.gameObject.GetComponentInChildren<Animator>().enabled = true;

                if (previousOutline != null)
                    previousOutline.enabled = false;
                previousOutline = null;
                buyButton.SetActive(false);

                SetColor(selectedColorIndex);
                UpdateCoinText();
            }
            else
            {
                Debug.Log("No Money");
            }
        }
        else if (previousOutline.tag == "EmojiShop")
        {
            // Debug.Log("BUYING EMO");
            if (SaveManager.instance.BuyEmoji(selectedEmojiIndex, emojiCost[selectedEmojiIndex]))
            {
                _audioManager.Play("ChaChing");
                previousOutline.gameObject.GetComponentInChildren<Animator>().enabled = true;

                if (previousOutline != null)
                    previousOutline.enabled = false;
                previousOutline = null;
                buyButton.SetActive(false);

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

    public void GotoDebuggerScene()
    {
        SceneManager.LoadScene("DebuggerImplemented");
    }
}

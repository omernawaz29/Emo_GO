using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebuggerMenuScript : MonoBehaviour
{

    [SerializeField] Transform slidersPanel;
    [SerializeField] Transform slidersTitlePanel;
    [SerializeField] GameObject DebugContainer;
    TextMeshProUGUI[] sliderTitles = new TextMeshProUGUI[10];
    [SerializeField] PhysicMaterial playerPhysicsMaterial;
    Slider[] sliders = new Slider[10];
    FloatBox[] variables = new FloatBox[8];


    int buttonPresses = 0;

    private void Start()
    {
        InitVariables();
        InitSliders();

        buttonPresses = 0;

    }

    void InitVariables()
    {
        variables[0] = SaveManager.instance.settings.levelRotationSpeed;
        variables[1] = SaveManager.instance.settings.levelMaxRotationAngle;
        variables[2] = SaveManager.instance.settings.levelMovementMultiplier;

        variables[3] = SaveManager.instance.settings.playerMaxSpeed;
        variables[4] = SaveManager.instance.settings.playerMaxAcceleration;

        variables[5] = SaveManager.instance.settings.playerBounciness;
        variables[6] = SaveManager.instance.settings.playerDynamicFriction;
        variables[7] = SaveManager.instance.settings.playerStaticFriction;
    }
    void InitSliders()
    {
        if (slidersPanel == null)
            Debug.LogWarning("Did not assign sliders panel in the inspector");
        if (slidersTitlePanel == null)
            Debug.LogWarning("Did not assign slider titles panel in the inspector");

        int i = 0;
        foreach(Transform t in slidersPanel)
        {
            int current = i;

            Slider slider = t.GetComponent<Slider>();

            if (current < variables.Length)
                slider.value = variables[current].Value;

            slider.onValueChanged.AddListener(delegate { SliderChanged(current); });

            sliders[current] = slider;
            

            i++;
        }

        i = 0;

        foreach(Transform t in slidersTitlePanel)
        {
            int current = i;

            TextMeshProUGUI text = t.GetComponent<TextMeshProUGUI>();

            if (current < variables.Length)
                text.text = variables[current].Name + ": " + variables[current].Value;
            else
                text.text = "not assigned";


            sliderTitles[current] = text;
            i++;
        }
    }

    private void Update()
    {
        if (!playerPhysicsMaterial)
            return;

        if (playerPhysicsMaterial.bounciness != variables[5].Value)
        {
            playerPhysicsMaterial.bounciness = variables[5].Value;
        }
        if (playerPhysicsMaterial.dynamicFriction != variables[6].Value)
        {
            playerPhysicsMaterial.dynamicFriction = variables[6].Value;
        }
        if (playerPhysicsMaterial.staticFriction != variables[7].Value)
        {
            playerPhysicsMaterial.staticFriction = variables[7].Value;
        }
    }


    void SliderChanged(int sliderIndex)
    {
        //Debug.Log(sliders[sliderIndex].value);

        if (sliderIndex < variables.Length)
        {
            variables[sliderIndex].Value = sliders[sliderIndex].value;
            sliderTitles[sliderIndex].text = variables[sliderIndex].Name + ": " + variables[sliderIndex].Value.ToString();
        }
    }
    public void ButtonPress()
    {
        buttonPresses++;
        Debug.Log("Button Pressed");
        StartCoroutine(ResetButtonPress());
        if(buttonPresses >= 3)
        {
            DebugContainer.SetActive(true);
        }
    }
    public void CloseDebugger()
    {
        DebugContainer.SetActive(false);
    }


    IEnumerator ResetButtonPress()
    {
        yield return new WaitForSeconds(0.75f);
        Debug.Log("Button Reset");
        buttonPresses = 0;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebuggerMenuScript : MonoBehaviour
{

    [SerializeField] Transform slidersPanel;
    [SerializeField] Transform slidersTitlePanel;
    TextMeshProUGUI[] sliderTitles = new TextMeshProUGUI[10];
    Slider[] sliders = new Slider[10];
    FloatBox[] variables = new FloatBox[8];

    private void Start()
    {
        InitVariables();
        InitSliders();
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
            Debug.Log("Did not assign color/emoji panel in the inspector");

        int i = 0;
        foreach(Transform t in slidersPanel)
        {
            int current = i;

            Slider slider = t.GetComponent<Slider>();

            Debug.Log(slider.gameObject.name);

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


    void SliderChanged(int sliderIndex)
    {
        //Debug.Log(sliders[sliderIndex].value);

        if (sliderIndex < variables.Length)
        {
            variables[sliderIndex].Value = sliders[sliderIndex].value;
            sliderTitles[sliderIndex].text = variables[sliderIndex].Name + ": " + variables[sliderIndex].Value.ToString();
        }
    }

}

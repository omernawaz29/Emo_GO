using UnityEngine;
[System.Serializable]
public class FloatBox
{
    [SerializeField ] private float myValue;
    [HideInInspector] public string myName;

    public FloatBox()
    {
        myValue = 0;
        myName = "float";
    }

    public FloatBox(float v, string name = "float")
    {
        myValue = v;
        myName = name;
    }
    public FloatBox(FloatBox copy)
    {
        myValue = copy.myValue;
        myName = copy.myName;
    }

    public float Value { get { return myValue; } set { myValue = value; } }
    public string Name { get { return myName;} }


}

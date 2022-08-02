public class FloatBox
{
    private float myValue;
    public string myName;
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

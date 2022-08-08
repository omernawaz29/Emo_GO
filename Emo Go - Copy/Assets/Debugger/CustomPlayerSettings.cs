
public class CustomPlayerSettings
{

    public FloatBox levelRotationSpeed = new FloatBox(1f, "levelRotationSpeed");
    public FloatBox levelMaxRotationAngle = new FloatBox(20f, "levelMaxRotationAngle");
    public FloatBox levelMovementMultiplier = new FloatBox(4f, "levelMovementMultiplier");

    public FloatBox playerMaxSpeed = new FloatBox(10f, "playerMaxSpeed");
    public FloatBox playerMaxAcceleration = new FloatBox(30f, "playerMaxAcceleration");

    public FloatBox playerBounciness = new FloatBox(1f, "playerBounciness");
    public FloatBox playerStaticFriction = new FloatBox(0f, "playerStaticFriction");
    public FloatBox playerDynamicFriction = new FloatBox(0f, "playerDynamicFriction");

    public FloatBox wallsBounciness = new FloatBox(0.605f, "wallsBounciness");

}

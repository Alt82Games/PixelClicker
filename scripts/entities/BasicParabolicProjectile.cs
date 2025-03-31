using Godot;
using System;
using System.Threading;

public partial class BasicParabolicProjectile : Sprite2D
{
    float initialSpeed;
    float throwAngrleDegrees;
    float gravity = 9.8f;
    float time = 0;
    bool isLaunch = false;

    Vector2 initialPosition, throwDirection;

    public void launchProjectile(Vector2 initialPos, Vector2 direction, float desiredDistance, float desireAngle){
        initialPosition = initialPos;
        throwDirection =  direction.Normalized();
        throwAngrleDegrees = desireAngle;
        initialSpeed = (float)Math.Pow(desiredDistance *gravity / Math.Sin(2 * Mathf.DegToRad(desireAngle)),0.5f);
    
        isLaunch = true;
    
    }

}

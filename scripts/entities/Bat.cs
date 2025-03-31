using Godot;
using System;

public partial class Bat : UnitBase
{
    new float gravityScaleCustom = 0;
    
    public override void initializeThis(int level){
        this.level = level;
        speed = speed + speed*level;
        currentHealt = baseMaxHealt*level;
        healtBar.initializeHealthBar(baseMaxHealt*level);
        this.GravityScale = gravityScaleCustom;
        jumpImpulse = (jumpImpulse)*0.3f;
    }

    public override void jump(){
        Vector2 jumpImpulse = this.GlobalPosition.DirectionTo(objectivePosition)*100;
        if(alreadyJump){
            ApplyCentralImpulse(jumpImpulse*-1);
            alreadyJump = false;
            testTimer.WaitTime = 0.5f;
        }
        else{
            ApplyCentralImpulse(jumpImpulse);
            alreadyJump = true;
            testTimer.WaitTime = 1.0f;
        }
        
    }

}

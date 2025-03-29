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
    }

    public override void jump(){
        Vector2 jumpImpulse = this.Position.DirectionTo(objectivePosition)*100;
        if(alreadyJump){
            jumpImpulse = (jumpImpulse*-1)*0.6f;
            ApplyCentralImpulse(jumpImpulse);
            alreadyJump = false;
        }
        else{
            ApplyCentralImpulse(jumpImpulse);
            alreadyJump = true;
        }
        
    }

}

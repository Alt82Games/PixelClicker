using Godot;
using System;

public partial class Slime : UnitBase
{
    new float gravityScaleCustom = 0.4f;
    
    public override void initializeThis(int level){
        this.level = level;
        speed = speed + speed*level;
        currentHealt = baseMaxHealt*level;
        healtBar.initializeHealthBar(baseMaxHealt*level);
        this.GravityScale = gravityScaleCustom;
    }
    public override void move()
    {
    }


    public override void jump(){ 
        int y = GD.RandRange(120 ,250);
        jumpImpulse = new Vector2(this.GlobalPosition.DirectionTo(objectivePosition).X * 80,-y);
        ApplyCentralImpulse(jumpImpulse);
        
    }
}

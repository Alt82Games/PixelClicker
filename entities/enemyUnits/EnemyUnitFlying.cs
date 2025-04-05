using Godot;
using System;

public partial class EnemyUnitFlying : EnemyUnitBase
{
    new float gravityScaleCustom = 0;
    public override void move(float delta){
        

        if(!isDead){
            target = gameManager.EnemyObjective;
            directionToObjective = GlobalPosition.DirectionTo(target);
            if(this.GlobalPosition.DistanceTo(target) > 1){
                
                Velocity = (directionToObjective.Normalized()*speed);
            }
            else{
                Velocity = Vector2.Zero;
            }
        }
        else{
            this.GlobalPosition += initialVelocityDead*delta-gravityDead*delta;
		    initialVelocityDead += gravityDead;
            if(Modulate.A > 0.1f){
                Modulate -= new Color (0,0,0,1f*delta);
            }
            if(RotationDegrees < 355){
                RotationDegrees += 90*delta;
            }
            else{
                RotationDegrees = 0;
            }
            
        }
        
        MoveAndSlide();
    }
}

using Godot;
using System;

public partial class EnemyUnitFlying : EnemyUnitBase
{
    new float gravityScaleCustom = 0;
    public override void move(){
        target = gameManager.EnemyObjective;
        directionToObjective = GlobalPosition.DirectionTo(target);
        if(this.GlobalPosition.DistanceTo(target) > 1){
            
            Velocity = (directionToObjective.Normalized()*speed);
        }
        else{
            Velocity = Vector2.Zero;
        }
        
        MoveAndSlide();
    }
}

using Godot;
using System;

public partial class Golem00 : BasicTurret
{
    bool isJumping = false, isFalling = false, shootAlready = false;
    Vector2 jumpSpeed = new Vector2(0,1f);
    String projectileToShoot = "res://entities/basic_linear_projectile.tscn";

    public override void _PhysicsProcess(double delta)
    {
        if(isJumping){
            this.GlobalPosition -= jumpSpeed;
            if(this.GlobalPosition.Y < this.spawnPosition.Y-5){
                isJumping = false;
                isFalling = true;
            }
        }
        else if(isFalling){
            this.GlobalPosition += jumpSpeed;
            if(!shootAlready){
                createProjectile(projectileToShoot);
                shootAlready = true;
            }
            if(this.GlobalPosition.Y > this.spawnPosition.Y){
                this.GlobalPosition = this.spawnPosition;
                isFalling = false;
            }
        }
    }

    public override void OnTimerCDTimeout(){
        shootAlready = false;
        jump();
    }

    public void jump(){
        isJumping = true;
    }
}

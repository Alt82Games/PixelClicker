using Godot;
using System;

public partial class BasicLinearProjectile : Sprite2D
{
    Node2D objective = null;
    Vector2 direction = Vector2.Zero;
    int speed = 5;
    int projectileDamage = 200;

    Timer expireTimer,pathUpdate;
    Area2D hitArea;

    public override void _Ready()
    {
        expireTimer = GetNode<Timer>("expireTimer");
        pathUpdate = GetNode<Timer>("pathUpdate");
        hitArea = GetNode<Area2D>("hitArea");

        expireTimer.Timeout += OnTimerExpireTimerTimeout;
        hitArea.BodyEntered += OnHitAreaBodyEntered;
        pathUpdate.Timeout  += OnPathUpdateTimerTimeout;

    }


    public override void _PhysicsProcess(double delta)
    {
        move();
    }

    public override void _ExitTree()
    {
        expireTimer.Timeout -= OnTimerExpireTimerTimeout;
        hitArea.BodyEntered -= OnHitAreaBodyEntered;
        pathUpdate.Timeout  -= OnPathUpdateTimerTimeout;
    }
    
    public void OnPathUpdateTimerTimeout(){
        try{
            direction = this.Position.DirectionTo(objective.Position);
        }
        catch(ObjectDisposedException){

        }

    }

    public void OnTimerExpireTimerTimeout(){
        this.QueueFree();
    }

    public void OnHitAreaBodyEntered(Node2D body){
        try{
            UnitBase target = (UnitBase)body;
            target.recibeDamage(projectileDamage);
        }
        catch(Exception){

        };
        this.QueueFree();
    }

    public void move(){
        this.Position += direction * speed;
    }

    public void setObjective(Node2D obje,Vector2 position){
        this.Position = position;
        //
        if(obje == null){
        }
        else{
            objective = obje;
            direction = this.Position.DirectionTo(objective.Position);
        }
        
        
    }
    public Vector2 customRound(Vector2 pos){
        Vector2 roundDirection = Vector2.Zero;
        if(pos.X > 0){
            roundDirection.X = 1;
        }
        else if(pos.X < 0){
            roundDirection.X = -1;
        }
        if(pos.Y > 0){
            roundDirection.Y = 1;
        }
        else if(pos.Y < 0){
            roundDirection.Y = -1;
        }
        return roundDirection;
    }

}

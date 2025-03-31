using Godot;
using System;

public partial class BasicLinearProjectile : Sprite2D
{
    UnitBase objective = null;
    Vector2 direction = Vector2.Zero;
    float gravity = 0.095f;
    int aceleration = 50;
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
            //direction = this.GlobalPosition.DirectionTo(objective.GlobalPosition);
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
        this.GlobalPosition += (direction) * speed;
    }

    public void setObjective(Node2D obje,Vector2 GlobalPosition){
        this.GlobalPosition = GlobalPosition;
        //
        if(obje == null){
        }
        else{
             objective = (UnitBase)obje;
            Vector2 dir = objective.getEntityVol();
            float distance = this.GlobalPosition.DistanceTo(objective.GlobalPosition);
            int steps = (int)Math.Ceiling(distance/speed);
            Vector2 a = (objective.directionToObjective*objective.speed)*steps;
            if(objective.IsInGroup("Fly")){
                direction = this.GlobalPosition.DirectionTo(objective.GlobalPosition + a);
            }
            else{
                direction = this.GlobalPosition.DirectionTo(objective.GlobalPosition);
            }
            
            
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

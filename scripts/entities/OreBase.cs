using Godot;
using System;

public partial class OreBase : RigidBody2D
{
    //------------------------------Entity properties------------------------------
    
    int MAX_FORCE_X             = 100;
    int MIN_FORCE_X             = 60;
    int MAX_FORCE_Y             = -350;
    int MIN_FORCE_Y             = -275;
    float SPEED                 = 300;
    float JUMP_VELOCITY         = -400;
    float forceX                = 0.0f;
    float forceY                = 0.0f;
    float gravityScaleCustom    = 0.5f;
    bool canShift               = false;
    Vector2 selfVelocity = Vector2.Zero;
    Vector2 minVelocity = new Vector2(0.001f,0.001f);

    
    //---------------------------Other nodes references---------------------------

    Area2D clickArea; 

    //------------------------------Override functions----------------------------
    public override void _Ready()
    {
        clickArea = GetNode<Area2D>("clickArea");
        this.LockRotation = true;
        this.GravityScale = gravityScaleCustom;
        applyCustomImpulse();

        //******************************
        clickArea.InputEvent += OnClickAreaInputEnvent;


        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
        updateOrePositionOnFloor();
        base._PhysicsProcess(delta);
    }

    public override void _ExitTree()
    {
        
    }

    //-------------------------------Custom functions------------------------------

    public void updateOrePositionOnFloor(){
        if(this.LinearVelocity < minVelocity){
            if(canShift){
                this.LinearVelocity = Vector2.Zero;
                float x = (int)Math.Round(this.Position.X/3)*3;
                float y = this.Position.Y - 0.01f;
                Position = new Vector2(x,y);
                canShift = false;
            }
        }
        else{
                canShift = true;
            }
    }
    public void applyCustomImpulse(){
        forceX = GD.RandRange(MIN_FORCE_X, MAX_FORCE_X);
        forceY = GD.RandRange(MIN_FORCE_Y, MAX_FORCE_Y);
        ApplyCentralImpulse(new Vector2(forceX,forceY));
    }

    public void updateCoords(int x, int y){

    }

    public void OnClickAreaInputEnvent(Node viewport, InputEvent @event, long shapeIdx){
        if(@event.IsActionPressed("MouseLeftClick")){
            applyCustomImpulse();
        }
    }

}

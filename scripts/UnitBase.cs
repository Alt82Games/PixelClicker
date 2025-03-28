using Godot;
using System;

public partial class UnitBase : RigidBody2D
{

    //------------------------------Entity properties------------------------------
    int MAX_ROTATION            = 90;
    int MAX_FORCE_X             = 60;
    int MIN_FORCE_X             = 20;
    int MAX_FORCE_Y             = -80;                  //Negative to make it go up
    int MIN_FORCE_Y             = -100;

    int baseMaxHealt            = 1000;                 //TODO: change from hardcoded to obtain from gameManager
    int currentHealt;                                   //Initialize on _Ready()
    int defense                 = 0;                    //Override on extended class
    int baseClickDamage         = 200;                  //Override on extended class
    int currentRotation         = 0;

    bool isDead                 = false;

    
    Vector2 objective           = new Vector2(0,0);     //Initialize on _Ready() from gameManager
    Vector2 selfVelocity        = Vector2.Zero;
    Vector2 jumpImpulse         = new Vector2(0,-300);
    Vector2 objectivePosition;
    float speed                 = 0.3f;                    //Override on extended class
    float forceX                = 0.0f;
    float forceY                = 0.0f;
    float gravityScaleCustom    = 0.4f;
    
    //---------------------------Other nodes references---------------------------
    Sprite2D sprite;                                    //Initialize on _Ready()
    CollisionShape2D collisionShape;
    AnimationPlayer animationPlayer;
    HealtBar healtBar;
    Control baseClickArea, criticalClickArea;
    GameManager gameManager;

    //*********************************************
    Timer testTimer;

    //------------------------------Override functions----------------------------
    public override void _Ready()
    {
        gameManager = GetTree().Root.GetChild(0).GetNode<GameManager>("gameManager");
        
        healtBar = GetNode<HealtBar>("healtBar");
        healtBar.initializeHealthBar(baseMaxHealt);
        currentHealt = baseMaxHealt;

        baseClickArea                    = GetNode<Control>("baseClickArea");
        baseClickArea.GuiInput          += OnBaseClickAreaGuiInput;

        criticalClickArea                = GetNode<Control>("criticalClickArea");
        criticalClickArea.GuiInput      += OnCriticalClickAreaGuiInput;

        objectivePosition                = gameManager.getEntrancePosition();

        this.LockRotation = true;

        //****************************************
        testTimer = GetNode<Timer>("testTimer");
        startTimer();





        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
        move();
        base._PhysicsProcess(delta);
    }

    public override void _ExitTree()
    {
        testTimer.Timeout               -= OnTimerTestTimeout;
        baseClickArea.GuiInput          -= OnBaseClickAreaGuiInput;
        criticalClickArea.GuiInput      -= OnCriticalClickAreaGuiInput;
    }

    //-------------------------------Custom functions------------------------------
    public void startTimer(){
        testTimer.Timeout += OnTimerTestTimeout;
        testTimer.Start();
    }

    public void selfDie(){
        gameManager.setPoints(15);
        this.QueueFree();
    }

    public void move(){
        if(!isDead){
            objectivePosition = gameManager.getEntrancePosition();
            this.GravityScale = gravityScaleCustom;
            if (this.Position.DistanceTo(objectivePosition) > 3){
                selfVelocity = Position.DirectionTo(objectivePosition) * speed;
                MoveAndCollide(selfVelocity);
            }
            else{
                selfVelocity = Vector2.Zero;
            }
        }
    }

    public void jump(){
        ApplyCentralImpulse(jumpImpulse);
    }

    public void OnBaseClickAreaGuiInput(InputEvent @event){
        if(@event.IsActionPressed("MouseLeftClick")){
            currentHealt -= baseMaxHealt/5;
            healtBar.recibeDamage(currentHealt);
            if(currentHealt <= 0){
                selfDie();
            }
        }
    }
    public void OnCriticalClickAreaGuiInput(InputEvent @event){
        if(@event.IsActionPressed("MouseLeftClick")){
            currentHealt -= baseMaxHealt/2;
            healtBar.recibeDamage(currentHealt);
            if(currentHealt <= 0){
                selfDie();
            }
        }
    }

    
    

    public void OnTimerTestTimeout(){
        jump();

    }
}

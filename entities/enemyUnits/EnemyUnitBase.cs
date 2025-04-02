using Godot;
using System;

public partial class EnemyUnitBase : CharacterBody2D
{

//Variables and constants---------------------------------------------
    float maxHealt = 100;
    float currentHealt = 100;
    float jumpSpeedBase = 50;
    float jumpSpeed = 0;
    float jumpMultiplier = 4;
    protected float gravityScaleCustom    = 9.8f;
    public Vector2 directionToObjective = Vector2.Zero;
    public float speed = 50;

    int level = 0;

    public Vector2 target,currentPosition,lastPosition;


    //Node references-----------------------------------------------------

    protected GameManager gameManager;
    Control baseClickArea,criticalClickArea;
    HealtBar healtBar;
    Timer dashTimer;

    //Overrided functions-------------------------------------------------
    public override void _Ready()
    {
        gameManager = GetTree().Root.GetChild(0).GetNode<GameManager>("gameManager");

        healtBar = GetNode<HealtBar>("healtBar");
        healtBar.initializeHealthBar(maxHealt);

        baseClickArea = GetNode<Control>("baseClickArea");
        baseClickArea.GuiInput += OnBaseClickAreaGuiInput;

        criticalClickArea = GetNode<Control>("criticalClickArea");
        criticalClickArea.GuiInput += OnCriticalClickAreaGuiInput;

        dashTimer = GetNode<Timer>("dashTimer");
        dashTimer.Timeout += OnDashTimerTimeout;
        
        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
        move();
        
        base._PhysicsProcess(delta);
    }


    public override void _ExitTree()
    {
        baseClickArea.GuiInput -= OnBaseClickAreaGuiInput;
        criticalClickArea.GuiInput -= OnCriticalClickAreaGuiInput;
        dashTimer.Timeout -= OnDashTimerTimeout;
        base._ExitTree();
    }


    //Signal functions----------------------------------------------------


    private void OnCriticalClickAreaGuiInput(InputEvent input)
    {
        if(input.IsActionPressed("MouseLeftClick")){
            receiveClickDamage(true);
        }
    }


    private void OnBaseClickAreaGuiInput(InputEvent input)
    {
        if(input.IsActionPressed("MouseLeftClick")){
           receiveClickDamage(false);
            
        }
    }

    private void OnDashTimerTimeout()
    {
        dash();
    }

    //Custom functions----------------------------------------------------
    
    public void initialize(int level){
        this.level = level;
        speed = speed*level;
        currentHealt = maxHealt*level;
        healtBar.initializeHealthBar(maxHealt*level);
        target = gameManager.EnemyObjective;
        directionToObjective = GlobalPosition.DirectionTo(target);
        

    }

    public virtual void move(){
        target = gameManager.EnemyObjective;
        directionToObjective = GlobalPosition.DirectionTo(target);
        if(!IsOnFloor()){
            jumpSpeed += gravityScaleCustom;
            
        }
        if(this.GlobalPosition.DistanceTo(target) > 1){
            Velocity = (directionToObjective.Normalized()*speed);
            Velocity += new Vector2(0,jumpSpeed);
        }
        else{
            Velocity = Vector2.Zero;
        }
        
        MoveAndSlide();
    }

    public float getSpeed(){
        return speed;
    }

    public void dash(){
        if(IsOnFloor()){
            jumpSpeed = jumpSpeedBase*jumpMultiplier*-1;
        }
    }

    public void receiveDamage(float damage,bool critical){
        //TODO: Add function that convert some of the normal clicks to critical clicks
        float damageRecieve = damage;
        if(critical){
            damageRecieve *= gameManager.CriticalClickMultiplier;
        }
        currentHealt -= damageRecieve;
        healtBar.receiveDamage(currentHealt);
        showDamageNumber(damageRecieve);
        if(currentHealt <= 0){
               dead();                
        }
    }
    public void receiveClickDamage(bool critical){
        //TODO: Add function that convert some of the normal clicks to critical clicks
        float clickDamage = gameManager.GetClickDamage();
        int numberOfClicks = gameManager.ClicksPerClick;
        if(critical){
            clickDamage *= gameManager.CriticalClickMultiplier;
        }
        for(int i = 0; i < numberOfClicks; i++){
            currentHealt -= clickDamage;
            healtBar.receiveDamage(currentHealt);
            showDamageNumber(clickDamage);
             if(currentHealt <= 0){
               dead();                
            }
        }
        
    }
    public void showDamageNumber(float damage){
        //make a number appear over the enemy showing the damage that it recive for each click and if critical damage show other animation
    }
    public void dead(){
        //Start timer to queue free (2 sec)
        //Apply force to do dead animation, a little impulse up and back with a rotation
        QueueFree();

    }
}

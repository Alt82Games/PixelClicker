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
    public float speed = GD.RandRange(40,60);
    protected bool isDead = false;
    protected Vector2 initialVelocityDead = new Vector2(20,-98);
    protected Vector2 gravityDead = new Vector2(0,9.8f);

    protected int level = 0;
    protected int basePointsGiven = 7;

    public Vector2 target,currentPosition,lastPosition;
    String damageNumberScene = "res://components/damage_number_label.tscn";
    


    //Node references-----------------------------------------------------

    protected GameManager gameManager;
    Control baseClickArea,criticalClickArea;
    HealtBar healtBar;
    Timer dashTimer, deadTimer;
    Sprite2D spriteAtlas;

    //Overrided functions-------------------------------------------------
    public override void _Ready()
    {
        gameManager = GetTree().Root.GetChild(0).GetNode<GameManager>("gameManager");

        healtBar = GetNode<HealtBar>("healtBar");
        healtBar.initializeHealthBar(maxHealt);

        spriteAtlas = GetNode<Sprite2D>("spriteAtlas");

        baseClickArea = GetNode<Control>("baseClickArea");
        baseClickArea.GuiInput += OnBaseClickAreaGuiInput;

        criticalClickArea = GetNode<Control>("criticalClickArea");
        criticalClickArea.GuiInput += OnCriticalClickAreaGuiInput;

        dashTimer = GetNode<Timer>("dashTimer");
        dashTimer.Timeout += OnDashTimerTimeout;

        deadTimer = GetNode<Timer>("deadTimer");
        deadTimer.Timeout += OnDeadTimerTimeout;         
        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
        move((float)delta);
        
        base._PhysicsProcess(delta);
    }


    public override void _ExitTree()
    {
        baseClickArea.GuiInput -= OnBaseClickAreaGuiInput;
        criticalClickArea.GuiInput -= OnCriticalClickAreaGuiInput;
        dashTimer.Timeout -= OnDashTimerTimeout;
        deadTimer.Timeout -= OnDeadTimerTimeout;
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
    private void OnDeadTimerTimeout()
    {
        QueueFree();
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

    public virtual void move(float delta){
        if(!isDead){
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

    public float getSpeed(){
        return speed;
    }

    public void dash(){
        if(IsOnFloor() && !isDead){
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
        PackedScene objectToSpawn = GD.Load<PackedScene>(damageNumberScene);
        DamageNumberLabel instance = (DamageNumberLabel)objectToSpawn.Instantiate();
        instance.GlobalPosition = this.GlobalPosition;
        instance.setDamageText(damage);
        AddSibling(instance);
    }
    public void dead(){
        SetCollisionLayerValue(3,false);
        RemoveFromGroup("Enemy");
        baseClickArea.MouseFilter = (Control.MouseFilterEnum)2;
        criticalClickArea.MouseFilter = (Control.MouseFilterEnum)2; 
        isDead = true;
        Velocity = Vector2.Zero;
        gameManager.CurrentPoints += basePointsGiven*level; //TODO: Make it drop a coin or cristal to click or hover to pick up and gain the points instead
        deadTimer.Start();
    }
}

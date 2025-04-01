using Godot;
using System;

public partial class EnemyUnitBase : CharacterBody2D
{

//Variables and constants---------------------------------------------
    float maxHealt = 100;
    float currentHealt = 100;
    float speed = 50;
    float gravityScaleCustom    = 0.95f;

    protected Vector2 target,currentPosition,lastPosition;


    //Node references-----------------------------------------------------

    GameManager gameManager;
    Control baseClickArea,criticalClickArea;
    HealtBar healtBar;

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

        initialize();
        
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
        base._ExitTree();
    }


    //Signal functions----------------------------------------------------
    
    private void OnCriticalClickAreaGuiInput(InputEvent input)
    {
        if(input.IsActionPressed("MouseLeftClick")){
            recibeDamage(gameManager.GetClickDamage(),true);
        }
    }


    private void OnBaseClickAreaGuiInput(InputEvent input)
    {
        if(input.IsActionPressed("MouseLeftClick")){
            recibeDamage(gameManager.GetClickDamage(),false);
            
        }
    }

    //Custom functions----------------------------------------------------
    public void initialize(){
        currentHealt = maxHealt;
        target = gameManager.EnemyObjective;
        GD.Print(target);
        currentPosition = this.GlobalPosition;
        lastPosition = currentPosition;

    }

    public void move(){
        target = gameManager.EnemyObjective;
        Vector2 directionToObjective = GlobalPosition.DirectionTo(target);
        if(!IsOnFloor()){
            directionToObjective.Y += gravityScaleCustom;
        }
        //GD.Print(Position + ", " + target);
       // GD.Print("Distance: " + this.GlobalPosition.DistanceTo(target) );
        if(this.GlobalPosition.DistanceTo(target) > 1){
            Velocity = directionToObjective*speed;
            //GD.Print(Velocity);
        }
        else{
            Velocity = Vector2.Zero;
        }
        
        MoveAndSlide();
    }

    public void recibeDamage(float damage, bool critical){
        //TODO: Add function that convert some of the normal clicks to critical clicks
        float clickDamage = gameManager.GetClickDamage();
        int numberOfClicks = gameManager.ClicksPerClick;
        if(critical){
            clickDamage *= gameManager.CriticalClickMultiplier;
        }
        for(int i = 0; i < numberOfClicks; i++){
            GD.Print(clickDamage);
            currentHealt -= clickDamage;
            healtBar.recibeDamage(currentHealt);
            showDamageNumber();
             if(currentHealt <= 0){
               dead();                
            }
        }
        
    }
    public void showDamageNumber(){
        //make a number appear over the enemy showing the damage that it recive for each click and if critical damage show other animation
    }
    public void dead(){
        //Start timer to queue free (2 sec)
        //Apply force to do dead animation, a little impulse up and back with a rotation
        QueueFree();

    }
}

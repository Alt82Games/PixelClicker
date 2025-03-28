using Godot;
using System;

public partial class SpawnertBase : RigidBody2D
{
    //------------------------------Entity properties------------------------------
    int MAX_FORCE_X             = 100;
    int MIN_FORCE_X             = 60;
    int MAX_FORCE_Y             = -350;
    int MIN_FORCE_Y             = -275;
    int MIN_SPAWN_OBJECTS       = 1;
    int MAX_SPAWN_OBJECTS       = 6;

    int baseMaxHealt            = 20;                       //TODO: change from hardcoded to obtain from gameManager
    int currentHealt            = 20;                       //Initialize on _Ready()
    int baseClickDamage         = 5;

    //bool isDead                 = false;

    Vector2 objective           = new Vector2(0,0);         //Initialize on _Ready() from gameManager
    Vector2 selfVelocity        = Vector2.Zero;
    Vector2 jumpImpulse         = new Vector2(0,-300);
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
    Area2D clickArea;

    //*********************************************
    Timer testTimer;

    //------------------------------Override functions----------------------------
    public override void _Ready()
    {
        gameManager = GetTree().Root.GetChild(0).GetNode<GameManager>("gameManager");
        clickArea = GetNode<Area2D>("clickZone");
        clickArea.InputEvent += OnClickAreaInputEnvent;
        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
        
        base._PhysicsProcess(delta);
    }

    public override void _ExitTree()
    {
        clickArea.InputEvent -= OnClickAreaInputEnvent;
    }

    //-------------------------------Custom functions------------------------------

    public void damageOre(){
        currentHealt -= baseClickDamage;
        if(currentHealt < 0){
            int spawnNumber =  GD.RandRange(MIN_SPAWN_OBJECTS,MAX_SPAWN_OBJECTS);
            for(int i = 0; i < spawnNumber; i++){
                PackedScene objectToSpawn = GD.Load<PackedScene>("res://entities/Resourses/ore_base.tscn");
                RigidBody2D instance = (RigidBody2D)objectToSpawn.Instantiate();
                AddSibling(instance);
                instance.Position = new Vector2(this.Position.X+35,this.Position.Y-35);
                instance.LockRotation = true;
                currentHealt = baseMaxHealt;
            }
        }
    }
    public void OnClickAreaInputEnvent(Node viewport, InputEvent @event, long shapeIdx){
        if(@event.IsActionPressed("MouseLeftClick")){
            damageOre();
        }
    }

}

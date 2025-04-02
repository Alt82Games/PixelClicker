using Godot;
using System;

public partial class EnemySpawner : Node2D
{
    int MAX_COORD_X         = 0;
    int MIN_COORD_X         = 0;
    int MAX_COORD_Y         = 120;
    int MIN_COORD_Y         = -120;
    int maxRespawnTime      = 8;
    int minRespawnTime      = 2;
    int minObjectsToSpawn   = 10;
    int maxObjectsToSpawn   = 15;
    float forceX            = 0.0f;
    float forceY            = 0.0f;
    float gravityScaleCustom= 0.05f;
    Vector2 selfVelocity    = Vector2.Zero;

    Timer respawnTimer;

    public override void _Ready()
    {
        respawnTimer = GetNode<Timer>("respawnTimer");
        respawnTimer.Timeout += OnTimerRespawnTimerTimeout;
        base._Ready();
    }

    public override void _ExitTree()
    {
        respawnTimer.Timeout -= OnTimerRespawnTimerTimeout;
        base._ExitTree();
    }
    

    public int selectEnemy(){
        return GD.RandRange(0,1);
        //return 0; //Change to the last line when there are more enemies
    }
    public int selectEnemyLevel(){
        int temp = GD.RandRange(0,100);
        return (temp < 60) ? 1 : (temp < 80) ? 2 : 3;
    }

    public void OnTimerRespawnTimerTimeout(){
        int spriteAtlasChildIndex = 2;
        PackedScene [] scenes = [GD.Load<PackedScene>("res://entities/enemyUnits/enemy_unit_base.tscn"),
                                 GD.Load<PackedScene>("res://entities/enemyUnits/enemy_unit_flying.tscn")];
        for(int i = GD.RandRange(minObjectsToSpawn,maxObjectsToSpawn); i > 0; i--){
            int sceneIndex = selectEnemy();
            int level = selectEnemyLevel();
            EnemyUnitBase obj = (EnemyUnitBase)scenes[sceneIndex].Instantiate();
            float y = (float)GD.RandRange(MIN_COORD_Y,MAX_COORD_Y);
            obj.Position = new Vector2(this.Position.X , this.Position.Y + y);
                     
            if(level == 1){
                obj.GetChild<Sprite2D>(spriteAtlasChildIndex)
                .SelfModulate = new Color(0.05f,0.95f,0.05f,1);
                
            }
            else if(level == 2){
                obj.GetChild<Sprite2D>(spriteAtlasChildIndex)
                .SelfModulate = new Color(0.05f,0.05f,0.95f,1);
            }
            else if(level == 3){
                obj.GetChild<Sprite2D>(spriteAtlasChildIndex)
                .SelfModulate = new Color(0.95f,0.05f,0.05f,1);
            }
            
            AddSibling(obj);
            obj.initialize(level);
            respawnTimer.WaitTime = GD.RandRange(minRespawnTime,maxRespawnTime);
        }
        
    }



}
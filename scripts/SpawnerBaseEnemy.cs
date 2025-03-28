using Godot;
using System;

public partial class SpawnerBaseEnemy : RigidBody2D
{
    int MAX_COORD_X         = 0;
    int MIN_COORD_X         = 0;
    int MAX_COORD_Y         = 120;
    int MIN_COORD_Y         = -120;
    int maxRespawnTime      = 8;
    int minRespawnTime      = 2;
    int minObjectsToSpawn   = 1;
    int maxObjectsToSpawn   = 6;
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


    public void OnTimerRespawnTimerTimeout(){
        PackedScene [] scenes = [GD.Load<PackedScene>("res://entities/unit_base.tscn"),
                                 GD.Load<PackedScene>("res://entities/unit_base.tscn"),
                                 GD.Load<PackedScene>("res://entities/unit_base.tscn")];
        for(int i = GD.RandRange(minObjectsToSpawn,maxObjectsToSpawn); i > 0; i--){
            int sceneIndex = GD.RandRange(0,scenes.Length-1);
            RigidBody2D obj = (RigidBody2D)scenes[sceneIndex].Instantiate();
            float y = (float)GD.RandRange(MIN_COORD_Y,MAX_COORD_Y);
            obj.Position = new Vector2(this.Position.X , this.Position.Y + y);
            if(sceneIndex == 0){
                obj.GetChild<Sprite2D>(3).SelfModulate = new Color(0.48f,0.09f,0.06f,1);
            }
            else if(sceneIndex == 1){
                obj.GetChild<Sprite2D>(3).SelfModulate = new Color(0.86f,0.41f,0.06f,1);
            }
            else if(sceneIndex == 2){
                obj.GetChild<Sprite2D>(3).SelfModulate = new Color(0.25f,0.10f,0.80f,1);
            }
            
            AddSibling(obj);
            respawnTimer.WaitTime = GD.RandRange(minRespawnTime,maxRespawnTime);
        }
        
    }



}

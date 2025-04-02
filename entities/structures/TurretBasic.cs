using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class TurretBasic : StaticBody2D
{
    //Variables and constants---------------------------------------------
    List<Node2D> targets = new List<Node2D>();
    protected Vector2 spawnPosition;

    //Node references-----------------------------------------------------
    Timer timerCD;
    Area2D detectionArea;
    String projectileToShoot = "res://entities/projectiles/projectile_base.tscn";

    //Overrided functions-------------------------------------------------
    public override void _Ready()
    {
        timerCD = GetNode<Timer>("shotCD");
        timerCD.Start();

        detectionArea = GetNode<Area2D>("detectionArea");
        spawnPosition = this.GlobalPosition;


        timerCD.Timeout             += OnTimerCDTimeout;
        detectionArea.BodyEntered   += OnDetectionAreaBodyEntered;
        detectionArea.BodyExited    += OnDetectionAreaBodyExited;
        base._Ready();
    }

    public override void _ExitTree()
    {
        
        timerCD.Timeout             -= OnTimerCDTimeout;
        detectionArea.BodyEntered   -= OnDetectionAreaBodyEntered;
        detectionArea.BodyExited    -= OnDetectionAreaBodyExited;
    }

    //Signal functions----------------------------------------------------
    public virtual void OnTimerCDTimeout(){
        shoot(projectileToShoot);
    }
    public void OnDetectionAreaBodyEntered(Node2D body){
        //GD.Print("+1" + body);
    }
    public void OnDetectionAreaBodyExited(Node2D body){
       //GD.Print("-1"+ body);
    }

    //Custom functions----------------------------------------------------
    public void shoot(String projectile){
        Godot.Collections.Array<Node2D> overlap = detectionArea.GetOverlappingBodies();
        targets.Clear();
        int cont = 0;
        foreach (Node2D body in overlap){
                if(body.IsInGroup("Enemy")){
                    if(body.GlobalPosition.X-this.GlobalPosition.X>150){
                    targets.Add(body);
                    cont ++;
                    if (cont == 100){
                        break;
                    }
                }
                }
            }
        if(targets.Any()){
            Node2D target = null;
            foreach(Node2D body in targets){
                if(target == null){
                    target = body;
                }
                else{
                    //TODO: change to a tryCatch for the body in case is deleted
                    if(target == null){
                        break;
                    }
                    else{
                        if(this.GlobalPosition.DistanceTo(target.GlobalPosition) > this.GlobalPosition.DistanceTo(body.GlobalPosition)){
                            target = body;
                        }
                    }
                    
                }
            }
            
            PackedScene objectToSpawn = GD.Load<PackedScene>(projectile);
            ProjectileBase instance = (ProjectileBase)objectToSpawn.Instantiate();
            AddSibling(instance);
            instance.setObjective(target,this.GlobalPosition);
            
            //GD.Print("shoot: "+target);
        }
    }

}

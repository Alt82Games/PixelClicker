using Godot;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class BasicTurret : StaticBody2D
{
    Timer timerCD;
    Area2D detectionArea;
    public override void _Ready()
    {
        timerCD = GetNode<Timer>("shotCD");
        timerCD.Start();
        detectionArea = GetNode<Area2D>("detectionArea");
        


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


    public void OnTimerCDTimeout(){
        Godot.Collections.Array<Node2D> overlap = detectionArea.GetOverlappingBodies();
        List<Node2D> targets = new List<Node2D>();
        if(overlap.Any()){
            int cont = 0;
            targets.Clear();
            foreach (Node2D body in overlap){
                if(body.IsInGroup("Enemy")){
                    if(body.Position.DistanceTo(this.Position)>20){
                    targets.Add(body);
                    cont ++;
                    if (cont == 10){
                        break;
                    }
                }
                }
            }
            Node2D objective = null;
            foreach(Node2D body in targets){
                if(objective == null){
                    objective = body;
                }
                else{
                    if(objective == null){
                        break;
                    }
                    else{
                        if(this.Position.DistanceTo(objective.Position) > this.Position.DistanceTo(body.Position)){
                            objective = body;
                        }
                    }
                    
                }
            }
            PackedScene objectToSpawn = GD.Load<PackedScene>("res://entities/basic_linear_projectile.tscn");
            BasicLinearProjectile instance = (BasicLinearProjectile)objectToSpawn.Instantiate();
            AddSibling(instance);
            instance.setObjective(objective,this.Position);
        }
    }
    public void OnDetectionAreaBodyEntered(Node2D body){
        //GD.Print("+1" + body);
    }
    public void OnDetectionAreaBodyExited(Node2D body){
       //GD.Print("-1"+ body);
    }

}

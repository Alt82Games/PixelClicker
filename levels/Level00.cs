using Godot;
using System;

public partial class Level00 : Node2D
{
    //Variables and constants---------------------------------------------
        
    //Node references-----------------------------------------------------
    GameManager gameManager;
    Node2D target;
    Camera2D subCamera;
    SubViewport svport;

    //Overrided functions-------------------------------------------------
    public override void _Ready()
    {
        gameManager = GetTree().Root.GetChild(0).GetNode<GameManager>("gameManager");
        target = GetNode<Node2D>("base");
        subCamera   = GetNode<Camera2D>("hud/svpc/svp/subCamera");
        svport      = GetNode<SubViewport>("hud/svpc/svp");
        //GD.Print(target.GlobalPosition);

        gameManager.EnemyObjective = target.GlobalPosition;
        setSubCamera();
        base._Ready();
    }

    //Signal functions----------------------------------------------------

    //Custom functions----------------------------------------------------
    public void setSubCamera(){
        svport.World2D = GetTree().Root.World2D;
        subCamera.Position = target.GlobalPosition + new Vector2(450,-150);
        subCamera.Zoom = new Vector2(0.3f,0.3f);
    }
}

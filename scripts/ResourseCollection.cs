using Godot;
using System;

public partial class ResourseCollection : Area2D
{
    RigidBody2D ore;
    Area2D collectionAreaStone;
    GameManager gameManager;

    public override void _Ready()
    {
        gameManager = GetTree().Root.GetChild(0).GetNode<GameManager>("gameManager");
        this.BodyEntered += OnCollectionAreaStoneBodyEntered;
        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
    }

    public override void _ExitTree()
    {
        this.BodyEntered -= OnCollectionAreaStoneBodyEntered;
    }

    public void OnCollectionAreaStoneBodyEntered(Node2D body){
        if(body.IsInGroup("Resourse")){
            body.QueueFree();
            gameManager.setPoints(3);
        }
    }
}

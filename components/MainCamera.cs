using Godot;
using System;

public partial class MainCamera : Camera2D
{
    static int cameraSpeed = 5;
    Vector2 cameraUD = new Vector2(0,cameraSpeed);
    Vector2 cameraLR = new Vector2(cameraSpeed,0);
    public override void _Ready()
    {
        base._Ready();
    }
    public override void _PhysicsProcess(double delta)
    {
        moveCamera();
        base._PhysicsProcess(delta);
    }

    private void moveCamera()
    {
        if(Input.IsActionPressed ("Up")){
            GlobalPosition -= cameraUD;
        }
        if(Input.IsActionPressed ("Down")){
            GlobalPosition += cameraUD;
        }
        if(Input.IsActionPressed ("Left")){
            GlobalPosition -= cameraLR;
        }
        if(Input.IsActionPressed ("Right")){
            GlobalPosition += cameraLR;
        }
    }

}

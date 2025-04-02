using Godot;
using System;

public partial class Hud : CanvasLayer
{
    Label fps;
    public override void _Ready(){
        fps = GetNode<Label>("fps");
        fps.Text = Engine.GetFramesPerSecond().ToString();
    }
    public override void _PhysicsProcess(double delta)
    {
        fps.Text = Engine.GetFramesPerSecond().ToString();
        base._PhysicsProcess(delta);
    }

}

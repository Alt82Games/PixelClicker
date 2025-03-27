using Godot;
using System;

public partial class World00 : Node2D
{

    Camera2D camera;
    Node2D entrance;
    GameManager gameManager;
    static int cameraSpeed = 5;
    Vector2 cameraUD = new Vector2(0,cameraSpeed);
    Vector2 cameraLR = new Vector2(cameraSpeed,0);
    Vector2 cameraZoom = new Vector2(0.25f,0.25f);
    Vector2 maxZoom = new Vector2(5,5);
    Vector2 minZoom = new Vector2(3,3);

    public override void _Ready()
    {
        camera      = GetNode<Camera2D>("MainCamera");
        entrance    = GetNode<Node2D>("entrance");
        gameManager = GetNode<GameManager>("gameManager");

        gameManager.setEntrancePosition(entrance.Position);
        
    }
    public override void _Process(double delta)
    {
        moveCamera();
    }
    public override void _Input(InputEvent @event)
    {

    }
    public void moveCamera(){
        if(Input.IsActionPressed ("CameraUp")){
            camera.GlobalPosition -= cameraUD;
        }
        if(Input.IsActionPressed("CameraDown")){
            camera.GlobalPosition += cameraUD;
        } 
        if(Input.IsActionPressed("CameraLeft")){
            camera.GlobalPosition -= cameraLR;
        }    
        if(Input.IsActionPressed("CameraRight")){
            camera.GlobalPosition += cameraLR;
        }
        if(Input.IsActionJustPressed("ZoomIn")){
            if(camera.Zoom < maxZoom){
                camera.Zoom += cameraZoom;
            }
        }
        if(Input.IsActionJustPressed("ZoomOut")){
            if(camera.Zoom > minZoom){
                camera.Zoom -= cameraZoom;
            }
        }
    }

}

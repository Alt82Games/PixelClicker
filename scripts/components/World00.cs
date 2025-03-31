using Godot;
using System;

public partial class World00 : Node2D
{

    Camera2D camera, subCamera;
    SubViewport svport;
    Node2D entrance;
    GameManager gameManager;
    int cameraTargetIndex = 0;
    float cameraSpeed;
    Vector2 [] cameraObjectives = [new Vector2(-720,-130), new Vector2(80,-220)];
    Vector2 cameraTarget;
    Vector2 cameraZoom = new Vector2(2f,2f);
    Vector2 [] zoom = [new Vector2(2,2), new Vector2(1,1)];
    Label pointsLabel;

    public override void _Ready()
    {
        camera      = GetNode<Camera2D>("MainCamera");
        subCamera   = GetNode<Camera2D>("hud/svpc/svp/subCamera");
        svport      = GetNode<SubViewport>("hud/svpc/svp");
        entrance    = GetNode<Node2D>("entrance");
        gameManager = GetNode<GameManager>("gameManager");
        pointsLabel = GetNode<Label>("hud/points");

        gameManager.setPointsLabel(pointsLabel);

        gameManager.setEntrancePosition(entrance.Position);
        camera.Position= cameraObjectives[0]; 
        cameraTarget = cameraObjectives[0];
        cameraObjectives[1] = new Vector2(86,-220);
        camera.Zoom = zoom[0];

        cameraSpeed = cameraObjectives[0].DistanceTo(cameraObjectives[1])/100;


        
        
    }
    public override void _Process(double delta)
    {
        detectImput();
        moveCamera();
        setSubCamera();
    }
    public override void _Input(InputEvent @event)
    {

    }

    public void detectImput(){
        if(Input.IsActionPressed ("CameraUp")){
            
        }
        if(Input.IsActionPressed("CameraDown")){
            
        } 
        if(Input.IsActionPressed("CameraLeft")){
            cameraTargetIndex = 0;
            
            
        }    
        if(Input.IsActionPressed("CameraRight")){
            cameraTargetIndex = 1;
        }
    }
    public void moveCamera(){
        cameraTarget = cameraObjectives[cameraTargetIndex];      
        if(camera.Position == cameraTarget){

        }
        else{
                Vector2 moveDirection = camera.Position.DirectionTo(cameraTarget);
                //moveDirection = customRound(moveDirection);
                moveDirection = (moveDirection * cameraSpeed);
                camera.Position += moveDirection;
                updateZoom();
                if(camera.Position.DistanceTo(cameraTarget) < 10){
                    camera.Position = cameraTarget;
                    
                }
                
        }
        
    }

    public void updateZoom(){
        Vector2 zoomVelocity = new Vector2(0.01f,0.01f);
        if(cameraTargetIndex == 0 && camera.Zoom < zoom[0]){
            camera.Zoom += zoomVelocity;
        }
        if(cameraTargetIndex == 1 && camera.Zoom > zoom[1]){
            camera.Zoom -= zoomVelocity;
        }
    }

    public Vector2 customRound(Vector2 pos){
        Vector2 roundDirection = Vector2.Zero;
        if(pos.X > 0){
            roundDirection.X = 1;
        }
        else if(pos.X < 0){
            roundDirection.X = -1;
        }
        if(pos.Y > 0){
            roundDirection.Y = 1;
        }
        else if(pos.Y < 0){
            roundDirection.Y = -1;
        }
        return roundDirection;
    }

    public void setSubCamera(){
        svport.World2D = GetTree().Root.World2D;
        subCamera.Position = entrance.Position + new Vector2(300,-150);
        subCamera.Zoom = new Vector2(0.3f,0.3f);
    }

}

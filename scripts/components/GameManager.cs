using Godot;
using System;

public partial class GameManager : Node2D
{

    float currentDificulty = 1.0f;
    float baseHP = 100;
    float points = 0;
    Vector2 entrancePosition = Vector2.Zero;
    Label pointLabel;

    public override void _Ready()
    {
        base._Ready();
    }


    public float getCurrentDificulty(){
        return currentDificulty;
    }
    public void setCurrentDificutly(float diff){
        currentDificulty = diff;
    }
    public float getPoints(){
        return points;
    }
    public void setPoints(int x){
        points += x;
        pointLabel.Text = "Points: " + points.ToString();
    }
    public float getBaseHP(){
        return baseHP * currentDificulty;
    }
    public void setPointsLabel(Label x){
        pointLabel = x;
    }
    public Label getPointsLabel(){
        return pointLabel;
    }
    public void setEntrancePosition(Vector2 position){
        entrancePosition = position;
    }
    public Vector2 getEntrancePosition(){
        return entrancePosition;
    }

}

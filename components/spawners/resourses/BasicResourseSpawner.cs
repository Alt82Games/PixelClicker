using Godot;
using System;

public partial class BasicResourseSpawner : StaticBody2D
{
    //Variables and constants---------------------------------------------
    float maxHealt = 10;
    float currentHealt = 10;
    //Node references-----------------------------------------------------
    Control hitArea;
    GameManager gameManager;
    //Overrided functions-------------------------------------------------
    public override void _Ready()
    {
        gameManager = GetTree().Root.GetChild(0).GetNode<GameManager>("gameManager");

        hitArea = GetNode<Control>("hitArea");
        hitArea.GuiInput += OnHitAreaGuiInput;
        
        base._Ready();
    }

    public override void _ExitTree()
    {
        hitArea.GuiInput -= OnHitAreaGuiInput;
        base._ExitTree();
    }


    //Signal functions----------------------------------------------------

    public void OnHitAreaGuiInput(InputEvent input){
        if(input.IsActionPressed("MouseLeftClick")){
            int baseClickDamage = gameManager.BaseClickDamage;
            int clickMultiplier = gameManager.ClickDamageMultiplier;
            int numberOfClicks = gameManager.ClicksPerClick;
            for(int i = 0; i <= numberOfClicks; i++){
                currentHealt -= baseClickDamage*clickMultiplier;
                if(currentHealt <= 0){
                    currentHealt = maxHealt;
                    spawnResourse();
                }
            }
        }
    }

    

    //Custom functions----------------------------------------------------
    public void initialize(){
        currentHealt = maxHealt;
    }
    public void spawnResourse(){
        GD.Print("SpawnResourceExecuted");
    }
}

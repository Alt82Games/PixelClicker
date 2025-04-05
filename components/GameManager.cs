using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class GameManager : Node2D
{
    private int currentPoints = 0;
    private int baseClickDamage = 10;
    private float baseProjectileDamage = 100;
    private float fireSpeedActual = 2;
    private int currentLevel = 0;
    private int currentHorde = 0;
    private int clicksPerClick = 1;
    private int clickDamageMultiplier = 1;
    private int criticalClickMultiplier = 2;

    Vector2 enemyObjective = Vector2.Zero;

    public int CurrentPoints            {get{return currentPoints;}             set{currentPoints = value;}}
    public int BaseClickDamage          {get{return baseClickDamage;}           set{baseClickDamage = value;}}
    public float BaseProjectileDamage   {get{return baseProjectileDamage;}      set{baseProjectileDamage = value;}}

    public int CurrentLevel             {get{return currentLevel;}              set{currentLevel = value;}}
    public int CurrentHorde             {get{return currentHorde;}              set{currentHorde = value;}}
    public int ClicksPerClick           {get{return clicksPerClick;}            set{clicksPerClick = value;}}
    public int ClickDamageMultiplier    {get{return clickDamageMultiplier;}     set{clickDamageMultiplier = value;}}
    public int CriticalClickMultiplier  {get{return criticalClickMultiplier;}   set{criticalClickMultiplier = value;}}
    public Vector2 EnemyObjective       {get{return enemyObjective;}            set{enemyObjective = value;}}
    public float FireSpeedActual        {get{return fireSpeedActual;}           set{fireSpeedActual = value;}}

    public float GetClickDamage(){return baseClickDamage*clickDamageMultiplier;}


    //Dificulty-----------------------------------------
    

    //Handle queue free
    public List<Node2D> queueFreeList = new List<Node2D>();
    public override void _Process(double delta)
    {
        if(queueFreeList.Any()){
            for (int i = 0; i<10; i++){
                if(queueFreeList.Any()){
                    queueFreeList[0].QueueFree();
                    queueFreeList.RemoveAt(0);
                }
                else{
                    break;
                }
            }
        }
        base._Process(delta);
    }

}

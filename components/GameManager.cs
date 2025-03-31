using Godot;
using System;

public partial class GameManager : Node2D
{
    private int baseClickDamage = 1;
    private int currentLevel = 0;
    private int currentHorde = 0;
    private int clicksPerClick = 1;
    private int clickDamageMultiplier = 1;
    Vector2 enemyObjective = Vector2.Zero;

    public int BaseClickDamage          {get{return baseClickDamage;}           set{baseClickDamage = value;}}
    public int CurrentLevel             {get{return currentLevel;}              set{currentLevel = value;}}
    public int CurrentHorde             {get{return currentHorde;}              set{currentHorde = value;}}
    public int ClicksPerClick           {get{return clicksPerClick;}            set{clicksPerClick = value;}}
    public int ClickDamageMultiplier    {get{return clickDamageMultiplier;}     set{clickDamageMultiplier = value;}}
    public Vector2 EnemyObjective       {get{return enemyObjective;}            set{enemyObjective = value;}}
}

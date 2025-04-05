using Godot;
using System;
using System.Linq;

public partial class ProjectileBase : CharacterBody2D
{
    //Variables and constants---------------------------------------------
    EnemyUnitBase target = null;
    Vector2 direction = Vector2.Zero;
    int aceleration = 50;
    Vector2 targetInitialPosition;
    Vector2 targetPredictedPosition,targetPredictedPositionTuned;
    [Export]int speed = 200;
    [Export]float projectileDamage = 200;
    float gravityScaleCustom    = 9.8f;
    float stepsPredicted;
    //float vx, x, vy, y;
        
    //Node references-----------------------------------------------------
    GameManager gameManager;
    Timer expireTimer,pathUpdate;
    Area2D hitArea;

    //Overrided functions-------------------------------------------------
    public override void _Ready()
    {
        gameManager = GetTree().Root.GetChild(0).GetNode<GameManager>("gameManager");
        expireTimer = GetNode<Timer>("expireTimer");
        pathUpdate = GetNode<Timer>("pathUpdate");
        hitArea = GetNode<Area2D>("hitArea");
        
        expireTimer.Timeout += OnTimerExpireTimerTimeout;
        hitArea.BodyEntered += OnHitAreaBodyEntered;
        pathUpdate.Timeout  += OnPathUpdateTimerTimeout;

    }


    public override void _PhysicsProcess(double delta)
    {
        move(delta);
    }

    public override void _ExitTree()
    {
        expireTimer.Timeout -= OnTimerExpireTimerTimeout;
        hitArea.BodyEntered -= OnHitAreaBodyEntered;
        pathUpdate.Timeout  -= OnPathUpdateTimerTimeout;
    }

    //Signal functions----------------------------------------------------

    public void OnPathUpdateTimerTimeout(){
        try{
            //direction = this.GlobalPosition.DirectionTo(target.GlobalPosition);
        }
        catch(ObjectDisposedException){

        }

    }

    public void OnTimerExpireTimerTimeout(){
        this.QueueFree();
    }

    public void OnHitAreaBodyEntered(Node2D body){
        try{
            if(body.IsInGroup("Enemy")){
                    EnemyUnitBase target = (EnemyUnitBase)body;
                    target.receiveDamage(projectileDamage,false);//TODO: CHECK FOR CRITICAL HITS FOR THE PROJECTILES MAYBE A UPGRADE WITH GM
                }
        }
        catch(Exception exc){
            GD.Print(exc);
        }
        gameManager.queueFreeList.Add(this);
        
    }

    //Custom functions----------------------------------------------------

    public void move(double delta){
        MoveAndSlide();
        Velocity += new Vector2(0,(float)(gravityScaleCustom*delta));
    }

    public Vector2 calculateInterceptionPoint(float dist){
        float distance1 = dist;
        float steps = distance1/speed;
        
        targetPredictedPosition = targetInitialPosition + target.directionToObjective*(target.getSpeed()*steps);
        
        float distance2 = this.GlobalPosition.DistanceTo(targetPredictedPosition);
        float steps2 = distance2/speed;
        
        targetPredictedPositionTuned = targetInitialPosition + target.directionToObjective*(target.getSpeed()*steps2);
        //GD.Print(targetPredictedPosition.DistanceTo(targetPredictedPositionTuned));
        
        if(targetPredictedPosition.DistanceTo(targetPredictedPositionTuned) > 0.01){
            //TODO: Implement this recursive if
            
            return calculateInterceptionPoint(distance2);
        }
        else{
            //GD.Print("Expected movement: "+target.directionToObjective*(target.getSpeed()*steps));
            //GD.Print("POS1:" +target.GlobalPosition + "POS1Expected:" + targetPredictedPosition);
            //GD.Print("Expected movement: "+target.directionToObjective*(target.getSpeed()*steps2));
            //GD.Print("POS2:" +target.GlobalPosition + "POS2Expected:" + targetPredictedPositionTuned);
            stepsPredicted = steps2;
            return targetPredictedPositionTuned;
        }
    }

    public void setObjective(Node2D obje,Vector2 GlobalPosition, float damage){
        this.GlobalPosition = GlobalPosition;
        this.projectileDamage = damage;
        //
        if(obje == null){
        }
        else{
            target = (EnemyUnitBase)obje;
            targetInitialPosition = target.GlobalPosition;
            float distance = this.GlobalPosition.DistanceTo(target.GlobalPosition);
            float hipotenusa = (float)Math.Sqrt(Math.Pow((target.GlobalPosition.Y-GlobalPosition.Y),2) + Math.Pow((target.GlobalPosition.X-GlobalPosition.X),2));
            Vector2 targetPredictedPositionLocal = calculateInterceptionPoint(this.GlobalPosition.DistanceTo(target.GlobalPosition));

            float x = (targetPredictedPositionLocal.X-this.GlobalPosition.X);
            float vx = speed;
            float timeToIntercept = x/vx;
            float y = (targetPredictedPositionLocal.Y-this.GlobalPosition.Y)*-1;
            float vy = (float)((vx/x)*(y+((gravityScaleCustom*0.5f)*(Math.Pow(x,2)/Math.Pow(vx,2)))));
            Velocity = new Vector2(vx,-vy);
            
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

}

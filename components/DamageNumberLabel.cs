using Godot;
using System;

public partial class DamageNumberLabel : Node2D
{
	//Variables and constants---------------------------------------------
        
    //Node references-----------------------------------------------------
	Timer timer;
	Label damageText;
	Vector2 gravity = new Vector2(0,9.8f);
	Vector2 initialVelocity = new Vector2(20,-49);

	float damage = 0;
    
    //Overrided functions-------------------------------------------------
    public override void _Ready(){
		timer = GetNode<Timer>("despawnTimer");
		damageText = GetNode<Label>("damageText");

		timer.Timeout += OnTimerTimeout;
		damageText.Text = damage.ToString();
    }
    public override void _PhysicsProcess(double delta)
    {
		moveText((float)delta);
        base._PhysicsProcess(delta);
    }


    public override void _ExitTree(){
        timer.Timeout -= OnTimerTimeout;
    }

	public void moveText(float delta){
		this.GlobalPosition += initialVelocity*delta-gravity*delta;
		initialVelocity += gravity;
	}
    public void setDamageText(float damage){
		this.damage = damage;
	}
    //Signal functions----------------------------------------------------
	private void OnTimerTimeout()
    {
        this.QueueFree();
    }

    //Custom functions----------------------------------------------------

}

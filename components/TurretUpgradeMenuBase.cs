using Godot;
using System;

public partial class TurretUpgradeMenuBase : Control
{
    //Variables and constants---------------------------------------------
    int damageUpgradeCounter = 1;
    int fireRateUpgradeCounter = 1;
    int damageUpgradePercent = 10, fireRateUpgradePercent = 5;
    //Node references-----------------------------------------------------
    Sprite2D upgradeMenuBackground;
    TextureButton upgradeDamage,upgradeFireRate;
    TurretBasic parent;
    
    //Overrided functions-------------------------------------------------
    public override void _Ready(){
        parent = GetParent<TurretBasic>();

        upgradeMenuBackground = GetNode<Sprite2D>("upgradeMenuBackground");
        upgradeMenuBackground.Visible = false;
        upgradeDamage = GetNode<TextureButton>("upgradeDamage");
        upgradeDamage.Visible = false;
        upgradeDamage.FocusMode = 0;
        upgradeFireRate = GetNode<TextureButton>("upgradeFireRate");
        upgradeFireRate.Visible = false;
        upgradeFireRate.FocusMode = 0;

        GuiInput                    += OnUpgradeMenuGuiInput;
        MouseExited                 += OnUpgradeMenuMouseExited;
        upgradeDamage.Pressed       += OnUpgradeDamageButtonPressed;
        upgradeFireRate.Pressed     += OnUpgradeFireRateButtonPressed;
    }

    public override void _ExitTree(){
        GuiInput                    -= OnUpgradeMenuGuiInput;
        MouseExited                 -= OnUpgradeMenuMouseExited;
        upgradeDamage.Pressed       -= OnUpgradeDamageButtonPressed;
        upgradeFireRate.Pressed     -= OnUpgradeFireRateButtonPressed;
    }
    //Signal functions----------------------------------------------------
    private void OnUpgradeMenuMouseExited()
    {
        closeUpgradeMenu();
    }


    private void OnUpgradeMenuGuiInput(InputEvent @event)
    {
        if(@event.IsActionPressed("MouseLeftClick")){
            //GD.Print("UpgradeMenuInput");
            openUpgradeMenu();
        }
    }

    private void OnUpgradeDamageButtonPressed()
    {
        if(damageUpgradeCounter < 9){
            damageUpgradeCounter++;
            parent.ProjectileDamage += parent.BaseProjectileDamage/100*damageUpgradePercent;
            GD.Print("UpgradeDamage: " + parent.ProjectileDamage);
        }
        else{
            damageUpgradeCounter++;
            parent.ProjectileDamage += parent.BaseProjectileDamage/100*damageUpgradePercent;
            GD.Print("UpgradeDamage: " + parent.ProjectileDamage);
            upgradeDamage.Disabled = true;
        }
        
    }

    private void OnUpgradeFireRateButtonPressed()
    {
        if (fireRateUpgradeCounter < 9){
            parent.FireRate -= (float)Math.Round((parent.BaseFireRate/100)*fireRateUpgradePercent , 2);
            fireRateUpgradeCounter++; 
            GD.Print("UpgradeFireRate: " + parent.FireRate);
        }
        else{
            parent.FireRate -= (float)Math.Round((parent.BaseFireRate/100)*fireRateUpgradePercent , 2);
            GD.Print("UpgradeFireRate: " + parent.FireRate);
            upgradeFireRate.Disabled = true;
        }
        
    }

    //Custom functions----------------------------------------------------

    public int DamageUpgradePercent{get{return damageUpgradePercent;} set{damageUpgradePercent = value;}}
    public int FireRateUpgradePercent{get{return fireRateUpgradePercent;} set{fireRateUpgradePercent = value;}}

    public void openUpgradeMenu(){
        upgradeMenuBackground.Visible = true;
        upgradeDamage.Visible = true;
        upgradeFireRate.Visible = true;
    }
    
    public void closeUpgradeMenu(){
        upgradeMenuBackground.Visible = false;
        upgradeDamage.Visible = false;
        upgradeFireRate.Visible = false;
    }
}

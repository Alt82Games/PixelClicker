using Godot;
using System;

public partial class HealtBar : TextureProgressBar
{
    public void initializeHealthBar(int maxHealt){
        this.MaxValue = maxHealt;
        this.Value = maxHealt;
        this.Step = maxHealt/100;
    }
    public void recibeDamage(float currentHealt){
        this.Value = currentHealt;
    }
}

using Godot;
using System;

public partial class DamagePopup : Label
{
    [Export] public float FloatSpeed = 50.0f;       // How fast the popup floats upward
    [Export] public float FadeDuration = 1.0f;      // Duration for the fade-out effect

    private float _timer = 0.0f;

    public override void _Ready()
    {
        Scale = new Vector2(3, 3); 
        Modulate = new Color(1, 1, 1, 1);  // Start fully opaque
    }

    public override void _Process(double delta)
    {
        // Move the popup upwards
        Position += new Vector2(0, -FloatSpeed * (float)delta);

        // Handle fading out
        _timer += (float)delta;
        float alpha = 1.0f - (_timer / FadeDuration);
        Modulate = new Color(Modulate.R, Modulate.G, Modulate.B, alpha);

        // Free the popup when fully faded
        if (_timer >= FadeDuration)
        {
            QueueFree();
        }
    }

    // Method to set the damage amount
    public void SetDamage(int damageAmount)
    {
        Text = damageAmount.ToString();
    }
}

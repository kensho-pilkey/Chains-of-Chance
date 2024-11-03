using Godot;
using System;

public partial class SpecialCard : Button
{
    [Export] public string EffectType;  // Type of special effect (e.g., "DoubleHealth", "AddCards")
    [Export] public int EffectValue;    // Value of the effect (e.g., 2 for "Add 2 Cards")
	[Export] public string Description;
    private Label _label;
    private TextureRect _cardTexture;
	private int cost = 1;

    public override void _Ready()
    {
        // Set up nodes
		cost = new RandomNumberGenerator().RandiRange(8, 12);
        _label = GetNode<Label>("Label");
        _cardTexture = GetNode<TextureRect>("CardTexture");

        UpdateCardAppearance();
    }

    private void UpdateCardAppearance()
    {
        // Set card name and texture based on the effect
        _label.Text = EffectType;
        switch (EffectType)
        {
            case "DoubleHealth":
				Description = "Doubles the player's health.";
				_label.Text = _label.Text + "\n" + Description;
                _cardTexture.Texture = GD.Load<Texture2D>("res://Assets/card_sprite.png");
                break;
            case "AddCards":
                _cardTexture.Texture = GD.Load<Texture2D>("res://Assets/card_sprite.png");
                break;
            case "Shield":
                _cardTexture.Texture = GD.Load<Texture2D>("res://Assets/card_sprite.png");
                break;
            // Add more cases for other special effects as needed
            default:
                _cardTexture.Texture = GD.Load<Texture2D>("res://Assets/card_sprite.png");
                break;
        }
    }
	public void Select() {
		GetNode<TextureRect>("glow").Visible = true;
	}
	public void UnSelect() {
		GetNode<TextureRect>("glow").Visible = false;
	}
	private void _on_pressed() {
		if (GetParent().GetParent() is Shop shop) {
			shop.OnSpecialSelected(this);
			GetParent().GetParent().GetNode<Button>("Buy").Text = "Buy $" + cost;
		}
	}

    private void UseSpecial()
    {
        // Trigger the effect based on the EffectType
        switch (EffectType)
        {
            case "DoubleHealth":
                ApplyDoubleHealth();
                break;
            case "AddCards":
                AddRandomCards(EffectValue);
                break;
            case "Shield":
                ApplyShield();
                break;
            // Add more cases for other special effects
        }

        // Optionally, destroy or hide the card after use
        QueueFree();
    }

    private void ApplyDoubleHealth()
    {
        // Example of doubling player's health
        Global.Instance.PlayerHealth *= 2;
        GD.Print("Player health doubled!");
    }

    private void AddRandomCards(int numberOfCards)
    {
        // Example of adding random cards to the player's deck
        for (int i = 0; i < numberOfCards; i++)
        {
            Global.Instance.PlayerCards.Add(Global.Instance.getRandomCard());
        }
        GD.Print($"{numberOfCards} random cards added to player's deck.");
    }

    private void ApplyShield()
    {
        // Example of applying a shield to a card for two turns
        //Global.Instance.ApplyShieldToCard(2); // Apply a shield effect that lasts for 2 turns
        GD.Print("Shield applied to card for 2 turns.");
    }
}

using System;
using Godot;
using System.Collections.Generic;

public partial class SpecialCard : Button
{
    private RandomNumberGenerator rng = new RandomNumberGenerator();

    private readonly Dictionary<string, int> effects = new Dictionary<string, int>
    {
        { "DoubleHealth", 0 },
        { "AddCards", 2 },    // "AddCards" effect with a default value of 2 cards
        { "PlusFour", 4 }      // "PlusFour" effect with a fixed value of 4 levels
    };

    public string EffectType;  // Type of special effect (e.g., "DoubleHealth", "AddCards")
    public int EffectValue;    // Value of the effect (e.g., 2 for "Add 2 Cards")
    public string Description;
    private RichTextLabel _label;
    private TextureRect _cardImage;
    public int cost;

    public override void _Ready()
    {
        rng.Randomize();
        cost = rng.RandiRange(8, 12);
        SelectRandomEffect();  // Randomly choose an effect type and value
        _label = GetNode<RichTextLabel>("Label");
        _cardImage = GetNode<TextureRect>("TextureRect");

        UpdateCardAppearance();
    }

    private void SelectRandomEffect()
    {
        var effectList = new List<string>(effects.Keys);
        EffectType = effectList[rng.RandiRange(0, effectList.Count - 1)];
        EffectValue = effects[EffectType];
    }

    private void UpdateCardAppearance()
    {
        _label.Text = EffectType;
        switch (EffectType)
        {
            case "DoubleHealth":
                Description = "Doubles all cards' health.";
                _cardImage.Texture = GD.Load<Texture2D>("res://Assets/health.png");
                break;
            case "AddCards":
                Description = $"Adds {EffectValue} random card(s) to your deck.";
                _cardImage.Texture = GD.Load<Texture2D>("res://Assets/plusTwoCards.png");


                break;
            case "PlusFour":
                Description = $"Grants +4 levels to all cards' damage stat.";
                _cardImage.Texture = GD.Load<Texture2D>("res://Assets/plusFour.png");
                break;
            default:
                Description = "An unknown power.";
                _cardImage.Texture = GD.Load<Texture2D>("res://Assets/health.png");
                break;
        }

        _label.BbcodeEnabled = true;
        _label.Text = Description;
    }

    public void Select()
    {
        GetNode<TextureRect>("glow").Visible = true;
    }

    public void UnSelect()
    {
        GetNode<TextureRect>("glow").Visible = false;
    }

    private void _on_pressed()
    {
        if (GetParent().GetParent() is Shop shop)
        {
            shop.OnSpecialSelected(this);
            GetParent().GetParent().GetNode<Button>("Buy").Text = "Buy $" + cost;
        }
    }

    public void UseSpecial()
    {
        switch (EffectType)
        {
            case "DoubleHealth":
                ApplyDoubleHealth();
                break;
            case "AddCards":
                AddRandomCards(EffectValue);
                break;
            case "PlusFour":
                ApplyPlusFour();
                break;
            // Add more cases for other special effects
        }

        QueueFree(); // Destroy or hide the card after use
    }

    private void ApplyDoubleHealth()
    {
        foreach (var card in Global.Instance.PlayerCards)
        {
            card.Health *= 2;
        }
    }

    private void AddRandomCards(int numberOfCards)
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            Global.Instance.AddCard(Global.Instance.createRandomCard());
        }
    }

    private void ApplyPlusFour()
    {
        foreach (var card in Global.Instance.PlayerCards)
        {
            card.Damage += 4;
        }
    }
}

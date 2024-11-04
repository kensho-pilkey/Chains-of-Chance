using Godot;
using System.Collections.Generic;
using System.Linq;


public partial class RoundSummary : Node2D
{
    private Label playedCardsLabel;
    private Label defeatedCardsLabel;
    private Label goldLabel;

    public List<Card> PlayedCards { get; private set; }
    public List<Card> DefeatedCards { get; private set; }
    public int GoldEarned { get; private set; }

    public RoundSummary()
    {
        PlayedCards = new List<Card>();
        DefeatedCards = new List<Card>();
        GoldEarned = 0;
    }

    public override void _Ready()
    {
        // Initialize labels
        playedCardsLabel = GetNode<Label>("PlayedCardsLabel");
        defeatedCardsLabel = GetNode<Label>("DefeatedCardsLabel");
        goldLabel = GetNode<Label>("GoldLabel");

        GetTree().CreateTimer(1.0f).Timeout += () => {
            GetNode<HBoxContainer>("HBoxContainer").GetChild(Global.Instance.LevelNum - 1).GetNode<TextureRect>("glow").Visible = true;
        };
    }

    public void AddPlayedCard(Card card)
    {
        PlayedCards.Add(card);
        UpdateDisplay();
    }

    public void AddDefeatedCard(Card card)
    {
        DefeatedCards.Add(card);
        UpdateDisplay();
    }

    public void AddGold(int amount)
    {
        GoldEarned += amount;
        UpdateDisplay();
    }

private void UpdateDisplay()
{
    //playedCardsLabel.Text = "Played Cards:\n" + string.Join("\n", PlayedCards.Select(card => card.Name));
    playedCardsLabel.Text = "Played Cards:" + Global.Instance.PlayerCards[1].Name;
    //defeatedCardsLabel.Text = "Defeated Cards:\n" + string.Join("\n", DefeatedCards.Select(card => card.Name));
    defeatedCardsLabel.Text = "Defeated Cards:" + Global.Instance.OpponentCards[1].Name;
    goldLabel.Text = "Gold Earned: " + GoldEarned;
}


public void DisplaySummary()
{
    var opponentCards = GetOpponentCards();
    // Handle display logic for opponent cards if necessary
}


private Card CreateCardFromData(CardData cardData)
{
    // Assume you have a method to preload or instantiate Card scenes
    Card card = (Card)GD.Load<PackedScene>("res://path/to/Card.tscn").Instantiate();

    // Set up the card's data
    card.SetCardData(cardData); // Create a method in Card class to set the card's data
    return card;
}

    public void _on_next_pressed()
    {
        Global.Instance.StartNextTurn();
    }
    private List<Card> GetOpponentCards()
    {
    // Assuming you have a way to instantiate Card objects from CardData
    List<Card> opponentCards1 = new List<Card>();

    foreach (var cardData in Global.Instance.OpponentCards)
    {
        Card card = CreateCardFromData(cardData); // Implement this method based on your card creation logic
        opponentCards1.Add(card);
    }

    return opponentCards1;
    }

}

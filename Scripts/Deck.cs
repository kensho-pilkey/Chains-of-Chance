using System;
using System.Collections.Generic;
using Godot;

public partial class Deck : Control
{
    [Export] public PackedScene CardScene;
    private GridContainer _cardDisplayContainer;

    public override void _Ready()
    {
        _cardDisplayContainer = GetNode<GridContainer>("CardDisplayContainer");
    }

    public void LoadPlayerCards()
    {
        // Get the list of cards from the singleton
        List<CardData> playerCards = Global.Instance.PlayerCards;
        List<CardData> availableCardsForTurn = Global.Instance.availableCardsForTurn;

        // Instantiate a Card scene for each CardData in PlayerCards
        foreach (var cardData in playerCards)
        {
            if (CardScene != null && CardScene.Instantiate() is CardStill instance)
            {
                _cardDisplayContainer.AddChild(instance);
                instance.AddData(cardData);

                // Check if the card has been played
                if (!availableCardsForTurn.Contains(cardData))
                {
                    // Set modulate to 50% transparency for played cards
                    instance.Modulate = new Color(instance.Modulate.R, instance.Modulate.G, instance.Modulate.B, 0.5f);
                }
            }
        }
    }

    private void RemoveCards()
    {
        // Remove all children from the container
        foreach (var child in _cardDisplayContainer.GetChildren())
        {
            _cardDisplayContainer.RemoveChild(child);
        }
    }

    private void _on_button_pressed()
    {
        Visible = false;
        GetParent().GetNode<Control>("Sidebar").Visible = true;
        GetParent().GetNode<Button>("Draw").Visible = true;
        GetParent().GetNode<Button>("Play").Visible = true;

        RemoveCards();
    }
}

using Godot;
using System;
using System.Collections.Generic;

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

        // Instantiate a Card scene for each CardData in PlayerCards
        foreach (var cardData in playerCards)
        {
            if (CardScene != null && CardScene.Instantiate() is CardStill instance)
            {
                _cardDisplayContainer.AddChild(instance);
                instance.AddData(cardData);
            }
        }
    }
    private void RemoveCards() {
        // Remove all children from the container
        foreach (var child in _cardDisplayContainer.GetChildren())
        {
            _cardDisplayContainer.RemoveChild(child);
        }
    }
    private void _on_button_pressed(){
        Visible = false;
        GetParent().GetNode<Control>("Sidebar").Visible = true;
        RemoveCards();
    }
}

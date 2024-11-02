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
		LoadPlayerCards();
	}

	private void LoadPlayerCards()
    {
        // Get the list of cards from the singleton
        List<CardData> playerCards = Global.Instance.PlayerCards;

        // Instantiate a Card scene for each CardData in PlayerCards
        foreach (var cardData in playerCards)
        {
            if (CardScene != null && CardScene.Instantiate() is CardStill instance)
            {
                _cardDisplayContainer.AddChild(instance);
            }
        }
    }
    private void _on_button_pressed(){
        Visible = false;
    }
}

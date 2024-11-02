using Godot;
using System;
using System.Collections.Generic;

public partial class Board : Control
{
	public List<CardData> OpponentCards { get; private set; } = new List<CardData>();
	private List<CardSlot> _opponentSlots = new List<CardSlot>(); 
	[Export] public PackedScene CardScene;
	private Control _opponentPlayArea;
	public override void _Ready()
	{
		_opponentPlayArea = GetNode<HBoxContainer>("P2Slots");
		generateCards(Global.Instance.LevelNum * 5);
		foreach (CardSlot slot in _opponentPlayArea.GetChildren())
        {
            _opponentSlots.Add(slot);
        }
	}

	public override void _Process(double delta)
	{
	}

	private void generateCards(int num) {
		for (int i = 0; i <	num; i++) {
			OpponentCards.Add(Global.Instance.createRandomCard());
		}
	}
	public void PlayHand()
    {
        if (OpponentCards.Count == 0 || !hasSlotAvailable())
        {
            GD.Print("Opponent has no more cards to play.");
            return;
        }

        CardData selectedCard = OpponentCards[0];

        if (CardScene.Instantiate() is Card cardInstance)
        {
            // cardInstance.SetCardData(selectedCard);
			cardInstance.PlaceCardInSlot(getNextSlot());
			// cardInstance.Rotation = Mathf.DegToRad(180);
            _opponentPlayArea.AddChild(cardInstance);

            GD.Print("Opponent played card: " + selectedCard.Name);

            // Remove the played card from the opponent's deck
            OpponentCards.RemoveAt(0);
        }
    }

	//returns true if a slot is open
	public Boolean hasSlotAvailable() {
		return _opponentSlots.Exists(slot => !slot.IsOccupied());
	}
	//randomly selects one slot from the remaining slots
	public CardSlot getNextSlot() {
		var openSlots = _opponentSlots.FindAll(slot => !slot.IsOccupied());

        // Return a random open slot if any are available
        if (openSlots.Count > 0)
        {
            int randomIndex = (int)GD.Randi() % openSlots.Count;
            return openSlots[randomIndex];
        }

        GD.Print("No available slots found.");
        return null; // No open slots
	}
}

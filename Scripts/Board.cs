using Godot;
using System;
using System.Collections.Generic;

public partial class Board : Control
{
	public List<CardData> OpponentCards { get; private set; } = new List<CardData>();
	private List<OpponentSlot> _opponentSlots = new List<OpponentSlot>(); 
	private Control _opponentPlayArea;
	private Card _cardInstance;
	public override void _Ready()
	{
		_opponentPlayArea = GetNode<HBoxContainer>("P2Slots");
		generateCards(Global.Instance.LevelNum * 5);
		foreach (OpponentSlot slot in _opponentPlayArea.GetChildren())
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

	// Get the next available slot
	OpponentSlot availableSlot = getNextSlot();
	
	// Check if there is an available slot before placing the card
	if (availableSlot != null)
	{
		_cardInstance = availableSlot.SpawnCard();

		GD.Print("Opponent played card: " + selectedCard.Name);

		// Remove the played card from the opponent's deck
		OpponentCards.RemoveAt(0);
	}
	else
	{
		GD.Print("No open slot available to place the card.");
	} 

}


	//returns true if a slot is open
	public Boolean hasSlotAvailable() {
		return _opponentSlots.Exists(slot => !slot.IsOccupied());
	}
	//randomly selects one slot from the remaining slots
	public OpponentSlot getNextSlot()
{
    var openSlots = _opponentSlots.FindAll(slot => !slot.IsOccupied());

    // Check if there are any open slots
    if (openSlots.Count > 0)
    {
        int randomIndex = (int)(GD.Randi() % openSlots.Count);
        return openSlots[randomIndex];
    }

    GD.Print("No available slots found.");
    return null; // No open slots
}

}

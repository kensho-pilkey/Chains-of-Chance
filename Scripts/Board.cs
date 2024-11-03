using Godot;
using System;
using System.Collections.Generic;

public partial class Board : Control
{
	public List<CardData> OpponentCards { get; private set; } = new List<CardData>();
	private List<OpponentSlot> _opponentSlots = new List<OpponentSlot>(); 
	private List<CardSlot> _playerSlots = new List<CardSlot>();
	private Control _opponentPlayArea;
	private Control _playerPlayArea;

	private Card _cardInstance;
	public override void _Ready()
	{
		_opponentPlayArea = GetNode<HBoxContainer>("P2Slots");
		_playerPlayArea = GetNode<HBoxContainer>("P1Slots");
		generateCards(Global.Instance.LevelNum * 5);
		foreach (OpponentSlot slot in _opponentPlayArea.GetChildren())
        {
            _opponentSlots.Add(slot);
        }
		foreach (CardSlot slot in _playerPlayArea.GetChildren())
		{
			_playerSlots.Add(slot);
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
    while (OpponentCards.Count != 0 && hasSlotAvailable())
	{
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
	GD.Print("Opponent has no more cards to play.");
    return;
    

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
public void Attack()
	{
		// Iterate over each slot, assuming slots are in corresponding positions
		for (int i = 0; i < _playerSlots.Count && i < _opponentSlots.Count; i++)
		{
			CardSlot playerSlot = _playerSlots[i];
			OpponentSlot opponentSlot = _opponentSlots[i];

			if (playerSlot.IsOccupied() && opponentSlot.IsOccupied())
			{
				Card playerCard = playerSlot.GetCard();
				Card opponentCard = opponentSlot.GetCard();

				// Process the attack
				if (playerCard != null && opponentCard != null)
				{
					playerCard.Attack(opponentCard);
					opponentCard.Attack(playerCard);

					// Check health and remove cards if defeated
					if (playerCard._Health <= 0)
					{
						playerSlot.RemoveCard();
						playerCard.Destroy();
						playerSlot.TakeDamage();
					}
					if (opponentCard._Health <= 0)
					{
						opponentSlot.RemoveCard();
						opponentCard.Destroy();
						opponentSlot.TakeDamage();
					}
				}
			}
			else if (playerSlot.IsOccupied() && !opponentSlot.IsOccupied())
			{
				// Handle the case where the opponent slot is empty
				Global.Instance.OpponentHealth -= playerSlot.GetCard()._Damage;

			}
			else if (!playerSlot.IsOccupied() && opponentSlot.IsOccupied())
			{
				// Handle the case where the player slot is empty
				Global.Instance.PlayerHealth -= opponentSlot.GetCard()._Damage;
			}
			else {
				// Both slots are empty
			}
			//trigger next card draw ig
			PlayHand();
			//GetParent().GetNode<CardDrawer>("CardDrawer").DrawCards(GetNode<CardDrawer>("CardDrawer").GlobalPosition, 3);
		}
	}
}

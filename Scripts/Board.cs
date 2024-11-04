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
	private int _currentAttackIndex = 0;
	private Timer _attackTimer;
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
    CalculateMultipliers();

    // Setup or retrieve the Timer node
    if (_attackTimer == null)
    {
        _attackTimer = new Timer();
        _attackTimer.WaitTime = 1.0f; // 1 second delay
        _attackTimer.OneShot = true;
        _attackTimer.Timeout += OnAttackTimerTimeout;
        AddChild(_attackTimer);
    }

    // Reset the attack index
    _currentAttackIndex = 0;

    // Start the first attack cycle
    if (_playerSlots.Count > 0 && _opponentSlots.Count > 0)
    {
        ProcessAttack();
        _attackTimer.Start();
    }
}

private void CalculateMultipliers()
{
    int maxMultiplier = 1;
    int currentMultiplier = 1;
    string lastElement = null;

    int maxCreatureStreak = 1;
    int currentCreatureStreak = 1;
    string lastCreatureName = null;

    for (int i = 0; i < _playerSlots.Count; i++)
    {
        if (_playerSlots[i].IsOccupied())
        {
            string currentElement = _playerSlots[i].GetCard()._cardData.ElementType;
            string currentCreatureName = _playerSlots[i].GetCard()._cardData.Name;

            if (currentElement == lastElement)
            {
                currentMultiplier++;
            }
            else
            {
                currentMultiplier = 1; // Reset if element changes
            }
            maxMultiplier = Math.Max(maxMultiplier, currentMultiplier);
            lastElement = currentElement;

            if (currentCreatureName == lastCreatureName)
            {
                currentCreatureStreak++;
            }
            else
            {
                currentCreatureStreak = 1; // Reset if creature name changes
            }
            maxCreatureStreak = Math.Max(maxCreatureStreak, currentCreatureStreak);
            lastCreatureName = currentCreatureName;
        }
    }

    if (maxMultiplier > 1 || maxCreatureStreak > 1)
    {
        GetParent<GameScene>().ScreenShake(50.0f, 0.5f, 0.03f);
    }
    Global.Instance.Multiplier = maxMultiplier + maxCreatureStreak;
}

private void OnAttackTimerTimeout()
{
    // Move to the next attack
    _currentAttackIndex++;

    if (_currentAttackIndex < _playerSlots.Count && _currentAttackIndex < _opponentSlots.Count)
    {
        ProcessAttack();
        _attackTimer.Start(); // Start the timer again for the next pair
    }
}

private void ProcessAttack()
{
    CardSlot playerSlot = _playerSlots[_currentAttackIndex];
    OpponentSlot opponentSlot = _opponentSlots[_currentAttackIndex];

    if (playerSlot.IsOccupied() && opponentSlot.IsOccupied())
    {
        Card playerCard = playerSlot.GetCard();
        Card opponentCard = opponentSlot.GetCard();

        if (playerCard != null && opponentCard != null)
        {
            playerCard.Attack(opponentCard);
            opponentCard.Attack(playerCard);

            if (playerCard._Health <= 0)
            {
                playerSlot.RemoveCard();
                playerCard.Destroy();
                playerSlot.TakeDamage();
				if (opponentCard._Health > 0) {
					opponentCard.PlayAttackAnimation();
				}
            }
            if (opponentCard._Health <= 0)
            {
                opponentSlot.RemoveCard();
                opponentCard.Destroy();
                opponentSlot.TakeDamage();
				if (playerCard._Health > 0) {
					playerCard.PlayAttackAnimation();
				}
            }
        }
    }
    else if (playerSlot.IsOccupied() && !opponentSlot.IsOccupied())
    {
        Global.Instance.OpponentHealth -= playerSlot.GetCard()._Damage * Global.Instance.Multiplier;
		playerSlot.GetCard().PlayAttackAnimation();
    }
    else if (!playerSlot.IsOccupied() && opponentSlot.IsOccupied())
    {
        Global.Instance.PlayerHealth -= opponentSlot.GetCard()._Damage;
		opponentSlot.GetCard().PlayAttackAnimation();
    }

    // Trigger the next set of cards if necessary
    PlayHand();
    GetParent().GetNode<CardDrawer>("CardDrawer").DrawCards(GetParent().GetNode<CardDrawer>("CardDrawer").GlobalPosition, 3);
}

}

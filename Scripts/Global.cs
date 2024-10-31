using Godot;
using System;
using System.Collections.Generic;

public partial class Global : Node
{
	public int PlayerScore { get; set; } = 0;
	public List<CardData> PlayerCards { get; private set; } = new List<CardData>();
	public override void _Ready()
	{
		PlayerCards.Add(new CardData("Dragon", "res://assets/dragon.png", 10, 10, "Fire"));
		//above is example of adding card to player deck.
		//TODO create randomizer to generate a random card.
		Timer timer = new Timer();
        timer.WaitTime = 1.0f; // Set the timer to fire every 1 second
        timer.Autostart = true; // Start automatically when added to the scene
        timer.Connect("timeout", new Callable(this, nameof(OnTimerTimeout))); // Connect timeout signal
        AddChild(timer); // Add timer to the node tree

        // Start the timer
        timer.Start();

		
	}
	private void OnTimerTimeout()
    {
        // Check if there is at least one card in the player's deck
        if (PlayerCards.Count > 0)
        {
            // Print the details of the first card in PlayerCards (in this case, "Dragon")
            GD.Print(PlayerCards[0].ToString());
        }
    }
	public CardData createRandomCard() {
		Random random = new Random();
		
		return new CardData("Dragon", "res://assets/dragon.png", 10, 10, "Fire");
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

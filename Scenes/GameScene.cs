using Godot;
using System;

public partial class GameScene : Node2D
{
	private int turn = 0;
	private int player1Score = 0;
	private int player2Score = 0;
	private int player1Health = 100;
	private int player2Health = 100;

	public override void _Ready()
	{
		StartRound();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(player1Health <= 0) {
			EndGame(false);
		}
		else if (player2Health <= 0) {
			EndGame(true);
		}
	}

	public void StartRound() {
		//Player draws 5 cards 
		//TODO card draw logic and storage of existing cards.

	}
	public void EndRound() {
		//TODO 
	}
	public void EndGame(Boolean player1Wins) {
		//TODO switch to end scene
	}
}

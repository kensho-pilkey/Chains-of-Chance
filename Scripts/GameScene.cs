using Godot;
using System;

public partial class GameScene : Node2D
{
	private int turn = 0;

	public override void _Ready()
	{
		Global.Instance.StartTurn();
		StartRound();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Global.Instance.PlayerHealth <= 0) {
			EndGame(false);
		}
		else if (Global.Instance.OpponentHealth <= 0) {
			EndGame(true);
			Global.Instance.StartNextTurn();
		}
	}

	public void StartRound() {
		//Player draws 5 cards 
		//TODO card draw logic and storage of existing cards.
		GetNode<Board>("board").PlayHand();
		GetNode<CardDrawer>("CardDrawer").DrawCards(GetNode<CardDrawer>("CardDrawer").GlobalPosition, 7);
	}
	public void AttackPhase() {
		//whoever placed first 
		GetNode<Board>("board").Attack();
	}
	public void EndRound() {
		//TODO 
	}
	public void changeTurn() {
		//TODO Swap turns, was thinking even turns could be player 1 and odd turns could be player 2 or something.
		turn ++;
	}
	public void EndGame(Boolean player1Wins) {
		if (player1Wins) {
			GD.Print("Player 1 Wins!");
		}
		else {
			GD.Print("You lose!");
		}
		//TODO switch to end scene and implement end game logic
	}
}

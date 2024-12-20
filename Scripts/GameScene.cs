using Godot;
using System;

public partial class GameScene : Node2D
{
	private int turn = 0;

	public override void _Ready()
	{
		Global.Instance.StartTurn();
		StartRound();
		var audioPlayer2D = GetNode<AudioStreamPlayer2D>("SoundEffectPlayer2");
		audioPlayer2D.Play();
	}
	private void _on_sound_effect_player_2_finished()
{
    var audioPlayer2D = GetNode<AudioStreamPlayer2D>("SoundEffectPlayer2");
    audioPlayer2D.Play(); // Replay the sound to create a loop
}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
	public async void ScreenShake(float intensity = 50.0f, float duration = 1.0f, float frequency = 0.05f)
	{
		var camera = GetNode<Camera2D>("Camera2D"); // Adjust the path as needed
		Vector2 originalPosition = camera.Position;

		float timeElapsed = 0.0f;
		while (timeElapsed < duration)
		{
			// Apply random offset with intensity
			camera.Position = originalPosition + new Vector2(
				(GD.Randf() * 2 - 1) * intensity, 
				(GD.Randf() * 2 - 1) * intensity
			);

			// Wait for a short interval before next shake
			await ToSignal(GetTree().CreateTimer(frequency), "timeout");
			timeElapsed += frequency;
		}

		// Reset camera to original position after shake ends
		camera.Position = originalPosition;
	}



	public void StartRound() {
		//Player draws 5 cards 
		//TODO card draw logic and storage of existing cards.
		GetNode<Board>("board").PlayHand();
		GetNode<CardDrawer>("CardDrawer").DrawCards(7);
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
   			GetTree().ChangeSceneToFile("res://Scenes/WinScreen.tscn");
		}
		else {
			GD.Print("You lose!");
			GetTree().ChangeSceneToFile("res://Scenes/GameOver.tscn");
		}
		//TODO switch to end scene and implement end game logic
	}
}

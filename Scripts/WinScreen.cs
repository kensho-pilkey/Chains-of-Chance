using Godot;
using System;

public partial class WinScreen : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public void _on_play_again_pressed() {
		Global.Instance.ResetGame();
	}
    public void _on_quit_pressed() {
		GetTree().Quit();
	}

	public override void _Ready()
	{

	}
	public override void _Process(double delta)
	{

	}

}

using Godot;
using System;

public partial class WinScreen : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public void _on_play_again_pressed() {
		GetTree().ChangeSceneToFile("res://Scenes/game_scene.tscn");
	}
    public void _on_quit_pressed() {
		GetTree().Quit();
	}

}

using Godot;
using System;

public partial class Ui : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void _on_deck_pressed(){
		GetNode<Control>("Deck").Visible = true;
	}
}

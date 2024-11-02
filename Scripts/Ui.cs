using Godot;
using System;

public partial class Ui : Control
{
	private void _on_deck_pressed(){
		GetNode<Control>("Deck").Visible = true;
	}
	private void _on_shop_pressed(){
		GetNode<Control>("Shop").Visible = true;
	}
	private void _on_play_pressed(){
		//initiate attack phase of round
	}
}

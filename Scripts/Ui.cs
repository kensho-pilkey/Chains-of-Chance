using Godot;
using System;

public partial class Ui : Control
{
	private void _on_deck_pressed(){
		GetNode<Control>("Deck").Visible = !GetNode<Control>("Deck").Visible;
		GetNode<Deck>("Deck").LoadPlayerCards();
		GetNode<Control>("Sidebar").Visible = false;
	}
	private void _on_play_pressed(){
		//initiate attack phase of round
		GetParent().GetNode<Board>("board").Attack();
	}
}

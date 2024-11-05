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
	private Label _money;
	private Label _playerHealth;
	private Label _cardCount;
	private Label _drawCount;
	private Label _level;
	public override void _Ready()
	{
		_level = GetNode<Label>("LevelInfo");
		_money = GetNode<Label>("MoneyInfo");
		_playerHealth = GetNode<Label>("PlayerHealth");
		_cardCount = GetNode<Label>("CardCount");
		_drawCount = GetNode<Label>("DrawCount");
	}
	public override void _Process(double delta)
	{
		_playerHealth.Text = "Player Health: " + Global.Instance.PlayerHealth.ToString();
		_level.Text = "Level: " + Global.Instance.LevelNum.ToString();
		_money.Text = Global.Instance.Money.ToString();
		_cardCount.Text = Global.Instance.CardCount().ToString();
		_drawCount.Text = Global.Instance.Draws.ToString();
	}

}

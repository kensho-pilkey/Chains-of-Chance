using Godot;
using System;

public partial class Sidebar : Control
{
	private Label _level;
	private Label _money;
	private Label _playerHealth;
	private Label _opponentHealth;
	private Label _multiplier;
	private Label _cardCount;
	private Label _drawCount;
	public override void _Ready()
	{
		_level = GetNode<Label>("LevelInfo");
		_money = GetNode<Label>("MoneyInfo");
		_playerHealth = GetNode<Label>("PlayerHealth");
		_opponentHealth = GetNode<Label>("OpponentHealth");
		_multiplier = GetNode<Label>("CurrentMult");
		_cardCount = GetNode<Label>("CardCount");
		_drawCount = GetNode<Label>("DrawCount");
	}
	public override void _Process(double delta)
	{
		_playerHealth.Text = "Player Health: " + Global.Instance.PlayerHealth.ToString();
		_opponentHealth.Text = "Opponent Health: " + Global.Instance.OpponentHealth.ToString();
		_level.Text = "Level: " + Global.Instance.LevelNum.ToString();
		_money.Text = Global.Instance.Money.ToString();
		_multiplier.Text = "X" + Global.Instance.Multiplier.ToString();
		_cardCount.Text = Global.Instance.CardCount().ToString();
		_drawCount.Text = Global.Instance.Draws.ToString();
	}
}

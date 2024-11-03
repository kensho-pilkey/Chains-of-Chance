using Godot;
using System;

public partial class Sidebar : Control
{
	private Label _turn;
	private Label _level;
	private Label _money;
	private Label _playerHealth;
	private Label _opponentHealth;
	public override void _Ready()
	{
		_turn = GetNode<Label>("TurnInfo");
		_level = GetNode<Label>("LevelInfo");
		_money = GetNode<Label>("MoneyInfo");
		_playerHealth = GetNode<Label>("PlayerHealth");
		_opponentHealth = GetNode<Label>("OpponentHealth");
	}
	public override void _Process(double delta) {
		_playerHealth.Text = "Player Health: " + Global.Instance.PlayerHealth;
		_opponentHealth.Text = "Opponent Health: " + Global.Instance.OpponentHealth;
	}
	public void UpdateTurn(int turn) {
		_turn.Text = "Turn: " + turn;
	}
	public void UpdateLevel(int level) {
		_level.Text = "Level: " + level;
	}
	public void UpdateMoney(int money) {
		_money.Text = "Money: " + money;
	}
	
}

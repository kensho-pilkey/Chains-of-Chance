using Godot;
using System;

public partial class Sidebar : Control
{
	private Label _turn;
	private Label _level;
	private Label _money;
	public override void _Ready()
	{
		_turn = GetNode<Label>("TurnInfo");
		_level = GetNode<Label>("LevelInfo");
		_money = GetNode<Label>("MoneyInfo");
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

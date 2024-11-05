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
	private TextureProgressBar _playerBar;
	private TextureProgressBar _opponentBar;
	private int maxPlayerHP;
	private int maxOpponentHP;
	public override void _Ready()
	{
		_level = GetNode<Label>("LevelInfo");
		_money = GetNode<Label>("MoneyInfo");
		_playerHealth = GetNode<Label>("PlayerHealth");
		_opponentHealth = GetNode<Label>("OpponentHealth");
		_multiplier = GetNode<Label>("CurrentMult");
		_cardCount = GetNode<Label>("CardCount");
		_drawCount = GetNode<Label>("DrawCount");
		_playerBar = GetNode<TextureProgressBar>("PlayerBar");
		_playerBar.MaxValue = Global.Instance.PlayerHealth;
		_opponentBar = GetNode<TextureProgressBar>("OpponentBar");
		_opponentBar.MaxValue = Global.Instance.OpponentHealth;
		maxPlayerHP = Global.Instance.PlayerHealth;
		maxOpponentHP = Global.Instance.OpponentHealth;
	}
	public override void _Process(double delta)
	{
		_playerHealth.Text = Global.Instance.PlayerHealth.ToString() + " / " + maxPlayerHP.ToString();
		_opponentHealth.Text = Global.Instance.OpponentHealth.ToString() + " / " + maxOpponentHP.ToString();
		_level.Text = "Level: " + Global.Instance.LevelNum.ToString();
		_money.Text = " x " + Global.Instance.Money.ToString();
		_multiplier.Text = "X" + Global.Instance.Multiplier.ToString();
		_cardCount.Text = " x " + Global.Instance.CardCount().ToString();
		_drawCount.Text = " x " + Global.Instance.Draws.ToString();
		_playerBar.Value = Global.Instance.PlayerHealth;
		_opponentBar.Value = Global.Instance.OpponentHealth;

	}
}

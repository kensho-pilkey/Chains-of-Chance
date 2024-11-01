using Godot;
using System;

public partial class CardStill : Button
{
	private TextureRect _cardTexture;
	private Label _label;
	private CardData _cardData = null;
	public override void _Ready()
	{
		_cardTexture = GetNode<TextureRect>("CardTexture");
		_label = GetNode<Label>("Label");
		_cardData = Global.Instance.getRandomCard();
		UpdateCardAppearance();
	}

	private void UpdateCardAppearance()
	{
		_cardTexture.Texture = GD.Load<Texture2D>(_cardData.AssetPath);
		_label.Text = _cardData.Name + "\n" + _cardData.Damage + "\n" + _cardData.Health;

	}
}

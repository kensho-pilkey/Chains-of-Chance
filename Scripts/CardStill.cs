using Godot;
using System;

public partial class CardStill : Button
{
	private TextureRect _cardTexture;
	private TextureRect _cardImage;
	private Label _label;
	public CardData _cardData = null;
	public int cost = 1;
	private Label _dmghp;
	public override void _Ready()
	{
		_cardTexture = GetNode<TextureRect>("CardTexture");
		_cardImage = GetNode<TextureRect>("CardImage");
		_label = GetNode<Label>("Label");
		_cardData = Global.Instance.getRandomCard();
		_dmghp = GetNode<Label>("dmgHp");
		UpdateCardAppearance();
		
	}


	private void UpdateCardAppearance()
	{
		_dmghp.Text = _cardData.Damage + "       " + _cardData.Health;
		_cardImage.Texture = GD.Load<Texture2D>(_cardData.AssetPath);
		_label.Text = _cardData.Name;
		if(_cardData.ElementType == "Fire") {
			_cardTexture.Texture = GD.Load<Texture2D>("res://Assets/fire_card_sprite.png");
			cost = new RandomNumberGenerator().RandiRange(1, 4);
		}
		else if(_cardData.ElementType == "Water") {
			_cardTexture.Texture = GD.Load<Texture2D>("res://Assets/water_card_sprite.png");
			cost = 5;
		}
		else if(_cardData.ElementType == "Grass") {
			_cardTexture.Texture = GD.Load<Texture2D>("res://Assets/grass_card_sprite.png");
			cost = 5;
		}

	}
	public void AddData(CardData data) {
		_cardData = data;
		UpdateCardAppearance();
	}

	public void Select() {
		GetNode<TextureRect>("glow").Visible = true;
	}
	public void UnSelect() {
		GetNode<TextureRect>("glow").Visible = false;
	}
	private void _on_pressed() {
		if (GetParent().GetParent() is Shop shop) {
			shop.OnCardSelected(this);
			GetParent().GetParent().GetNode<Button>("Buy").Text = "Buy $" + cost;
		}
	}
}

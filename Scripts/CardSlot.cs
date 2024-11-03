using Godot;
using System;

public partial class CardSlot : Control
{
    private bool _occupied = false;
    private Card _currentCard = null;
	private int _health;

    [Export] private Color HoverColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);
    [Export] private Color DefaultColor = new Color(1, 1, 1, 1); // clear
	[Export] private Color BlueColor = new Color(0.5f, 0.5f, 1.0f, 0.8f); //Light blue
	[Export] private Color RedColor = new Color(1.0f, 0.5f, 0.5f, 0.8f); //Light red
	[Export] private Color GreenColor = new Color(0.5f, 1.0f, 0.5f, 0.8f); //Light green

    public override void _Ready()
    {
		_health = new RandomNumberGenerator().RandiRange(4, 5);
        Modulate = DefaultColor;
    }
	public override void _Process(double delta) {
		GetNode<Sprite2D>("Sprite2D").Frame = 5 - _health;
	}

    public void PlaceCard(Card card)
    {
        if (!_occupied)
        {
            _occupied = true;
            _currentCard = card;
            if (card._cardData.ElementType == "Fire") {
				Modulate = RedColor;
			}
			else if (card._cardData.ElementType == "Water") {
				Modulate = BlueColor;
			}
			else if (card._cardData.ElementType == "Grass") {
				Modulate = GreenColor;
			}
        }
    }

	public Boolean IsOccupied(){
		return _occupied;
	}

	public Vector2 GetCenterPosition()
    {
		ColorRect colorRect = GetNode<ColorRect>("ColorRect");
        Vector2 centerPosition = colorRect.GlobalPosition + (colorRect.Size / 2.0f);
        return centerPosition;
    }
	public void RemoveCard()
    {
        if (_occupied)
        {
            _occupied = false;
            _currentCard = null;
            GD.Print("Card removed from slot.");
            Modulate = DefaultColor; // Reset color when card is placed
        }
    }

	private void _on_area_2d_area_entered(Area2D area) {
		if (area.GetParent() is Card card && !_occupied)
        {
            GetNode<Sprite2D>("Sprite2D").Modulate = HoverColor; // Change color on hover
			card.updateHoverStatus(true, this);
        }
	}
	
	private void _on_area_2d_area_exited(Area2D area) {
		if (area.GetParent() is Card card && !_occupied)
        {
            GetNode<Sprite2D>("Sprite2D").Modulate = DefaultColor;
			card.updateHoverStatus(false, this);
        }
	}
	public void MoveCardToSlot()
    {
		GD.Print(_currentCard.GlobalPosition);
		GD.Print(GlobalPosition);
        _currentCard.GlobalPosition = GlobalPosition + (Size - _currentCard.Size) / 2.0f;
        _occupied = true;
		GD.Print(_currentCard.GlobalPosition);
		GD.Print(GlobalPosition);
    }
	public Card GetCard() {
		return _currentCard;
	}
	public void TakeDamage() {
		if (_health > 0) {
			_health -= 1;
		}
	}
}

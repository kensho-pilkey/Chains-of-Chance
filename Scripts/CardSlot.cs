using Godot;
using System;

public partial class CardSlot : Control
{
    private bool _occupied = false;
    private Card _currentCard = null;
	private int _health;
    [Export] private Color HoverColor = new Color(1, 1, 1, 0.5f); // Light gray
    [Export] private Color DefaultColor = new Color(1, 1, 1, 1); // clear
	[Export] private Color PlacedColor = new Color(0.5f, 0.5f, 1.0f, 0.5f); //Light blue

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
            Modulate = PlacedColor; // Reset color when card is placed
        }
    }
	public void PlaceOpponentCard(CardStill card)
    {
        if (!_occupied)
        {
            _occupied = true;
            // _currentCard = card;
            Modulate = PlacedColor; // Reset color when card is placed
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
            Modulate = HoverColor; // Change color on hover
			card.updateHoverStatus(true, this);
        }
	}
	
	private void _on_area_2d_area_exited(Area2D area) {
		if (area.GetParent() is Card card && !_occupied)
        {
            Modulate = DefaultColor;
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

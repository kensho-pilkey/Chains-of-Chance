using Godot;
using System;

public partial class OpponentSlot : Control
{
	private bool _occupied = false;
	[Export] public PackedScene CardScene;
    private Card _currentCard = null;

	private ColorRect _colorRect;

	private int _health;
    [Export] private Color HoverColor = new Color(1, 1, 1, 0.5f); // Light gray
    [Export] private Color DefaultColor = new Color(1, 1, 1, 1); // clear
	[Export] private Color PlacedColor = new Color(0.5f, 0.5f, 1.0f, 1.0f);
	public override void _Ready()
	{
		_health = new RandomNumberGenerator().RandiRange(4, 5);
		
		_colorRect = GetNode<ColorRect>("ColorRect");
		_colorRect.Modulate = DefaultColor;
	}
    public override void _Process(double delta)
    {
        GetNode<Sprite2D>("Sprite2D").Frame = 5 - _health;
    }

    public Card SpawnCard() {
		if (_currentCard == null) {
			_currentCard = CardScene.Instantiate<Card>();
			// _currentCard.GlobalPosition = GlobalPosition;
			_currentCard._placed = true;
			AddChild(_currentCard);
            
            _currentCard.Position = (Size - _currentCard.Size) / 2.0f;
			_currentCard.Scale = _currentCard.Scale / 2;
			_occupied = true;
			_colorRect.Modulate = PlacedColor;
			return _currentCard;
		}
		else {
			return _currentCard;
		}
	}
	public Boolean IsOccupied(){
		return _occupied;
	}
	public Card GetCard() {
		return _currentCard;
	}
	public void RemoveCard()
    {
        if (_occupied)
        {
            _occupied = false;
            _currentCard = null;
            GD.Print("Card removed from slot.");
            _colorRect.Modulate = DefaultColor; // Reset color when card is placed
        }
    }
	public void TakeDamage() {
		if (_health > 0) {
			_health -= 1;
		}
	}
}

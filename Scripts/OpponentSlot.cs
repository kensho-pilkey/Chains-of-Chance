using Godot;
using System;

public partial class OpponentSlot : Control
{
	private bool _occupied = false;
	[Export] public PackedScene CardScene;
    private Card _currentCard = null;
    [Export] private Color HoverColor = new Color(1, 1, 1, 0.5f); // Light gray
    [Export] private Color DefaultColor = new Color(1, 1, 1, 1); // clear
	[Export] private Color PlacedColor = new Color(0.5f, 0.5f, 1.0f, 0.5f);
	public override void _Ready()
	{
		Modulate = DefaultColor;
	}

	public Card SpawnCard() {
		if (_currentCard == null) {
			_currentCard = CardScene.Instantiate<Card>();
			// _currentCard.GlobalPosition = GlobalPosition;
			AddChild(_currentCard);
            
            _currentCard.Position = (Size - _currentCard.Size) / 2.0f;
			_currentCard.Scale = _currentCard.Scale / 2;
			Modulate = PlacedColor;
			return _currentCard;
			
		}
		else {
			return _currentCard;
		}
	}
	public Boolean IsOccupied(){
		return _occupied;
	}
}

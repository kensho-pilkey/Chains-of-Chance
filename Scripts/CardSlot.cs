using Godot;
using System;

public partial class CardSlot : Control
{
    private bool _occupied = false;
    private Card _currentCard = null;
    [Export] private Color HoverColor = new Color(1, 1, 1, 0.5f); // Light gray
    [Export] private Color DefaultColor = new Color(1, 1, 1, 0); // clear
	[Export] private Color PlacedColor = new Color(0.5f, 0.5f, 1.0f, 0.0f); //Light blue


    public override void _Ready()
    {
        Modulate = DefaultColor;
    }

    public void PlaceCard(Card card)
    {
        if (!_occupied)
        {
            _occupied = true;
            _currentCard = card;
            GD.Print("Card placed in slot.");
            Modulate = PlacedColor; // Reset color when card is placed
        }
    }

	public void RemoveCard(Card card)
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
}

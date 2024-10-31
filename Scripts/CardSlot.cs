using Godot;
using System;

public partial class CardSlot : Control
{
    private bool _occupied = false;
    private Card _currentCard = null;
    [Export] private Color HoverColor = new Color(0.7f, 0.7f, 0.7f, 1.0f); // Light gray
    [Export] private Color DefaultColor = new Color(1, 1, 1, 1); // White

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

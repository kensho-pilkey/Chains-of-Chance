using Godot;
using System;

public partial class CardSlot : Control
{
    [Signal] public delegate void CardPlacedEventHandler(Card card);
    [Signal] public delegate void CardRemovedEventHandler(Card card);

    private bool _occupied = false;
    private Card _currentCard = null;

    // Define colors for hover and default
    [Export] private Color HoverColor = new Color(0.7f, 0.7f, 0.7f, 1.0f); // Light gray
    [Export] private Color DefaultColor = new Color(1, 1, 1, 1); // White

    public override void _Ready()
    {

        // Set default color initially
        Modulate = DefaultColor;
    }

    public void PlaceCard(Card card)
    {
        if (!_occupied)
        {
            _occupied = true;
            _currentCard = card;
            EmitSignal(nameof(CardPlaced), card);
            GD.Print("Card placed in slot.");
            Modulate = DefaultColor; // Reset color when card is placed
        }
    }

	private void _on_area_2d_area_entered(Area2D area) {
		if (area.GetParent() is Card && !_occupied)
        {
            GD.Print("Card hovering over slot.");
            Modulate = HoverColor; // Change color on hover
        }
	}
    private void OnBodyEntered(Node body)
	
    {
		GD.Print("Card hovering over slot.");
        if (body is Card && !_occupied)
        {
            GD.Print("Card hovering over slot.");
            Modulate = HoverColor; // Change color on hover
        }
    }

    private void OnBodyExited(Node body)
    {
        if (body is Card && !_occupied)
        {
            GD.Print("Card left slot.");
            Modulate = DefaultColor; // Revert color when card leaves
        }
    }
}

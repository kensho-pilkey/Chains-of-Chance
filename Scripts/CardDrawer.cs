using System;
using System.Collections.Generic;
using Godot;

public partial class CardDrawer : Control
{
    [Export] public PackedScene CardScene { get; set; } // Card scene to instantiate
    [Export] public float CardOffsetX { get; set; } = 20.0f; // Horizontal offset between cards
    [Export] public float RotMax { get; set; } = 10.0f; // Maximum rotation angle in degrees

    private List<Card> _hand = new List<Card>(); // List to keep track of cards in hand
    private Control _deckButton; // Reference to the deck button
    private bool _isDrawingCards = false; // Flag to prevent multiple draw calls

    public override void _Ready()
    {
        // Convert rotation from degrees to radians for calculations
        RotMax = Mathf.DegToRad(RotMax);

        // Get the deck button node
        _deckButton = GetParent().GetNode<Control>("UI").GetNode<Button>("OpenDeck");
    }

    public void DrawCards(int number)
    {
        if (_isDrawingCards)
            return; // Prevent starting multiple draw sequences

        _isDrawingCards = true;

        // Adjust the number of cards to draw based on available cards
        if (Global.Instance.CardCount() < number && Global.Instance.CardCount() > 0)
            number = Global.Instance.CardCount();

        if (Global.Instance.CardCount() >= number)
        {
            // Start drawing cards one by one
            DrawNextCard(number);
        }
        else
        {
            GD.Print("Not enough cards in the deck!");
            _isDrawingCards = false;
        }
    }

    private void DrawNextCard(int remainingCards)
    {
        if (CardScene != null && CardScene.Instantiate() is Card instance)
        {
            AddChild(instance);

            // Set the card's starting position to the deck button's position
            instance.GlobalPosition = _deckButton.GlobalPosition;

            // Initialize the card with data
            instance.AddData(Global.Instance.DrawUniqueCard());

            // Add the new card to the hand list
            _hand.Add(instance);

            // Reposition and animate the hand
            UpdateHandLayout();

            if (remainingCards > 1)
            {
                // Schedule the next card draw after 0.25 seconds
                var timer = GetTree().CreateTimer(0.25f);
                timer.Timeout += () => DrawNextCard(remainingCards - 1);
            }
            else
            {
                // All cards have been drawn
                _isDrawingCards = false;
            }
        }
        else
        {
            GD.PrintErr("CardScene is null or could not be instantiated as a Card.");
            _isDrawingCards = false;
        }
    }

    private void UpdateHandLayout()
    {
        int number = _hand.Count;

        for (int i = 0; i < number; i++)
        {
            Card card = _hand[i];

            // Calculate the final position for the card
            Vector2 finalPos = -(card.Size / 2.0f) - new Vector2(CardOffsetX * (number - 1 - i), 0);
            finalPos.X += (CardOffsetX * (number - 1)) / 2.0f;

            // Calculate the rotation angle for the card
            float rotRadians = (number == 1) ? 0 : Mathf.LerpAngle(-RotMax, RotMax, (float)i / (number - 1));

            // Create a tween for this card's movement and rotation
            var cardTween = CreateTween();
            cardTween.SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);

            // Animate the card to its new position and rotation
            cardTween.TweenProperty(card, "position", finalPos, 0.5f);

            card.SetHandPosition(finalPos);
        }
    }

    public void RemoveCard(Card card)
    {
        if (_hand.Contains(card))
        {
            _hand.Remove(card);

            // Reposition the hand after removing a card
            UpdateHandLayout();
        }
    }

    public void UndrawCards(Vector2 toPos)
    {
        if (_hand.Count == 0)
            return;

        foreach (Card card in _hand)
        {
            // Create a tween for moving the card back to the specified position
            var cardTween = CreateTween();
            cardTween.SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);

            // Animate the card back and reset rotation
            cardTween.TweenProperty(card, "global_position", toPos, 0.3f);
            cardTween.TweenProperty(card, "rotation", 0.0f, 0.3f);

            // Queue free the card after the tween completes
            cardTween.TweenCallback(Callable.From(() => card.QueueFree()));
        }

        // Clear the hand after all cards are undrawn
        _hand.Clear();
    }
}

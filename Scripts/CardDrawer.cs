using Godot;
using System;

public partial class CardDrawer : Control
{
    // Exported variables for easy configuration in the inspector
    [Export] public Control From { get; set; } // Starting position control
    [Export] public PackedScene CardScene { get; set; } // Scene to instantiate cards from
    [Export] public float CardOffsetX { get; set; } = 20.0f; // Horizontal offset for cards
    [Export] public float RotMax { get; set; } = 10.0f; // Max rotation in degrees, converted to radians
    [Export] public float AnimOffsetY { get; set; } = 0.3f; // Y-axis offset multiplier for animation
    [Export] public float TimeMultiplier { get; set; } = 2.0f; // Sine wave speed multiplier

    // Tween variables for handling animations
    private Tween _tween;
    private Tween _tweenAnimate;

    // Other variables
    private float _time = 0.0f; // Time tracker for sine animation
    private float _sineOffsetMult = 0.0f; // Offset multiplier for sine movement
    private bool _drawn = false; // Tracks whether cards are drawn or not

    // Public property for sine offset, so it can be tweened
    public float SineOffsetMult
    {
        get => _sineOffsetMult;
        set => _sineOffsetMult = value;
    }

    public override void _Ready()
    {
        SetProcess(false);

        // Convert the max rotation from degrees to radians
        RotMax = Mathf.DegToRad(RotMax);

        // Delay and call DrawCards after 2 seconds to demonstrate the animation
        //GetTree().CreateTimer(2.0f).Timeout += () => DrawCards(From.GlobalPosition, 10);
    }

    public override void _Process(double delta)
    {
        // Update time and animate each card's position with sine wave motion
        _time += (float)delta;
        for (int i = 0; i < GetChildCount(); i++)
        {
            if (GetChild(i) is Button card)
            {
                // Apply sine wave offset to each card's position on the Y-axis
                float val = Mathf.Sin(i + (_time * TimeMultiplier));
                card.Position += new Vector2(0, val * _sineOffsetMult);
            }
        }
    }

    public void DrawCards(Vector2 fromPos, int number)
{
    _drawn = true;

    // Adjust `number` based on available cards
    if (Global.Instance.CardCount() < number && Global.Instance.CardCount() > 0)
    {
        number = Global.Instance.CardCount();
    }

    // Proceed only if there are enough cards in the deck
    if (Global.Instance.CardCount() >= number)
    {
        if (_tween != null && _tween.IsRunning())
            _tween.Kill();

        _tween = CreateTween();
        _tween.SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);

        for (int i = 0; i < number; i++)
        {
            if (CardScene != null && CardScene.Instantiate() is Card instance)
            {
                AddChild(instance);
                instance.GlobalPosition = fromPos;
                instance.AddData(Global.Instance.DrawUniqueCard());

                // Handle positioning differently if only one card
                Vector2 finalPos;
                float rotRadians;
                
                if (number == 1)
                {
                    // Center position and zero rotation for a single card
                    finalPos = -instance.Size / 2.0f;
                    rotRadians = 0;
                }
                else
                {
                    // Standard calculation for multiple cards
                    finalPos = -(instance.Size / 2.0f) - new Vector2(CardOffsetX * (number - 1 - i), 0);
                    finalPos.X += (CardOffsetX * (number - 1)) / 2.0f;
                    rotRadians = Mathf.LerpAngle(-RotMax, RotMax, (float)i / (number - 1));
                }

                // Set the initial hand position and rotation
                instance.SetHandPosition(finalPos);

                // Animate the card to its final hand position and rotation
                _tween.Parallel().TweenProperty(instance, "position", finalPos, 0.3f + (i * 0.075f));
                _tween.Parallel().TweenProperty(instance, "rotation", rotRadians, 0.3f + (i * 0.075f));
            }
            else
            {
                GD.PrintErr("CardScene is null or could not be instantiated as a Button.");
            }
        }

        _tween.TweenCallback(Callable.From(() => SetProcess(true)));
        _tween.TweenProperty(this, "SineOffsetMult", AnimOffsetY, 1.5f).From(0.0f);
    }
    else
    {
        GD.Print("Not enough cards in the deck!");
    }
}

    public async void UndrawCards(Vector2 toPos)
    {
        _drawn = false;

        // Kill existing tween if running
        if (_tween != null && _tween.IsRunning())
            _tween.Kill();

        // Create new tween for undrawing cards
        _tween = CreateTween();
        _tween.SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);
        _tween.TweenProperty(this, "SineOffsetMult", 0.0f, 0.9f);

        int childCount = GetChildCount();

        for (int i = childCount - 1; i >= 0; i--)
        {
            if (GetChild(i) is Button c)
            {
                // Animate cards back to the specified position and reset rotation
                _tween.Parallel().TweenProperty(c, "global_position", toPos, 0.3f + ((childCount - i) * 0.075f));
                _tween.Parallel().TweenProperty(c, "rotation", 0.0f, 0.3f + ((childCount - i) * 0.075f));
            }
        }

        // Wait for tween to finish before removing cards
        await ToSignal(_tween, "finished");

        foreach (Node child in GetChildren())
        {
            child.QueueFree();
        }
    }

    public void AnimateCards()
    {
        if (_tweenAnimate != null && _tweenAnimate.IsRunning())
            _tweenAnimate.Kill();

        // Create a looping tween for continuous animation (optional)
        _tweenAnimate = CreateTween();
        _tweenAnimate.SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);
        _tweenAnimate.SetLoops();
    }

    private void _OnDrawButtonPressed()
    {
        // Toggle between drawing and undrawing cards
        if (_drawn)
            UndrawCards(From.GlobalPosition);
        else
            DrawCards(From.GlobalPosition, 10);
    }
}

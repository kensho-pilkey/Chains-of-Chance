using Godot;
using System;

public partial class Card : Button
{
    // Effect Variables
    [Export] public float AngleXMax = 15.0f;
    [Export] public float AngleYMax = 15.0f;
    [Export] public float MaxOffsetShadow = 50.0f;

    [ExportCategory("Oscillator")]
    [Export] public float Spring = 150.0f;
    [Export] public float Damp = 10.0f;
    [Export] public float VelocityMultiplier = 2.0f;

    private float _displacement = 0.0f;
    private float _oscillatorVelocity = 0.0f;

    private Tween _tweenRot;
    private Tween _tweenHover;
    private Tween _tweenDestroy;
    private Tween _tweenHandle;
    private Vector2 _lastPos;
    private Vector2 _velocity;
    private TextureRect _cardTexture;
    private Control _shadow;
    private CollisionShape2D _collisionShape;

    private CardSlot _hoveredSlot;
    private bool _isEnlarged = false; // New: tracks if card is enlarged

    private CardData _cardData = null;

    public override void _Ready()
    {
        AngleXMax = Mathf.DegToRad(AngleXMax);
        AngleYMax = Mathf.DegToRad(AngleYMax);
        _collisionShape = GetNode<CollisionShape2D>("DestroyArea/CollisionShape2D");
        _collisionShape.SetDeferred("disabled", false);

        _cardTexture = GetNode<TextureRect>("CardTexture");
        _shadow = GetNode<Control>("Shadow");

        // Connect signals for hover and click events
        MouseEntered += _onMouseEntered;
        MouseExited += _onMouseExited;
        Pressed += _onPressed;

        _cardData = Global.Instance.getRandomCard();
        GD.Print(_cardData.ToString());
        UpdateCardAppearance();
    }

    public override void _Process(double delta)
    {
        RotateVelocity((float)delta);
        HandleShadow((float)delta);
    }

    private void UpdateCardAppearance()
    {
        _cardTexture.Texture = GD.Load<Texture2D>(_cardData.AssetPath);
    }

    private void RotateVelocity(float delta)
    {
        var centerPos = GlobalPosition - (Size / 2.0f);
        _velocity = (Position - _lastPos) / delta;
        _lastPos = Position;
        
        _oscillatorVelocity += _velocity.Normalized().X * VelocityMultiplier;
        float force = -Spring * _displacement - Damp * _oscillatorVelocity;
        _oscillatorVelocity += force * delta;
        _displacement += _oscillatorVelocity * delta;

        Rotation = _displacement;
    }

    private void HandleShadow(float delta)
    {
        var center = GetViewportRect().Size / 2.0f;
        float distance = GlobalPosition.X - center.X;

        _shadow.Position = new Vector2(
            Mathf.Lerp(0.0f, -Mathf.Sign(distance) * MaxOffsetShadow, Mathf.Abs(distance / center.X)),
            _shadow.Position.Y
        );
    }

    private void _onPressed()
    {
        // Toggle enlarge/shrink effect on click
        _isEnlarged = !_isEnlarged;
        
        if (_tweenHandle != null && _tweenHandle.IsRunning())
            _tweenHandle.Kill();

        _tweenHandle = CreateTween().SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);
        
        // Toggle between original and enlarged size
        if (_isEnlarged)
        {
            _tweenHandle.TweenProperty(this, "scale", new Vector2(2.5f, 2.5f), 0.3f);
        }
        else
        {
            _tweenHandle.TweenProperty(this, "scale", Vector2.One, 0.3f);
        }
    }

    private void _onMouseEntered()
    {
        if (_isEnlarged) return; // Skip hover effect if the card is enlarged

        if (_tweenHover != null && _tweenHover.IsRunning())
            _tweenHover.Kill();

        _tweenHover = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Elastic);
        _tweenHover.TweenProperty(this, "scale", new Vector2(2.2f, 2.2f), 0.5f);
    }

    private void _onMouseExited()
    {
        if (_isEnlarged) return; // Skip reset if the card is enlarged

        if (_tweenHover != null && _tweenHover.IsRunning())
            _tweenHover.Kill();

        _tweenHover = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Elastic);
        _tweenHover.TweenProperty(this, "scale", Vector2.One, 0.55f);
    }

    public void Destroy()
    {
        _cardTexture.UseParentMaterial = true;
        if (_tweenDestroy != null && _tweenDestroy.IsRunning())
            _tweenDestroy.Kill();
        
        _tweenDestroy = CreateTween().SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);
        _tweenDestroy.TweenProperty(_cardTexture.Material, "shader_parameter/dissolve_value", 0.0f, 2.0f).From(1.0f);
        _tweenDestroy.Parallel().TweenProperty(_shadow, "self_modulate:a", 0.0f, 1.0f);
    }
}

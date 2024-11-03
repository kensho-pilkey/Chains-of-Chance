using Godot;
using System;

public partial class Card : Button
{
	//Effect Vars
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
	private Label _label;
	private GpuParticles2D _burnParticles;
	private AnimationPlayer _animationPlayer;
    private CardSlot _hoveredSlot;
 	private bool _followingMouse = false;
	public bool _placed = false;

	public CardData _cardData = null;

	private Vector2 _handPosition;

	private Boolean opponentCard;
	public int _Health { get; set; }

	public int _Damage;



    public override void _Ready()
{
    AngleXMax = Mathf.DegToRad(AngleXMax);
    AngleYMax = Mathf.DegToRad(AngleYMax);
    _collisionShape = GetNode<CollisionShape2D>("DestroyArea/CollisionShape2D");
    _collisionShape.SetDeferred("disabled", false);

    _cardTexture = GetNode<TextureRect>("CardTexture");
    _shadow = GetNode<Control>("Shadow");
	//ADD card health dmg etc
	_label = GetNode<Label>("Label");
	_burnParticles = GetNode<GpuParticles2D>("BurnParticles");
	_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    // Connect the signals to the methods
    MouseEntered += _onMouseEntered;
    MouseExited += _onMouseExited;

	_cardData = Global.Instance.getRandomCard();
	//GD.Print(_cardData.ToString());
	UpdateCardAppearance();
	if(GetParent() is OpponentSlot) {
		opponentCard = true;
	}
	else {
		opponentCard = false;
	}
}

    public override void _Process(double delta)
    {
        RotateVelocity((float)delta);
        FollowMouse((float)delta);
        HandleShadow((float)delta);
    }

	private void UpdateCardAppearance()
	{
		//_cardTexture.Texture = GD.Load<Texture2D>(_cardData.AssetPath);
		_label.Text = _cardData.Name + "\n" + _cardData.Damage + "\n" + _cardData.Health;
		_Health = _cardData.Health;
		_Damage = _cardData.Damage;
		if(_cardData.ElementType == "Fire") {
			_cardTexture.Texture = GD.Load<Texture2D>("res://Assets/fire_card_sprite.png");
		}
		else if(_cardData.ElementType == "Water") {
			_cardTexture.Texture = GD.Load<Texture2D>("res://Assets/water_card_sprite.png");
		}
		else if(_cardData.ElementType == "Grass") {
			_cardTexture.Texture = GD.Load<Texture2D>("res://Assets/grass_card_sprite.png");
		}
	}
	public void AddData(CardData data) {
		_cardData = data;
		UpdateCardAppearance();
	}
    private void RotateVelocity(float delta)
    {
        if (!_followingMouse) return;

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
	//TODO make shadow actually follow card
    {
        var center = GetViewportRect().Size / 2.0f;
        float distance = GlobalPosition.X - center.X;

        _shadow.Position = new Vector2(
            Mathf.Lerp(0.0f, -Mathf.Sign(distance) * MaxOffsetShadow, Mathf.Abs(distance / center.X)),
            _shadow.Position.Y
        );
    }

    private void FollowMouse(float delta)
    {
        if (!_followingMouse) return;
        var mousePos = GetGlobalMousePosition();
        GlobalPosition = mousePos - (Size / 2.0f);
    }
    public void PlaceCardInSlot(CardSlot slot) {
		GlobalPosition = slot.GetCenterPosition() - (Size / 2.0f);
		slot.PlaceCard(this);
		_hoveredSlot = null;
		_placed = true;
    }

	public void PlaceOpponentCardInSlot(CardSlot slot) {
		// Scale = Vector2.One;
		// Position = Position = slot.Position + (slot.Size - Size) / 2.0f;
		slot.PlaceCard(this);
        _hoveredSlot = null;
		_placed = true;
		Scale = Scale / 2;
	}
 	public void Destroy()
	{
		_animationPlayer.Play("fade");

		// Start emitting particles if you want
		_burnParticles.Emitting = true;

		// Queue the card for removal after the animation finishes
		//_animationPlayer.Connect("animation_finished", OnFadeComplete);
		GetTree().CreateTimer(1.0f).Timeout += () => {
			_burnParticles.Emitting = false; // Stop emitting particles
			QueueFree(); // Remove the card from the scene
		};
	}

	// Method to handle after the animation finishes

    public override void _GuiInput(InputEvent @event)
	{
		if (!_placed) {
			//if LMB is clicked, held, or released
			if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Left)
			{
				_followingMouse = mouseEvent.IsPressed();

				if (!_followingMouse)
				{
					if (_hoveredSlot != null)
					{
						PlaceCardInSlot(_hoveredSlot);
					}
					else
					{
						// Not over a slot, return to hand
						ReturnToHand();
					}

					if (_tweenHandle != null && _tweenHandle.IsRunning())
						_tweenHandle.Kill();

					_tweenHandle = CreateTween().SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);
					_tweenHandle.TweenProperty(this, "rotation", 0.0f, 0.3f);
				}
			}
		}
	}
	public void updateHoverStatus(bool hovering, CardSlot slot) {
		if (hovering) {
			_hoveredSlot = slot;
		}
		else if (!hovering && _hoveredSlot == slot) {
			_hoveredSlot = null;
		}
	}
    private void _onMouseEntered()
    {
		if (!opponentCard) {
			if (_tweenHover != null && _tweenHover.IsRunning())
            	_tweenHover.Kill();

			_tweenHover = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Elastic);
			_tweenHover.TweenProperty(this, "scale", new Vector2(2.2f, 2.2f), 0.5f);
		}
    }

    private void _onMouseExited()
    {
		if (!opponentCard) {
			if (_tweenRot != null && _tweenRot.IsRunning())
				_tweenRot.Kill();

			_tweenRot = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Back).SetParallel(true);

			// Reset shader rotation values to 0
			if (_cardTexture.Material is ShaderMaterial shaderMaterial)
			{
				_tweenRot.TweenProperty(shaderMaterial, "shader_parameter/x_rot", 0.0f, 0.5f);
				_tweenRot.TweenProperty(shaderMaterial, "shader_parameter/y_rot", 0.0f, 0.5f);
			}

			// Reset shadow rotation to 0
			_tweenRot.TweenProperty(_shadow, "rotation", 0.0f, 0.5f);

			if (_tweenHover != null && _tweenHover.IsRunning())
				_tweenHover.Kill();

			_tweenHover = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Elastic);
			_tweenHover.TweenProperty(this, "scale", new Vector2(2f, 2f), 0.55f);
		}
	}

	public void SetHandPosition(Vector2 position)
	{
		_handPosition = position;
	}

	private void ReturnToHand()
	{
		Position = _handPosition;
	}
	public void TakeDamage(int damage) {
		_Health -= damage;
	}
	public void Attack(Card card) {
		var finalDmg = _Damage;
		if (!opponentCard) {
			finalDmg *= Global.Instance.Multiplier;
		} 

		// Load and instantiate the popup
		var popupScene = (PackedScene)ResourceLoader.Load("res://Scenes/damage_popup.tscn");
		var popup = (DamagePopup)popupScene.Instantiate();

		// Get the PopupLayer (Node2D) and convert the position
		var popupLayer = GetTree().Root.GetNode<Node2D>("gameScene/PopupLayer");
		popup.Position = popupLayer.ToLocal(card.GlobalPosition);  // Convert to local coordinates of PopupLayer
		popup.SetDamage(finalDmg);

		// Add the popup to the PopupLayer
		popupLayer.AddChild(popup);

		// Apply damage to the target card
		card.TakeDamage(finalDmg);
	}

	public void SetCardData(CardData data)
	{
    _cardData = data; // Store reference if needed
    UpdateCardAppearance(); // Call to update the visuals
	}

}

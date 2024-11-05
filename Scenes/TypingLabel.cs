using Godot;
using System;

public partial class TypingLabel : Label
{
    [Export] public float TypingSpeed = 0.05f; // Delay between each character
    private string _fullText = ""; // The full text to display
    private int _charIndex = 0; // Index to track the current character
    private Timer _typingTimer; // Reference to the typing timer
    private Timer _disappearTimer; // Reference to the disappear timer

    public override void _Ready()
    {
        Text = ""; // Start with an empty label

        // Initialize the disappear timer
        _disappearTimer = new Timer();
        AddChild(_disappearTimer);
        _disappearTimer.WaitTime = 1.0f; // 1-second delay
        _disappearTimer.OneShot = true;
        _disappearTimer.Connect("timeout", new Callable(this, nameof(OnDisappearTimeout)));
    }

    public void StartTyping(string textToDisplay)
    {
        // Initialize the text and character index
        GetParent<TextureRect>().Visible = true;
        _fullText = textToDisplay;
        _charIndex = 0;
        Text = ""; // Clear any existing text

        // Create the typing timer and start it
        _typingTimer = new Timer();
        AddChild(_typingTimer);
        _typingTimer.WaitTime = TypingSpeed;
        _typingTimer.OneShot = false;
        _typingTimer.Connect("timeout", new Callable(this, nameof(OnTypingTimeout)));
        _typingTimer.Start();
    }

    private void OnTypingTimeout()
    {
        // Display the next character
        if (_charIndex < _fullText.Length)
        {
            Text += _fullText[_charIndex];
            _charIndex++;
        }
        else
        {
            // Stop the typing timer once all characters are displayed
            _typingTimer.Stop();
            _typingTimer.QueueFree();
            _typingTimer = null;

            // Start the disappear timer after text has fully appeared
            _disappearTimer.Start();
        }
    }

    private void OnDisappearTimeout()
    {
        // Clear the label and hide the parent
        Text = "";
        GetParent<TextureRect>().Visible = false;
    }
}

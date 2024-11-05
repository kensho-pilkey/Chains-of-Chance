using Godot;
using System;

public partial class P2Slots : HBoxContainer
{
    public override void _Ready()
    {
        // Create and configure the Timer
        Timer delayTimer = new Timer();
        AddChild(delayTimer);
        delayTimer.WaitTime = 2.0f; // Set delay to 2 seconds
        delayTimer.OneShot = true; // Run only once
        delayTimer.Connect("timeout", new Callable(this, nameof(OnDelayTimeout)));
        delayTimer.Start();
    }

    private void OnDelayTimeout()
    {
        // Randomly select one of the OpponentSlots
        Random randomGenerator = new Random();
        int num = randomGenerator.Next(0, 5);
        int num2 = randomGenerator.Next(0, 10);
        // Get the TypingLabel and start typing the first phrase
        GetChild<OpponentSlot>(num).GetNode<TextureRect>("SpeechBubble").GetNode<TypingLabel>("Label").StartTyping(Global.Instance.Phrases[num2]);
    }

}

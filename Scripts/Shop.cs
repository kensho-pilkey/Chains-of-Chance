using Godot;
using System;

public partial class Shop : Control
{
	[Export] public PackedScene CardScene; // Packed scene for instantiating cards
    private HBoxContainer _hBoxContainer1;
    private HBoxContainer _hBoxContainer2;
    private Button _buyButton;
    private CardStill _selectedCard;
	public override void _Ready()
	{
		_hBoxContainer1 = GetNode<HBoxContainer>("CreatureShop");
        _hBoxContainer2 = GetNode<HBoxContainer>("SpecialShop");
        _buyButton = GetNode<Button>("Buy");
		PopulateContainer(_hBoxContainer1, 3);
		PopulateContainer(_hBoxContainer2, 1);
	}


    private void PopulateContainer(HBoxContainer container, int num)
    {
        for (int i = 0; i < num; i++) // Assuming each container holds 3 cards
        {
            if (CardScene.Instantiate() is CardStill card)
            {
                container.AddChild(card);
            }
        }
    }

    public void OnCardSelected(CardStill card)
    {
		if (_selectedCard != null) {
			_selectedCard.UnSelect();
		}
        _selectedCard = card;
		_selectedCard.Select();
    }

    private void OnBuyButtonPressed()
    {
        if (_selectedCard != null)
        {
            // Implement buying logic here, e.g., adding to player inventory
            GD.Print("Bought Card: " + _selectedCard.Name);
            _selectedCard.QueueFree(); // Remove the bought card from the shop
            _selectedCard = null; // Reset selection after buying
        }
        else
        {
            GD.Print("No card selected to buy.");
        }
    }
	private void _on_exit_pressed() {
		GetParent<Control>().Visible = true;
		Visible = false;
	}
}

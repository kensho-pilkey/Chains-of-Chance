using Godot;
using System;

public partial class Shop : Control
{
	[Export] public PackedScene CardScene;
    [Export] public PackedScene SpecialScene;
    private HBoxContainer _hBoxContainer1;
    private HBoxContainer _hBoxContainer2;
    private Button _buyButton;
    private CardStill _selectedCard;
    private SpecialCard _selectedSpecial;
	public override void _Ready()
	{
		_hBoxContainer1 = GetNode<HBoxContainer>("CreatureShop");
        _hBoxContainer2 = GetNode<HBoxContainer>("SpecialShop");
        _buyButton = GetNode<Button>("Buy");
		ResetShop();
        
		
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
    private void PopulateSpecial(HBoxContainer container, int num) {
        for (int i = 0; i < num; i++) // Assuming each container holds 3 cards
        {
            if (SpecialScene.Instantiate() is SpecialCard card)
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
        if (_selectedSpecial != null) {
			_selectedSpecial.UnSelect();
            _selectedSpecial = null;
		}
        _selectedCard = card;
		_selectedCard.Select();
        _buyButton.Disabled = false;
    }
    public void OnSpecialSelected(SpecialCard card)
    {
		if (_selectedSpecial != null) {
			_selectedSpecial.UnSelect();
		}
        if (_selectedCard != null) {
			_selectedCard.UnSelect();
            _selectedCard = null;
		}
        _selectedSpecial = card;
		_selectedSpecial.Select();
        _buyButton.Disabled = false;
    }
	public void ResetShop() {
		PopulateContainer(_hBoxContainer1, 3);
		if (Global.Instance.LevelNum > 4) {
            PopulateSpecial(_hBoxContainer2, 5);
        }
        else {
            PopulateSpecial(_hBoxContainer2, Global.Instance.LevelNum);
        }
	}

    private void OnBuyButtonPressed()
    {
        _buyButton.Disabled = true;
        if (_selectedCard != null)
        {
            // Implement buying logic here, e.g., adding to player inventory
            GD.Print(Global.Instance.Money);
            if (Global.Instance.Money >= _selectedCard.cost) {
                Global.Instance.Money -= _selectedCard.cost;
                Global.Instance.AddCard(_selectedCard._cardData);
                _selectedCard.QueueFree(); // Remove the bought card from the shop
                _selectedCard = null; // Reset selection after buying
            }
            else {
                GD.Print("Not enough money to buy card.");
            }
        }
        else if (_selectedSpecial != null)
        { 
             if (Global.Instance.Money >= _selectedSpecial.cost) {
                Global.Instance.Money -= _selectedSpecial.cost;
                _selectedSpecial.UseSpecial();
                _selectedSpecial.QueueFree(); // Remove the bought card from the shop
                _selectedSpecial = null; // Reset selection after buying
             }
             else {
                GD.Print("Not enough money to buy card.");
                _buyButton.Disabled = false;
            }
        }
        else {
            GD.Print("No card selected to buy.");
        }
    }
}

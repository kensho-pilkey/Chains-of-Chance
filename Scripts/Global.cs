using Godot;
using System;
using System.Collections.Generic;

public partial class Global : Node
{
	public int PlayerScore { get; set; } = 0;
	public List<CardData> PlayerCards { get; private set; } = new List<CardData>();
	private static readonly Dictionary<string, string> NameAssetPairs = new Dictionary<string, string>
    {
        { "Dragon", "res://assets/dragon.png" },
        { "Phoenix", "res://assets/phoenix.png" },
        { "Griffin", "res://assets/griffin.png" },
        { "Unicorn", "res://assets/unicorn.png" },
        { "Hydra", "res://assets/hydra.png" }
    };
	private static readonly string[] Elements = { "Fire", "Water", "Grass" };
	private static readonly Random RandomGenerator = new Random();
	public override void _Ready()
	{
		PlayerCards.Add(createRandomCard());
		PlayerCards.Add(createRandomCard());
		PlayerCards.Add(createRandomCard());
		
		foreach (var card in PlayerCards)
			{
				GD.Print(card.ToString());
			}
		
	}
	public CardData createRandomCard()
    {
        // Select a random name-asset pair
        var nameAsset = GetRandomNameAsset();

        // Randomly select element type
        string element = Elements[RandomGenerator.Next(Elements.Length)];

        // Randomly assign damage (1-8) and health (3-10)
        int damage = RandomGenerator.Next(1, 9); // Inclusive of 1-8
        int health = RandomGenerator.Next(3, 11); // Inclusive of 3-10

        return new CardData(nameAsset.Key, nameAsset.Value, damage, health, element);
    }

	private KeyValuePair<string, string> GetRandomNameAsset()
    {
        // Convert dictionary keys to a list to access randomly
        List<string> keys = new List<string>(NameAssetPairs.Keys);
        string randomKey = keys[RandomGenerator.Next(keys.Count)];

        return new KeyValuePair<string, string>(randomKey, NameAssetPairs[randomKey]);
    }


	public override void _Process(double delta)
	{
	}
}

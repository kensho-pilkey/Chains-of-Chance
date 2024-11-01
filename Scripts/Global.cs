using Godot;
using System;
using System.Collections.Generic;

public partial class Global : Node
{
	public int PlayerScore { get; set; } = 0;
	public List<CardData> PlayerCards { get; private set; } = new List<CardData>();
	public static Global Instance { get; private set; }
	private static readonly Dictionary<string, string> NameAssetPairs = new Dictionary<string, string>
    {
        { "Dragon", "res://assets/Tiles_A_white.png" },
        { "Phoenix", "res://assets/Tiles_A_white.png" },
        { "Griffin", "res://assets/Tiles_A_white.png" },
        { "Unicorn", "res://assets/Tiles_A_white.png" },
        { "Hydra", "res://assets/Tiles_A_white.png" }
    };
	private static readonly string[] Elements = { "Fire", "Water", "Grass" };
	private static readonly Random RandomGenerator = new Random();
	public override void _Ready()
	{
		// Initialize the singleton instance (allows us to ref this object and its data from anywhere)
		Instance = this;
		PlayerCards.Add(createRandomCard());
		PlayerCards.Add(createRandomCard());
		PlayerCards.Add(createRandomCard());
		PlayerCards.Add(createRandomCard());
		PlayerCards.Add(createRandomCard());
		PlayerCards.Add(createRandomCard());
		
	}
	public CardData getRandomCard() {
		return PlayerCards[RandomGenerator.Next(PlayerCards.Count)];
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

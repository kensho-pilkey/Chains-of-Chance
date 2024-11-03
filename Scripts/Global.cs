using Godot;
using System;
using System.Collections.Generic;

public partial class Global : Node
{
	public int PlayerScore { get; set; } = 0;
	public int PlayerHealth { get; set; } = 100;
	public int OpponentHealth { get; set; } = 100;
	public List<CardData> PlayerCards { get; private set; } = new List<CardData>();
	private List<CardData> availableCardsForTurn = new List<CardData>();
	public static Global Instance { get; private set; }

	public int LevelNum { get; set; } = 1;
	public int Money { get; set; } = 10;
	public int Multiplier { get; set; } = 1;
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
		PlayerCards.Add(createRandomCard());
		PlayerCards.Add(createRandomCard());
		PlayerCards.Add(createRandomCard());
		PlayerCards.Add(createRandomCard());
		PlayerCards.Add(createRandomCard());
		PlayerCards.Add(createRandomCard());
		ShuffleCards();
		
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
	public void ShuffleCards()
	{
		int n = PlayerCards.Count;
		for (int i = n - 1; i > 0; i--)
		{
			int j = RandomGenerator.Next(i + 1);
			// Swap elements at i and j
			CardData temp = PlayerCards[i];
			PlayerCards[i] = PlayerCards[j];
			PlayerCards[j] = temp;
		}
	}
	public void StartTurn()
	{
		// Reset available cards at the start of each turn
		availableCardsForTurn = new List<CardData>(PlayerCards);
		ShuffleCards();
	}
	public void StartNextTurn() {
		availableCardsForTurn = new List<CardData>(PlayerCards);
		ShuffleCards();
		GetTree().ChangeSceneToFile("res://Scenes/game_scene.tscn");
		LevelNum ++;
		GD.Print("Level: " + LevelNum);
		// PlayerHealth = 100;
		OpponentHealth = 100;
		Multiplier = 1;
		Money += 10;
	}

	public CardData DrawUniqueCard()
	{
		if (availableCardsForTurn.Count == 0)
		{
			GD.Print("No more unique cards available for this turn!");
			return null;
		}
		
		int index = RandomGenerator.Next(availableCardsForTurn.Count);
		CardData drawnCard = availableCardsForTurn[index];
		availableCardsForTurn.RemoveAt(index); // Remove the drawn card from the pool
		
		return drawnCard;
	}

	public int CardCount() {
		return availableCardsForTurn.Count;
	}

	public override void _Process(double delta)
	{
	}
}

using System;

public class CardData
{
    public string Name { get; set; }
    public string AssetPath { get; set; } // Path to the texture or image file
    public int Damage { get; set; }
    public int Health { get; set; }
    public string ElementType { get; set; }

    // Constructor to initialize the card data
    public CardData(string name, string assetPath, int damage, int health, string elementType)
    {
        Name = name;
        AssetPath = assetPath;
        Damage = damage;
        Health = health;
        ElementType = elementType;
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health < 0) Health = 0;
    }

    public bool IsAlive()
    {
        return Health > 0;
    }

    public override string ToString()
    {
        return $"{Name} (Element: {ElementType}, Damage: {Damage}, Health: {Health})";
    }
}
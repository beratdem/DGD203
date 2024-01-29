using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Location
{
    public string Name { get; private set; }
    public Vector2 Coordinates { get; private set; }
    public LocationType Type { get; private set; }
    public List<Item> ItemsOnLocation { get; private set; }

    public Location(string locationName, LocationType type, Vector2 coordinates, List<Item> itemsOnLocation)
    {
        Name = locationName;
        Type = type;
        Coordinates = coordinates;
        ItemsOnLocation = itemsOnLocation;
    }

    public Location(string locationName, LocationType type, Vector2 coordinates)
    {
        Name = locationName;
        Type = type;
        Coordinates = coordinates;
        ItemsOnLocation = new List<Item>();
    }

    public void RemoveItem(Item item)
    {
        ItemsOnLocation.Remove(item);
    }
}

public enum LocationType
{
    Village,
    Npc,
    MysteriousTemple
}

public class Player
{
    public void TakeItem(Item item)
    {
    
    }
}

public enum Item
{
    compass,
    book
}

public struct Vector2
{
    public int X { get; set; }
    public int Y { get; set; }

    public Vector2(int x, int y)
    {
        X = x;
        Y = y;
    }
}
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory System/Items/Generic")]

public class Item : ScriptableObject
{
    public float cost;
    public string Name;
    public Sprite ImageUI;
    public bool IsConsumable;
    public bool IsStackable;
}
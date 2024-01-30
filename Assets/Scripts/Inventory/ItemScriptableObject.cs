using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {
    Default, Food, Weapon, Instrument
}

public class ItemScriptableObject : ScriptableObject {
    public ItemType itemType;
    public GameObject itemPrefab;
    public string itemName;
    public Sprite icon;
    public int maximumAmount;
    public string itemDescription;
    public bool isConsumeable;

    [Header("Consumable Characteristics")]
    public float changeHealth;
    public float changeHunger;
    public float changeThirst;

}
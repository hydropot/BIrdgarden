using UnityEngine;

[CreateAssetMenu(fileName = "New Bird", menuName = "GameObjects/Bird")]
public class Bird : ScriptableObject
{
    //shop item properties
    public string Name = "Default";
    [TextArea]
    public string Description = "Description";
    //public int Level;
    //public int Price;
    public CurrencyType Currency;
    public BirdType Type;
    public GameObject Prefab;
    //public bool isPlaced;
    public bool isLocked;
}

//types of objects - also tabs
public enum BirdType
{
    Type1,
    Type2,
    Type3,
    Type4,
    Type15
}

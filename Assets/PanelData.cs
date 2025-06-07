using UnityEngine;

[CreateAssetMenu(fileName = "ItemPanelData", menuName = "Item/PanelData", order = 1)]
public class ItemPanelData : ScriptableObject
{
    public string panelName;
    public ItemData[] items;
}

[System.Serializable]
public class ItemData
{
    public string itemName;
    public Sprite itemImage;
}
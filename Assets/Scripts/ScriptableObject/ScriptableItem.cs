
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/Item")]
public class ScriptableItem : ScriptableObject
{
    public ItemName itemName;
    public RuntimeAnimatorController itemAnimator;

    public float itemDuration;

    public Sprite itemSprite;
}

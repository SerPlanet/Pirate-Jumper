using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharachter", menuName = "Charachter/Charachter")]
public class ScriptableCharachter : ScriptableObject
{
   public CharachterName charachterName;
   public RuntimeAnimatorController animatorController;

   public Rarity rarity;

   public Sprite icon;
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Bag", menuName = "Item/Bag")]
public class Bag : ScriptableObject
{
    public List<Kotodama> itemList = new List<Kotodama>();

    public void Init()
    {
       Debug.Log($"Bag {name} を初期化しました");
    }
}

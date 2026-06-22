using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "Kotodama", menuName = "Item/Kotodama")]
public class Kotodama : ScriptableObject
{
    public string number;

    [TextArea]public string text;

    public float MaxPower;

    public float CommonAlert;

    public string[] KyoKotoba;

    public float KyoAlert;
}

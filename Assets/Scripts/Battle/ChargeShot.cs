using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeShot : MonoBehaviour
{
    [SerializeField] BattleSystem battleSystem;
    // Start is called before the first frame update
    public void OnClick()
    {
        battleSystem.CallShot();
    }
}

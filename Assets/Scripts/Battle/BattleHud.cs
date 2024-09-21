using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField]  Text nameText;
    [SerializeField]  HPBar hpBar;
    [SerializeField] ENBar enBar;

    Unit _unit;
    public void SetData(Unit unit){
        _unit=unit;
        nameText.text=unit.Base.Name;
        hpBar.SetHP((float)unit.HP/unit.MaxHP);
    }

    public IEnumerator UpdateHP(){
        yield return hpBar.SetHPSmooth((float)_unit.HP/_unit.MaxHP);
        
    }
    public IEnumerator UPdateEN(){
        yield return enBar.SetENSmooth((float)_unit.NowEN/_unit.EN);
      

    }

}




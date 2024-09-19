using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    // HPの増減をする
    [SerializeField] GameObject hpBar;

    
    public void SetHP(float hp){
        hpBar.transform.localScale=new Vector3(hp,1,1);
    }

    public IEnumerator SetHPSmooth(float newHP){
        float currentHP=hpBar.transform.localScale.x;
        float changeAmount = currentHP-newHP;
        while(currentHP - newHP>Mathf.Epsilon){
            currentHP-=changeAmount*Time.deltaTime;
            hpBar.transform.localScale=new Vector3(currentHP,1,1);
            yield return null;
        }
        hpBar.transform.localScale=new Vector3(newHP,1,1);
    }
}

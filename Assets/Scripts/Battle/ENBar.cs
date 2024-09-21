using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ENBar : MonoBehaviour
{  
    // ENの増減をする
    [SerializeField] GameObject enBar;

    
    public void SetEN(float en){
        enBar.transform.localScale=new Vector3(en,1,1);

}
 public IEnumerator SetENSmooth(float newEN){
        float currentEN=enBar.transform.localScale.x;
        float changeAmount = currentEN-newEN;
        while(currentEN - newEN>Mathf.Epsilon){
            currentEN-=changeAmount*Time.deltaTime;
            enBar.transform.localScale=new Vector3(currentEN,1,1);
            yield return null;
        }
        enBar.transform.localScale=new Vector3(newEN,1,1);
    }
}

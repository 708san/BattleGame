using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BattleStartPanel : MonoBehaviour
{
   //rotationXを0>90>0
   private void Start(){
    transform.rotation=Quaternion.Euler(90,0,0);

    StartCoroutine(PanelAnim());
   }
   
   IEnumerator PanelAnim(){
    yield return null;
   
    yield return transform.DORotate(new Vector3(0,0,0),0.5f).WaitForCompletion();
    yield return new WaitForSeconds(0.5f);
    transform.DORotate(new Vector3(90,0,0),0.5f);
   }
}

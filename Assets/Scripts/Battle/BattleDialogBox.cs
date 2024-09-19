using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//dialogのTextを取得
public class BattleDialogBox : MonoBehaviour
{

    [SerializeField] Text dialogText;
    [SerializeField] int letterPerSecond;
    //変更するための関数
    public void SetDialog(string dialog){
        dialogText.text=dialog;
    }

    //コルーチンで一文字ずつ
    public IEnumerator TypeDialog(string dialog){
        dialogText.text="";
        foreach (char letter in dialog){
            dialogText.text+=letter;
            yield return new WaitForSeconds(1f/letterPerSecond);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StartButton : MonoBehaviour {

    [SerializeField] BattleSystem battleSystem;

    // ボタンが押された場合、今回呼び出される関数
    public void OnClick()
    {
        battleSystem.StartTurn();  // ログを出力
        
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//ユニットのマスターデータ外部から変更不可(インスペクタからのアクセス可)
//スクリプタブルオブジェクト
[CreateAssetMenu]
public class UnitBase : ScriptableObject{
    [SerializeField] new string name;
    [TextArea]
    [SerializeField] string description;
    //画像
    [SerializeField] Sprite allySprite;
    [SerializeField] Sprite enemySprite;

    //ステータス Hp Atk Def sAtk sDef Spd
    [SerializeField] int maxHP;  
    [SerializeField] int en; 
    [SerializeField] int atk;
    [SerializeField] int def;
    [SerializeField] int spAtk;
    [SerializeField] int spDef;
    [SerializeField] int spd;

    [SerializeField] List<MoveOfUnit> movesOfUnit;
    public List<MoveOfUnit> MovesOfUnit{
        get => movesOfUnit;
    }
    
//プロパティ化で参照でき変更できない
    public string Name{
        get=>name;
    }
    public string Description{
        get=>description;
    }
    public int MaxHP{
        get => maxHP;
    }

    public int EN{
        get => en;
    }

    public int Atk{
        get => atk;
    }
    public int Def{
        get => def;
    }
    public int SpAtk{
        get => spAtk;
    }
    public int SpDef{
        get =>  spDef;
    }
     public int Spd{
        get => spd;
    }

    public Sprite  AllySprite{
        get=>allySprite;
    }
    public Sprite  EnemySprite{
        get=>enemySprite;
    }
    

}

//技一覧

[Serializable]
public class MoveOfUnit{
    //ヒエラルキで設定
    [SerializeField] MoveBase _base;
    public MoveBase Base{get =>_base;}
}

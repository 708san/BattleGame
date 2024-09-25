using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//キャラクターのデータクラス、データのみ扱う
public class Unit//monobehavior使わない
{
    //ベースデータ
    public UnitBase Base{get;set;}
    int Level;

    // @hayato
    int TakeEN;
   

    public int HP{get;set;}

    public int NowEN{get;set;}

    //技リスト
    public List<Move> Moves{get;set;}
    // コンストラクタ
    public Unit(UnitBase unitBase, int level){
        Base=unitBase;
        Level=level;
        HP=MaxHP;
        NowEN=EN;
        Moves=new List<Move>();
        foreach(MoveOfUnit moveOfUnit in unitBase.MovesOfUnit){
            Moves.Add(new Move(moveOfUnit.Base));
        }
    }

    public Move GetRandomMove(){
        int i = UnityEngine.Random.Range(1, Moves.Count);
        if (Moves[i].Base.Cost<=NowEN){
            NowEN-=Moves[i].Base.Cost;
            return Moves[i];
        }
        else{
            return Moves[0];
        }
        
    }
    
    //レベルによるステータスを返す:プロパティ
    public int Atk{
        get{
            return Mathf.FloorToInt((Base.Atk*Level)/100f)+5;
        }
    }

    public int  Def{
        get{
            return Mathf.FloorToInt((Base.Def*Level)/100f)+5;
        }
    }
    public int SpAtk{
        get{
            return Mathf.FloorToInt((Base.SpAtk*Level)/100f)+5;
        }
    }

    public int  SpDef{
        get{
            return Mathf.FloorToInt((Base.SpDef*Level)/100f)+5;
        }
    }

    public int  Spd{
        get{
            return Mathf.FloorToInt((Base.Spd*Level)/100f)+5;
        }
    }
    public int  EN{
        get{
            return Mathf.FloorToInt((Base.EN*Level)/100f)+5;
        }
    }
    public int  MaxHP{
        get{
            return Mathf.FloorToInt((Base.MaxHP*Level)/100f)+10;
        }
    }
    
    //@hayato buffのフラグを立てる
    
    public bool TakeDamage(Move move, Unit attacker,bool Buffrag, bool debuffrag){
        //揺らぎ
        float modifiers = UnityEngine.Random.Range(0.85f,1f);
        float d=0;
        if(move.Base.SkillType==MoveBase.CandidateSkillType.Physical){
            float a =(2*attacker.Level+10)/250f;
            d=a*move.Base.Power*((float)attacker.Atk/Def)+2;
        }
        else if(move.Base.SkillType==MoveBase.CandidateSkillType.Energy){
            float a =(2*attacker.Level+10)/250f;
            d=a*move.Base.Power*((float)attacker.SpAtk/SpDef)+2;
        }
        //@hayato
        else if(move.Base.SkillType==MoveBase.CandidateSkillType.Buff){
            return false;
        }
        else if(move.Base.SkillType==MoveBase.CandidateSkillType.Debuff){
            return false;
        }
        
        
        int damage =Mathf.FloorToInt(d*modifiers);
        //@hayato ダメージ計算
        
        if(Buffrag == true && debuffrag == false){
            
            damage = Mathf.FloorToInt(d*1.2f*modifiers);
            Debug.Log("バフ攻撃");         
        }
        else if(Buffrag == false && debuffrag == true){
            damage = Mathf.FloorToInt(d*0.8f*modifiers);
            Debug.Log("デバフ攻撃");
        }
        
        
        HP-=damage;
        
        if(HP<=0){
            HP=0;
            return true;
        }
        else {
            return false;
        }


    }
    public void Charge(){
        float e=0;
        e=EN*0.4f;
        Debug.Log($"e={e}");
        if((NowEN+Mathf.FloorToInt(e))<EN){
            NowEN+=Mathf.FloorToInt(e);
        }
        else{
            NowEN=EN;
        }
        
    }

    public void Repair(){
        Debug.Log("Now reparing..");
        float e=0;
        e=MaxHP*0.4f;
        Debug.Log($"e={e}");
        if((HP+Mathf.FloorToInt(e))<MaxHP){
            HP+=Mathf.FloorToInt(e);
        }
        else{
            HP=MaxHP;
        }
    }
    // @hayato
    //現在のENゲージ取得+ゼロにする
    public void ZeroEn(){
        TakeEN = NowEN;
        NowEN = 0;
    }
    // @hayato 
    //チャージショットのダメージ計算（物理）

    public bool SuperDamage(Unit attacker){
        float modifiers = UnityEngine.Random.Range(0.85f,1f);
        float d = 0;
        float a = (2*attacker.Level+10)/250f;
        d = a*20*((float)attacker.Atk/Def)+2;
        int damage = Mathf.FloorToInt(d*modifiers);

        HP-=damage+TakeEN;
        
        if(HP<=0){
            HP=0;
            return true;
        }
        else {
            return false;
        }


    }
}


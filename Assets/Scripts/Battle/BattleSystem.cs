using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class BattleSystem : MonoBehaviour
{
    public bool finished=false;
    int playerDone;
    int enemyDone;
    int pIndex=0;
    int eIndex=0;
    //@hayato バフ・デバフの管理フラグ
    bool Player_buf = false;
    bool Enemy_buff = false;
    bool Player_debuff = false;
    bool Enemy_debuff = false;
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHud playerHud;
    [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] GameObject startButton; 
    [SerializeField] GameObject chargeButton; 
    [SerializeField] GameObject  repairButton; 
    //@hayato
    [SerializeField] GameObject ShotButton;
    
    [SerializeField] List<UnitBase> playerBaseList;
    [SerializeField] List<UnitBase> enemyBaseList;
    private void Start(){
        playerUnit.SetUp(playerBaseList[pIndex]);
        enemyUnit.SetUp(enemyBaseList[eIndex]);
        playerHud.SetData(playerUnit.Unit);
        enemyHud.SetData(enemyUnit.Unit);
        pIndex+=1;
        eIndex+=1;
        StartCoroutine(dialogBox.TypeDialog("バトル開始!"));

    }

    

    public void StartTurn(){
        playerDone=0;
        enemyDone=0;
        startButton.SetActive(false);
        chargeButton.SetActive(false);
        repairButton.SetActive(false);
        ShotButton.SetActive(false);
        if(playerUnit.Unit.Spd>=enemyUnit.Unit.Spd){
            StartCoroutine(StartPlayerMove());
        }
        else{
            StartCoroutine(StartEnemyMove());
        }
        
    }

    public void CallRepair(){
         StartCoroutine(Repair_());
    }

    public void CallCharge(){
        StartCoroutine(Charge_());
    }
    //@hayato CallChargeShot
    public void CallShot()
    {
        StartCoroutine(Shot_());
    }
    // @hayato

    IEnumerator Shot_(){
        playerDone=1;
        //enemyDone=0;
        startButton.SetActive(false);
        chargeButton.SetActive(false);
        repairButton.SetActive(false);
        ShotButton.SetActive(false);

        yield return dialogBox.TypeDialog("チャージショット");
        yield return new WaitForSeconds(1);

        playerUnit.Unit.ZeroEn();
        playerUnit.AttackAnimation();
        yield return new WaitForSeconds(1);
        enemyUnit.HitAnimation();
        bool isFainted =enemyUnit.Unit.SuperDamage(playerUnit.Unit);
        yield return enemyHud.UpdateHP();
        //ENゲージをゼロにする
        
        //ENゲージ反映
        yield return playerHud.UPdateEN();

        
        //戦闘不能ならメッセージ
        if(isFainted){
            yield return dialogBox.TypeDialog($"{enemyUnit.Unit.Base.Name}は戦闘不能");
            if(eIndex==enemyBaseList.Count){
                finished=true;
            }
            enemyUnit.DeadAnimation();
            yield return new WaitForSeconds(0.5f);
            if(eIndex<enemyBaseList.Count){
                enemyUnit.SetUp(enemyBaseList[eIndex]);
                enemyHud.SetData(enemyUnit.Unit);
                eIndex+=1;
                TurnStart();
            }

        }
        else if(enemyDone==0){
            StartCoroutine(StartEnemyMove());
        }
        else{
            TurnStart();
        }



    }
    
    IEnumerator Charge_(){
        playerDone=1;
        enemyDone=0;
        startButton.SetActive(false);
        chargeButton.SetActive(false);
        repairButton.SetActive(false);
        ShotButton.SetActive(false);
        
        yield return dialogBox.TypeDialog($"{playerUnit.Unit.Base.Name}はチャージした");
        yield return new WaitForSeconds(1);
        
        playerUnit.Unit.Charge();
        yield return playerHud.UPdateEN();
        yield return new WaitForSeconds(1);
        Debug.Log($"HP:{playerUnit.Unit.NowEN}");
        StartCoroutine(StartEnemyMove());
    }

    IEnumerator Repair_(){
        playerDone=1;
        enemyDone=0;
        startButton.SetActive(false);
        chargeButton.SetActive(false);
        repairButton.SetActive(false);
        ShotButton.SetActive(false);
        yield return dialogBox.TypeDialog($"{playerUnit.Unit.Base.Name}は自己修復した");
        yield return new WaitForSeconds(1);
        Debug.Log($"HP:{playerUnit.Unit.HP}");
        Debug.Log("Now preparing..");
        playerUnit.Unit.Repair();
        yield return playerHud.UpdateHP();
        yield return new WaitForSeconds(1);
        Debug.Log($"HP:{playerUnit.Unit.HP}");
        StartCoroutine(StartEnemyMove());
    }

    IEnumerator StartPlayerMove(){
        playerDone=1;
        Move playerMove=playerUnit.Unit.GetRandomMove();
        yield return dialogBox.TypeDialog($"{playerUnit.Unit.Base.Name}は{playerMove.Base.Name}を使った");
        yield return playerHud.UPdateEN();

        //@hayato buffの起動を追加
        if(playerMove.Base.SkillType==MoveBase.CandidateSkillType.Buff)
        {
            Player_buf = true;
            yield return new WaitForSeconds(1);
        }
        else if(playerMove.Base.SkillType==MoveBase.CandidateSkillType.Debuff)
        {
            Enemy_debuff = true;
        }
        else{
           playerUnit.AttackAnimation();
           yield return new WaitForSeconds(1);
           enemyUnit.HitAnimation(); 
        }
        
        
        bool isFainted =enemyUnit.Unit.TakeDamage(playerMove,playerUnit.Unit, Player_buf, Player_debuff);
        //HP反映
        yield return enemyHud.UpdateHP();
        //戦闘不能ならメッセージ
        if(isFainted){
            yield return dialogBox.TypeDialog($"{enemyUnit.Unit.Base.Name}は戦闘不能");
            //buffフラグを下ろす
            Enemy_buff = false;
            Enemy_debuff = false;
            Debug.Log("バフをリセットした");
            if(eIndex==enemyBaseList.Count){
                finished=true;
            }
            enemyUnit.DeadAnimation();
            yield return new WaitForSeconds(0.5f);
            if(eIndex<enemyBaseList.Count){
                enemyUnit.SetUp(enemyBaseList[eIndex]);
                enemyHud.SetData(enemyUnit.Unit);
                eIndex+=1;
                TurnStart();
            }

        }
        else if(enemyDone==0){
            StartCoroutine(StartEnemyMove());
        }
        else{
            TurnStart();
        }

    }
    IEnumerator StartEnemyMove(){
        enemyDone=1;
        Move enemyMove=enemyUnit.Unit.GetRandomMove();
        yield return dialogBox.TypeDialog($"{enemyUnit.Unit.Base.Name}は{enemyMove.Base.Name}を使った");
        yield return enemyHud.UPdateEN();

        //@hayato buffの起動
        if(enemyMove.Base.SkillType==MoveBase.CandidateSkillType.Buff)
        {
            Enemy_buff = true;
        }
        if(enemyMove.Base.SkillType==MoveBase.CandidateSkillType.Debuff)
        {
            Player_debuff = true;
        }

        enemyUnit.AttackAnimation();
        yield return new WaitForSeconds(1);
        playerUnit.HitAnimation();
        bool isFainted =playerUnit.Unit.TakeDamage(enemyMove,enemyUnit.Unit,Enemy_buff,Enemy_debuff);
        yield return playerHud.UpdateHP();
        if(isFainted){
            yield return dialogBox.TypeDialog($"{playerUnit.Unit.Base.Name}は戦闘不能");
            //buffフラグを下ろす
            Player_buf = false;
            Player_debuff = false;
            Debug.Log("バフをリセットした");
            if(pIndex==playerBaseList.Count){
                finished=true;
            }
            playerUnit.DeadAnimation();
            yield return new WaitForSeconds(0.5f);
            if(pIndex<playerBaseList.Count){
                playerUnit.SetUp(playerBaseList[pIndex]);
                playerHud.SetData(playerUnit.Unit);
                pIndex+=1;
                TurnStart();
            }

        }
         else if(playerDone==0){
            StartCoroutine(StartPlayerMove());
        }
        else{
            TurnStart();
        }
    }

    public void TurnStart(){
        StartCoroutine(dialogBox.TypeDialog("ターンを開始してください"));
        startButton.SetActive(true);
        chargeButton.SetActive(true);
        repairButton.SetActive(true);
        //@hayato
        ShotButton.SetActive(true);
        if(playerUnit.Unit.NowEN == 0){
            ShotButton.SetActive(false);
        }
    }


    

}

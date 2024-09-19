using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] bool isPlayerUnit;
    [SerializeField] BattleSystem battleSystem;
    int level;
    //生成したユニットのプロパティ化
    public Unit Unit{get;set;}

    Vector3 originalPos;
    Color originalColor;
    Image image;
    public Image UnitImage{
        get=>image;
    }
    //バトルで使うユニットの保持
    //画像の変更

    public void Awake(){
        originalPos=transform.localPosition;
        image = GetComponent<Image>();
        originalColor=image.color;
    }
    public void SetUp(UnitBase _base){
        level=50;
        Unit = new Unit(_base,level);
        
        if (isPlayerUnit){
            image.sprite= Unit.Base.AllySprite;
        }
        else{
            image.sprite= Unit.Base.EnemySprite;
        }
        PlayerEnterAnimation();
    }

    //登場 Anim
    public void PlayerEnterAnimation(){
        if(isPlayerUnit){
            transform.localPosition=new Vector3(-1200,originalPos.y);
        }
        else{
            transform.localPosition=new Vector3(1200,originalPos.y);
        }
        transform.DOLocalMoveX(originalPos.x,1f);
    }

    //攻撃 Anim
    public void AttackAnimation(){
        //シーケンス
        Sequence sequence =DOTween.Sequence();
        //動いた後元の位置
        if(isPlayerUnit){
            sequence.Append(transform.DOLocalMoveX(originalPos.x+50f,0.25f));//後ろに追加
            sequence.Append(transform.DOLocalMoveX(originalPos.x,0.25f));//後ろに追加
        }
        else{
            sequence.Append(transform.DOLocalMoveX(originalPos.x-50f,0.25f));//後ろに追加
            sequence.Append(transform.DOLocalMoveX(originalPos.x,0.25f));//後ろに追加
        }           
    }

    public void HitAnimation(){
        //シーケンス
        Sequence sequence =DOTween.Sequence();
        //色かえ
        sequence.Append(image.DOColor(Color.gray,0.1f));
        sequence.Append(image.DOColor(originalColor,0.1f));
    }

    public void DeadAnimation(){
        //下に下がりながらフェイント
        Sequence sequence = DOTween.Sequence();
        //appendは続けてjoinは同時に
        sequence.Append(transform.DOLocalMoveY(originalPos.y-150f,0.5f));
        sequence.Join(image.DOFade(0,0.5f));
        sequence.Append(transform.DOLocalMoveY(originalPos.y,0)); 
        if(battleSystem.finished==false){
            sequence.Append(image.DOFade(1, 0)); 
        }    
        
        
    }
}

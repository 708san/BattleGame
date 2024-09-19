using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MoveBase : ScriptableObject
{
    public enum CandidateSkillType{
        Physical,
        Energy,
        Buff,
        Debuff
    }

    //技のマスターデータ
    //名前、説明、威力、命中、コスト
    [SerializeField] new string name;
    [TextArea]
    [SerializeField] string description;
    [SerializeField] int power;
    [SerializeField] int accuracy;
    [SerializeField] int cost;
    [SerializeField] CandidateSkillType skillType;
    public string Name{
        get=>name;
    }
    public string Description{
        get=>description;
    }
    public int Power{
        get=>power;
    }
    public int Accuracy{
        get=>accuracy;
    }
    public int Cost{
        get=>cost;
    }

    public CandidateSkillType SkillType{
        get=>skillType;
    }
    
}

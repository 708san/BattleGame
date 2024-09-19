using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    //実際に使う上での技
    //unitが参照するのでpublicにする
    
    public MoveBase Base{
        get;set;
    }

    public Move(MoveBase moveBase){
        Base=moveBase;
    }
}


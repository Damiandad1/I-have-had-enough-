using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateEnum : MonoBehaviour
{
    public enum State
    {
        Patrol = 1,
        Spot = 2,
        Attack = 3,
        Run = 4,
        Death = 5,
        Triumph = 6
    }

    public enum BossState
    {
        FirstState = 1,
        SecondState = 2,
        ThirdState = 3,
        FourthState = 4
    }

    public enum BossWhatDo
    {
     Attack = 1,
     Death = 2

    }
}

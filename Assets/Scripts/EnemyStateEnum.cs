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
}

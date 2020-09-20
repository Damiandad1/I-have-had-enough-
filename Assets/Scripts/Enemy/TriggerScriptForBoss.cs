using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScriptForBoss : MonoBehaviour
{
    [SerializeField] private GameObject _enemyRb;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _enemyRb.gameObject.GetComponent<EnemyBoss>().enabled = true;
    }
}

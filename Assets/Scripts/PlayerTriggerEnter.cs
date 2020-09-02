using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerEnter : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _enemyRb;


    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.gameObject.GetComponent<Movement>())
        {
            EnemyBoss.playerArrived = true;
            _enemyRb.gameObject.GetComponent<EnemyBoss>().enabled = true;
        }
    }
}

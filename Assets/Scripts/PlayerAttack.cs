using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float timeBtwMeleeAttack;
    public float startTimeBtwMeleeAttack;

    public Transform attackPos;
    public float attackRange;
    public LayerMask whatIsEnemies;
    public int damage;


    public float attackRangeX;
    public float attackRangeY;
    private void Update()
    {
        if (timeBtwMeleeAttack <= 0)
        {
            // then u can attack
            if (Input.GetKey(KeyCode.Z))
            {
                Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(attackPos.position, new Vector2(attackRangeX, attackRangeY), 0, whatIsEnemies);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    enemiesToDamage[i].GetComponent<Enemy>().TakeMeleeDamage(damage);
                }
            }
            timeBtwMeleeAttack = startTimeBtwMeleeAttack;
        }
        else
        {
            timeBtwMeleeAttack -= Time.deltaTime;
        }

        
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPos.position, new Vector3(attackRangeX, attackRangeY, 1));
    }
}

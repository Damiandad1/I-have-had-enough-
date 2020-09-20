using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballSkill : MonoBehaviour
{
    private float timeBtwFireballAttack;
    public float startTimeBtwFireballAttack;

    public Transform attackPos;
    public float attackRange;
    public LayerMask whatIsEnemies;
    private int closeDamage = 6;
    private int mediumDamage = 4;
    private int farDamage = 2;
    private int closeRange = 2;
    private int mediumRange = 3;
    private int farRange = 5;

    public float distance;

   

    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _enemy;

    public float attackRangeX;
    public float attackRangeY;
    public Animator animator;

    private void Update()
    {
        if (Enemy._isAlive)
        {
            distance = Vector2.Distance(_player.transform.position, _enemy.transform.position);
        }
        
      //  Debug.Log(distance);

        if (timeBtwFireballAttack <= 0)
        {
            // then u can attack
            if (Input.GetKey(KeyCode.J))
            {
                Debug.Log("flame");
                animator.SetBool("IsFlameSpellActive", true);
                Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(attackPos.position, new Vector2(attackRangeX, attackRangeY), 0, whatIsEnemies);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {

                    if (distance <= closeRange)
                    {
                        enemiesToDamage[i].GetComponent<Enemy>().TakeMeleeDamage(closeDamage);
                    }
                    else if (distance >= closeRange && distance <= farRange)
                    {
                        enemiesToDamage[i].GetComponent<Enemy>().TakeMeleeDamage(mediumDamage);
                    }
                    else if (distance >= farRange)

                    {
                        enemiesToDamage[i].GetComponent<Enemy>().TakeMeleeDamage(farDamage);
                    }
                }
                    
                  Collider2D[] bossToDamage = Physics2D.OverlapBoxAll(attackPos.position, new Vector2(attackRangeX, attackRangeY), 0, whatIsEnemies);
                  for (int i = 0; i < bossToDamage.Length; i++)
                    {

                        if (distance <= closeRange)
                        {
                            bossToDamage[i].GetComponent<EnemyBoss>().TakeBossMeleeDamage(closeDamage);
                        }
                        else if (distance >= closeRange && distance <= farRange)
                        {
                            bossToDamage[i].GetComponent<EnemyBoss>().TakeBossMeleeDamage(mediumDamage);
                        }
                        else if (distance >= farRange)

                        {
                            bossToDamage[i].GetComponent<EnemyBoss>().TakeBossMeleeDamage(farDamage);
                        }


                    }
                animator.SetBool("IsFlameSpellActive", false);
            }
            timeBtwFireballAttack = startTimeBtwFireballAttack;
        }
        else
        {
            timeBtwFireballAttack -= Time.deltaTime;
        }


    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(attackPos.position, new Vector3(attackRangeX, attackRangeY, 1));
    }
}

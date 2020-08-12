using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    private EnemyStateEnum.BossState _bossState;
    private EnemyStateEnum.BossWhatDo _bossWhatDo;
    public int bossHealth = 15;
    public float speed = 5;

    [SerializeField] private GameObject _enemy;

    [SerializeField] private Rigidbody2D _player;
    [SerializeField] private Rigidbody2D _enemyPrefab;
    public static bool _isAlive = true;
    private bool _spawned = false;

    private int _enemyAmount = 0;

    private float timeBtwSpawnNext = 2f;
    private float countdownTime;

    private bool canDash = true;
    private float stoppingDistance = 10f;
    [SerializeField] private Transform _playerPrefab;

    private Vector2 _playerLastPosition;
    private bool _storedPlayerPosition = false;
    private bool isBlockActive = false;
    private bool canBlock = true;

    private float nextAttackTime;

    public GameObject hitEffect;

    private void Awake()
    {
        StorePlayerPosition();
        _bossState = EnemyStateEnum.BossState.SecondState;
        countdownTime = timeBtwSpawnNext;
    }

    private void Update()
    {
        CurrentBossStatus();

        if (bossHealth <= 0)
        {
            _bossState = EnemyStateEnum.BossState.FourthState;
        }
    }

    private void CurrentBossStatus()
    {
        switch (_bossState)
        {
            case EnemyStateEnum.BossState.FirstState: // Debug.Log("FirstState");
                SpawnEnemies();
                // spawning 15 enemies and wait for their death
                break;

            case EnemyStateEnum.BossState.SecondState: //Debug.Log("SecondState");
                // using first and second spell, if boss hp is less than 30% turn to third state and do it for 30 seconds then turn to first state
                if (canDash && !isBlockActive)
                {
                    BossDash();
                    Debug.Log("dashing");
                }
                else if (canBlock && !canDash && !isBlockActive)
                {

                    TotalBlock();

                    Debug.Log("Blocking");
                }
                break;

            case EnemyStateEnum.BossState.ThirdState: // Debug.Log("ThirdState");
                // using first and third spell if 0 hp turn to fourth state

                break;

            case EnemyStateEnum.BossState.FourthState: //Debug.Log("Death");
                // animation of death
                Destroy(this.gameObject);
                break;
        }

        switch (_bossWhatDo)
        {
            case EnemyStateEnum.BossWhatDo.Attack:
                break;

            case EnemyStateEnum.BossWhatDo.Death:
                break;
        }
    }
    private void SpawnEnemies()
    {


        //        if (countdownTime == 0)
        //{
        //    for (_enemyAmount = 0; _enemyAmount < 14; _enemyAmount++)
        //    {
        //        Instantiate(_enemyPrefab, new Vector2(-40, 0.75f), Quaternion.identity);
        //        _enemyAmount = 0;
        //        // _spawned = true;



        //    }
        //}
        //        else
        //        {
        //            countdownTime -= Time.timeScale;
        //        }
        //    //  WaitForSpawn();
        //    countdownTime = timeBtwSpawnNext;

        if (!_spawned )  // and less than 15, in method ++
        {
            StartCoroutine(WaitForSpawn());
        }
    }
        

    private IEnumerator WaitForSpawn()
    {
        _spawned = true;
        yield return new WaitForSeconds(1);
       
        _spawned = false;

    }

    // coroutine that have random range and do skills between first and second, then between third and first

    private void ShieldMove()
    {
        // once sec wait - untouchable, one move toward our player then attack on 3x length - 2 sec cd

    }

    private void BossDash()
    {
        canBlock = false;
        if (!_storedPlayerPosition)
        {
            StorePlayerPosition();
            _storedPlayerPosition = true;
        }
        // wait 2 secs - animation, then dash toward last player position on 8x his length - 3x faster, deal 5 dmg if it collide with player
       
        if (Vector2.Distance(transform.position, _playerLastPosition) > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(_playerPrefab.position.x, transform.position.y), speed * Time.deltaTime);
           
        }
        else
        {
            // deal dmg
            Attack();
        }
        StartCoroutine(MakingEnemyDash());

    }

    private void TotalBlock()
    {
        // untouchable for 5 secs and look on player, if in 5 secs boss got dmg then cancel this animation, else after 5 secs deal 6 dmg in two sides, 5 sec cd
       StartCoroutine(BlockTime());
    }

    private IEnumerator MakingEnemyDash()
    {
        canDash = false;
        speed = 10;
     
        yield return new WaitForSeconds(1);
        speed = 5;
        canBlock = true;
        yield return new WaitForSeconds(5);
        canDash = true;
    }
    private IEnumerator BlockTime()
    {
        isBlockActive = true;
        _enemyPrefab.gameObject.layer = LayerMask.NameToLayer("Untouchable");
        yield return new WaitForSeconds(5);
        _enemyPrefab.gameObject.layer = LayerMask.NameToLayer("Enemy");
        yield return new WaitForSeconds(10);
        isBlockActive = false;
    }

    private void StorePlayerPosition()
    {
        _playerLastPosition = _playerPrefab.transform.position;
    }

    private void Attack()
    {
        if (Vector2.Distance(_playerPrefab.position, _enemy.transform.position) < 2f && _playerPrefab.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (Time.time > nextAttackTime)
            {
                Health.myHealth--;
                float fireRate = 1f;
                nextAttackTime = Time.time + fireRate;
            }
        }


    }

    public void TakeBossMeleeDamage(int damage)
    {
        // play a hurt sound 
        Instantiate(hitEffect, transform.position, Quaternion.identity);
        bossHealth -= damage;
        Debug.Log("damage TAKEN !" + damage);


        //dazedTime = startDazedTime;
    }
}

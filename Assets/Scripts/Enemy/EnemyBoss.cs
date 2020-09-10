// wchodzi gracz, jest od razu drugi etap 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _playerOnScene;
    [SerializeField] private Transform _playerReference;
    [SerializeField] private GameObject _enemy;
    [SerializeField] private Rigidbody2D _enemyPrefab;
    [SerializeField] private Transform _playerPrefab;
    [SerializeField] private Transform[] moveSpots;

    public int bossHealth = 15;
    public float speed = 5;
    public float normalSpeed = 5;
    public static int _enemyAmount = 0;
    public EnemyStateEnum.BossState _bossState;
    public static bool _isAlive = true;
    public Transform attackPos;
    public float attackRange;
    public GameObject hitEffect;
    public float attackRangeX;
    public float attackRangeY;
    public LayerMask player;
    public Vector2 posEnem;
    public int damage = 2;
    public static bool playerArrived;

    private bool _firstStateIsActive;
    private Animator _animator;
    private bool _canBeSecondState;
    private EnemyStateEnum.BossWhatDo _bossWhatDo; 
    private bool _spawned = false;
    private float _timeBtwSpawnNext = 2f;
    private float _countdownTime;
    private bool canDash = true;   
    private float _stoppingDistance = 3f; 
    private int _randomSpot;
    private Vector2 _playerLastPosition;   
    private bool _storedPlayerPosition = false;   
    private bool _isBlockActive = false;
    private bool _canBlock = true;
    private float _nextAttackTime;
    private bool _bossIsDead; 
    private bool _isFacingLeft;

    private int skillSwitch;

    private void Awake()
    {
        _bossIsDead = false;
        StorePlayerPosition();
        _bossState = EnemyStateEnum.BossState.SecondState;
        _countdownTime = _timeBtwSpawnNext;
    }

    private void Update()
    {

            FlipCharacter();

           


            if (bossHealth <= 0)
            {
                Destroy(this.gameObject);
                _bossIsDead = true;
            }

            if (_bossIsDead == false)
            {
                ChangeStates();

                CurrentBossStatus();
            }

        

    }
    private void FlipCharacter()
    {
        Vector3 localScale = transform.localScale;
        posEnem = (_playerPrefab.transform.position - _enemy.transform.position).normalized;
        //Debug.Log(posEnem);
        if (posEnem.x > 0 && !_isFacingLeft)
        {
            _isFacingLeft = true;
            //_enemy.transform.localScale.x = -8;
            localScale.x *= -1;
        }
        else if (posEnem.x < 0 && _isFacingLeft)
        {
            _isFacingLeft = false;
            localScale.x *= -1;
        }
        transform.localScale = localScale;
    }
    private void CurrentBossStatus()
    {
        switch (_bossState)
        {
            case EnemyStateEnum.BossState.FirstState:
                SpawnEnemies();
                
                    if (_canBeSecondState)
                    {
                        _bossState = EnemyStateEnum.BossState.SecondState;
                    }
                    else
                    {
                        _bossState = EnemyStateEnum.BossState.ThirdState;
                    }
                
             
                // spawning 15 enemies and wait for their death
                break;

            case EnemyStateEnum.BossState.SecondState: //Debug.Log("SecondState");
                StartCoroutine(ChangeToFirstStateAfterSometime());
                // using first and second spell, if boss hp is less than 30% turn to third state and do it for 30 seconds then turn to first state
                if (canDash && !_isBlockActive && !_canBlock)
                {
                    _animator.SetBool("canRush", true);
                    BossDash();
                    _animator.SetBool("canRush", false);
                    Debug.Log("dashing");
                    _canBlock = true;
                }
                else if (_canBlock && !canDash && !_isBlockActive)
                {

                    TotalBlock();
                    //animator.SetBool("canRush", false);
                    Debug.Log("Blocking");
                }
                break;

            case EnemyStateEnum.BossState.ThirdState: // Debug.Log("ThirdState");
                                                      // using first and third spell if 0 hp turn to fourth state
                StartCoroutine(ChangeToFirstStateAfterSometime());
                if (canDash && !_isBlockActive)
                {
                    _animator.SetBool("canRush", true);
                    BossDash();
                    _animator.SetBool("canRush", false);
                    Debug.Log("Dash");
                }
                else if (_canBlock && !canDash && !_isBlockActive)
                {

                    ShieldMove();

                    Debug.Log("ShieldMove");
                }
                break;

            case EnemyStateEnum.BossState.FourthState: //Debug.Log("Death");
                // animation of death
                Destroy(this.gameObject);
                break;
        }

        
    }
    private void AnotherSwitch()
    {
        switch (skillSwitch)
        {
            case 1:
                break;
        }
    }
    private void SpawnEnemies()
    {

        if (!_spawned)  // and less than 15, in method ++
        {
            StartCoroutine(WaitForSpawn());
        }
    }

    
    private IEnumerator WaitForSpawn()
    {
        _spawned = true;
        _enemyAmount++;
        if (_enemyAmount <15)
        {
            
            //Instantiate(_enemyPrefab, new Vector2(-40, 0.75f), Quaternion.identity);
            MoveToPlayer enemyInstance = Instantiate(_enemyPrefab.GetComponent<MoveToPlayer>());
            enemyInstance._playerPrefab = _playerOnScene;

            Debug.Log(_enemyAmount);
        }
     
        yield return new WaitForSeconds(1);

        _spawned = false;

    }

    // coroutine that have random range and do skills between first and second, then between third and first

    private void ShieldMove()
    {
        // one sec wait - untouchable, one move toward our player then attack on 3x length - 2 sec cd
        StartCoroutine(BlockTime());
     
    }

    private void BossDash()
    {
        _canBlock = false;
     
        // wait 2 secs - animation, then dash toward last player position on 8x his length - 3x faster, deal 5 dmg if it collide with player


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
        // animation for 2 secs then doing dash
        // transform.position = Vector2.MoveTowards(transform.position, _playerLastPosition, speed * Time.deltaTime);

        Collider2D[] hitByDash = Physics2D.OverlapBoxAll(attackPos.position, new Vector2(attackRangeX, attackRangeY), 0, player);
        for (int i = 0; i < hitByDash.Length; i++)
        {
            hitByDash[i].GetComponent<Health>().TookDamageFromEnemy(damage);

        }
        if (!_storedPlayerPosition)
        {
            StorePlayerPosition();
            _storedPlayerPosition = true;
        }
        speed = 20;
        _enemyPrefab.AddForce(transform.forward * speed, ForceMode2D.Impulse);

        //if (Vector2.Distance(_enemyPrefab.position, _playerLastPosition) < 0)
        //{

        //}

        yield return new WaitForSeconds(2);
        speed = 5;
        _canBlock = true;

        yield return new WaitForSeconds(5); // cooldown
        canDash = true;

    }
    private IEnumerator BlockTime()
    {
        _isBlockActive = true;
        canDash = false;
        _enemyPrefab.gameObject.layer = LayerMask.NameToLayer("Untouchable");
        yield return new WaitForSeconds(5);
        _enemyPrefab.gameObject.layer = LayerMask.NameToLayer("Enemy");
        yield return new WaitForSeconds(10);
        _isBlockActive = false;
        if (!_storedPlayerPosition)
        {
            StorePlayerPosition();
            _storedPlayerPosition = true;
        }
        yield return new WaitForSeconds(2);
        canDash = true;
    }

    private IEnumerator ChangeToFirstStateAfterSometime()
    {
        yield return new WaitForSeconds(30);
        _bossState = EnemyStateEnum.BossState.FirstState;
        _firstStateIsActive = true;

    }

    private void ChangeStates()
    {
        if (bossHealth <= 5)
        {
            _canBeSecondState = false;
            _bossState = EnemyStateEnum.BossState.ThirdState;
        }
        else if (bossHealth <=0)
        {
            _bossState = EnemyStateEnum.BossState.FourthState;
        }
        else if(bossHealth > 5)
        {
            _canBeSecondState = true;
        }
    }

    
    private void StorePlayerPosition()
    {
        _playerLastPosition = _playerPrefab.transform.position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(attackPos.position, new Vector3(attackRangeX, attackRangeY, 1));
    }

    private void Attack()
    {
        if (Vector2.Distance(_playerPrefab.position, _enemy.transform.position) < 2f && _playerPrefab.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (Time.time > _nextAttackTime)
            {
               // Collider2D attackRange = Physics2D.OverlapCircleAll() trzeba dodać kierunek w który się porusza i wtedy jak w drugą strone 

                Health.myHealth--;
                float fireRate = 1f;
                _nextAttackTime = Time.time + fireRate;
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

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.pink;
    //    Gizmos.DrawWireCube(attackPos.position, new Vector3(attackRangeX, attackRangeY, 1));
    //}
    //if (_bossState == EnemyStateEnum.BossState.SecondState)
    //{
    //    if (!canDash && !_canBlock)
    //    {
    //        if (Vector2.Distance(transform.position, _playerLastPosition) >= _stoppingDistance)
    //        {
    //        if (Vector2.Distance(transform.position, moveSpots[_randomSpot].position) < 0.2f) // distance between 2 spots then wait some time and taking another spot
    //            {

    //                _randomSpot = Random.Range(0, moveSpots.Length);
    //            }

    //        }
    //        else if (Vector2.Distance(transform.position, _playerLastPosition) <= 0)
    //        {
    //            _storedPlayerPosition = false;
    //            if (!_storedPlayerPosition)
    //            {
    //                StorePlayerPosition();
    //                Debug.Log(_storedPlayerPosition);
    //                _storedPlayerPosition = true;

    //            }
    //            // deal dmg

    //        }
    //    }

    //}
}

﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
	[SerializeField] private EnemyStateEnum.State _state;

	public int health = 3;
	public float speed;
	// private float dazedTime;
	// public float startDazedTime;

	public GameObject hitEffect;
	public GameObject fireballHitEffect;

	//private Animator anim;


	private float _speed = 5f;
	private float stoppingDistance = 2f;
	private float nextAttackTime;

	private Transform _target;
	[SerializeField] private GameObject _enemy;

	[SerializeField] public GameObject _player;

	public static bool _isAlive = true;

	//[SerializeField] private Transform _leftPoint;
	//[SerializeField] private Transform _rightPoint;
	public Transform[] moveSpots;
	private int randomSpot;
	private float waitTime;
	public float startWaitTime;

	private bool isPatrolling;


	float faceLeft;
	Vector3 characterScale;
	float characterScaleX;

	private bool isFacingLeft;

	public float characterMovePos;

	public Vector2 posEnem;
	public Animator animator;


	//
	private void Start()
	{
		faceLeft = _enemy.transform.localScale.x;
		
		characterScale = transform.localScale;
		characterScaleX = characterScale.x;
		_enemy.gameObject.GetComponent<Rigidbody2D>();
		_state = EnemyStateEnum.State.Patrol;
		_target = _player.gameObject.GetComponent<Transform>();
		Debug.Log(_state);

		//anim = GetComponent<Animator>();
		//anim.SetBool("isRunning", true);
	}


	private void Update()
	{
		//Vector3 localScale;

		if (posEnem.x > 0)
		{
			//_enemy.transform.localScale.x = -8;
			characterScale.x = characterScaleX;
		}
		else
		{
			characterScale.x -= characterScaleX;
		}

		characterMovePos = Input.GetAxis("Horizontal");
		


		CurrentState();

		if (health <= 0)
		{
			_state = EnemyStateEnum.State.Death;
			_isAlive = false;
			EnemyBoss._enemyAmount--;
			Debug.Log(EnemyBoss._enemyAmount);
			// animacja teksturki i jej wylaczenie
			StartCoroutine(WaitFewSeconds());
			Destroy(gameObject);
			
		}
		
	//	Dazing();
	}



	public void TakeMeleeDamage(int damage)
	{
		// play a hurt sound 
		Instantiate(hitEffect, transform.position, Quaternion.identity);
		health -= damage;
		Debug.Log("damage TAKEN !" + damage);


		//dazedTime = startDazedTime;
	}

	private void CurrentState()
	{
		switch (_state)
		{
			
			case EnemyStateEnum.State.Patrol:

				//Debug.Log("I am patrolling");

				// function of patrol when player is in range change to spot
				Patrol();
				break;

			case EnemyStateEnum.State.Spot: Debug.Log("I spotted you");

				// function of wait and change to Attack and Follow
				StartCoroutine(WaitToChangeState());
				Spot();
				break;

			case EnemyStateEnum.State.Attack: // Debug.Log("I will attack");

				// attack and follow, if enemy is on low then run and disappear if player is too far
				Follow();
				if (Vector2.Distance(_target.transform.position, _enemy.transform.position) < 2f)
				{
					WaitToChangeState();
					Attack();
					if (Health.myHealth == 0)
					{
						_state = EnemyStateEnum.State.Triumph;
					}
				}
				// if enemy has 0 hp then change to death
				// if enemy has won turn on animation from triumph
				break;

			case EnemyStateEnum.State.Run: Debug.Log ("I am leaving");
				// function to run away from player
				// if player is too far from enemy then disappear
				Run();
				break;

			case EnemyStateEnum.State.Death:
				// remove collider and change texture

					Debug.Log("I died");
				break;

			case EnemyStateEnum.State.Triumph: 
				// turn animation triumph, wait few seconds then screen u are dead and return player to beggining 
				
					Debug.Log("HERO HAS WON");
				StartCoroutine(WaitFewSeconds());
				_state = EnemyStateEnum.State.Patrol;
				
				break;

		}
	}
	
	private void Patrol()
	{
		// Changing scale to flip object when move to spots 

		Vector3 localScaleFollow = transform.localScale;
		isPatrolling = true;
		if (isPatrolling)
		{
			transform.position = Vector2.MoveTowards(transform.position, moveSpots[randomSpot].position, _speed * Time.deltaTime);


			posEnem = (moveSpots[randomSpot].transform.position - _enemy.transform.position).normalized;
			//Debug.Log(posEnem);
			// positions between spot and enemy from 0 to 1
			if (posEnem.x > 0 && !isFacingLeft)   
			{
				isFacingLeft = true;
				//_enemy.transform.localScale.x = -8;
				localScaleFollow.x *= -1;
			}
			else if (posEnem.x < 0 && isFacingLeft)
			{
				isFacingLeft = false;
				localScaleFollow.x *= -1;
			}
			transform.localScale = localScaleFollow;


			if (Vector2.Distance(transform.position, moveSpots[randomSpot].position) < 0.2f) // distance between 2 spots then wait some time and taking another spot
			{

				if (waitTime <= 0)
				{
					randomSpot = Random.Range(0, moveSpots.Length);
					waitTime = startWaitTime;
				}
				else
				{
					waitTime -= Time.deltaTime;
				}
		
			}

	
		}

		if (Vector2.Distance(_player.transform.position, _enemy.transform.position) <= 7)
		{
			isPatrolling = false;
			_state = EnemyStateEnum.State.Spot;
			Debug.Log("Spotted");
		}
	}

	private void Spot()
	{
		// animacja wlaczona ze wyspotowało
		// czekanie jednej sekundy
		// zmiana na follow

		// tutaj zmiana teksturki
		//StartCoroutine(WaitToChangeState());
		if (Vector2.Distance(_player.transform.position, _enemy.transform.position) >= 7)
		{
			_state = EnemyStateEnum.State.Patrol;
			isPatrolling = true;
		}
		else
		{
			_state = EnemyStateEnum.State.Attack;
		}
		

	}

	public void Follow()
	{
		Vector3 localScale = transform.localScale ;
		posEnem = (_player.transform.position - _enemy.transform.position).normalized;
		//Debug.Log(posEnem);
		if (posEnem.x > 0 && !isFacingLeft)
		{
			isFacingLeft = true;
			//_enemy.transform.localScale.x = -8;
			localScale.x *= -1;
		}
		else if(posEnem.x < 0 && isFacingLeft)
		{
			isFacingLeft = false;
			localScale.x *= -1;
		}
		transform.localScale = localScale;
		if (Vector2.Distance(transform.position, _target.position) > stoppingDistance)
		{
			transform.position = Vector2.MoveTowards(transform.position, new Vector2(_target.position.x, transform.position.y), _speed * Time.deltaTime);
			//transform.position = new Vector3(_speed * Time.deltaTime, 0.32f, 0);
			//transform.position = new Vector3(transform.position.x, 0.75f, 0);
			//transform.Translate(Vector3.forward * _speed * Time.deltaTime);
			// _enemy.transform.Translate(Vector3.forward * _speed, 0, 0);
		}
		if (Vector2.Distance(transform.position, _target.position) > 10)
		{
			_state = EnemyStateEnum.State.Patrol;
			isPatrolling = true;
		}
		if (health == 1)
		{
			_state = EnemyStateEnum.State.Run;

		}
	}
	private void Run()
	{
		transform.position = Vector2.MoveTowards(transform.position, _target.position, -_speed * Time.deltaTime);
		Debug.Log("I am running away");
		if (Vector2.Distance(_target.position, _enemy.transform.position) > 10)
		{
			_state = EnemyStateEnum.State.Patrol;
		}
	}
	private void Attack()
	{
		if (Vector2.Distance(_target.position, _enemy.transform.position) < 2f && _target.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			if (Time.time > nextAttackTime)
			{
				Health.myHealth--;
				float fireRate = 1f;
				nextAttackTime = Time.time + fireRate;
			}
		}
		

	}

	private IEnumerator WaitToChangeState()
	{
		yield return new WaitForSeconds(1);
	}

	private IEnumerator WaitFewSeconds()
	{
		yield return new WaitForSeconds(3);
	}

	
	//private void Dazing() // makes enemy slower when hit by player
	//{
	//	if (dazedTime <= 0)
	//	{
	//		_speed = 10;
	//	}
	//	else
	//	{
	//		_speed = 0;
	//		dazedTime -= startDazedTime;
	//	}
	//}
}

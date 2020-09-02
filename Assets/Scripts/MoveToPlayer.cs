using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPlayer : MonoBehaviour
{
    public Rigidbody2D _playerPrefab;
 //   [SerializeField] private GameObject _currentEnemy;
	private float stoppingDistance = 2f;
	private float _speed = 5f;
	public int enemyHealth = 3;
	public GameObject hitEffect;

	private float nextAttackTime;


	private void Update()
    {
		if (Input.GetKey(KeyCode.I))
		{
			Destroy(gameObject);
		}
		
		if (Vector2.Distance(transform.position, _playerPrefab.position) > stoppingDistance)
		{
			transform.position = Vector2.MoveTowards(transform.position, new Vector2(_playerPrefab.position.x, transform.position.y), _speed * Time.deltaTime);



		}
		if (enemyHealth <= 0)
		{
			EnemyBoss._enemyAmount--;
		}
		if (Vector2.Distance(transform.position, _playerPrefab.position) <= stoppingDistance && _playerPrefab.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			if (Time.time > nextAttackTime)
			{
				Health.myHealth--;
				float fireRate = 1f;
				nextAttackTime = Time.time + fireRate;
			}
		}
		
	}

	public void TakeMeleeDamage(int damage)
	{
		// play a hurt sound 
		Instantiate(hitEffect, transform.position, Quaternion.identity);
		enemyHealth -= damage;
		Debug.Log("damage TAKEN !" + damage);


		//dazedTime = startDazedTime;
	}

	
}

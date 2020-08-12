using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPlayer : MonoBehaviour
{
    [SerializeField] private Transform _playerPrefab;
 //   [SerializeField] private GameObject _currentEnemy;
	private float stoppingDistance = 2f;
	private float _speed = 5f;

    private void Update()
    {
		if (Vector2.Distance(transform.position, _playerPrefab.position) > stoppingDistance)
		{
			transform.position = Vector2.MoveTowards(transform.position, new Vector2(_playerPrefab.position.x, transform.position.y), _speed * Time.deltaTime);
		
		}
	}

}

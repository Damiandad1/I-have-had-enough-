using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMobs : MonoBehaviour
{
    [SerializeField] private GameObject _playerOnScene;
    [SerializeField] private GameObject[] _enemyPrefab;



    private int _numberOfEnemiesToSpawn;
    private bool _canSpawn;
    private bool  canSpawnCoroutine = true;

    private void Awake()
    {
        _canSpawn = true;
    }
    void Update()
    {
        if (Health.myHealth <= 0)
        {
            if (_canSpawn)
            {
                StartCoroutine(Some());
                _canSpawn = false;
            }
        }
      
      
    }


    private void SpawnEnemies()
    {
        
       _numberOfEnemiesToSpawn = Random.Range(4, 9);
        for (int i = 0; i < _numberOfEnemiesToSpawn; i++)
        {


            Enemy enemyInstance = Instantiate(_enemyPrefab[i].GetComponent<Enemy>());
            enemyInstance._player = _playerOnScene;
        }


        _canSpawn = false;
    }


    private IEnumerator Some()
    {
        
        if (canSpawnCoroutine)
        {
            SpawnEnemies();
        }
        _canSpawn = false;
        canSpawnCoroutine = false;
        yield return new WaitForSeconds(5);
        canSpawnCoroutine = true;
        
    }
}

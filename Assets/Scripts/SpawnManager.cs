using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject[] powerups;
    [SerializeField] private GameObject _asteroidPrefab;
    [SerializeField] private GameObject _asteroidContainer;
    
    private bool _stopSpawning = false;
    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnAsteroidRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 6.5f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn,Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(Random.Range(2.0f,4.0f));
        }
    }
    
    IEnumerator SpawnAsteroidRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 6.5f, 0);
            GameObject newAsteroid = Instantiate(_asteroidPrefab, posToSpawn,Quaternion.identity);
            newAsteroid.transform.parent = _asteroidContainer.transform;
            yield return new WaitForSeconds(Random.Range(2.0f,4.0f));
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 6.5f, 0);
            int powerUpType = Random.Range(0, 3);
            GameObject newPowerUp = Instantiate(powerups[powerUpType], posToSpawn,Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3,8));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}

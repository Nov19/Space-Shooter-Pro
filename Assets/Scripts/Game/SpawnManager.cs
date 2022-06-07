using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;

    [SerializeField] private float _spawnRate = 5.0f;
    [SerializeField] private GameObject[] _powerups;

    private bool isPlayerAlive = true;

    // Start is called before the first frame update
    // void Start()
    // {
    //     StartSpawning();
    // }
    
    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(Random.Range(2.0f, 5.0f));
        
        while (isPlayerAlive)
        {
            Vector3 position = new Vector3(Random.Range(-10.5f, 10.5f), 7.0f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, position, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform; 
            yield return new WaitForSeconds(_spawnRate);
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(Random.Range(8.0f, 15.0f));
        
        while (isPlayerAlive)
        {
            Vector3 position = new Vector3(Random.Range(-10.5f, 10.5f), 7.0f, 0);
            Instantiate(_powerups[Random.Range(0,3)], position, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(7.0f, 15.0f));
        }
    }

    
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }
    
    public void OnPlayerDeath()
    {
        isPlayerAlive = false;
    }
}

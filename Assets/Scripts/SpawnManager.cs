using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject[] powerups;

    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _powerupContainer;

    private bool _stopSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        // spawn game objects every 5 seconds
        while (!_stopSpawning)
        {
            float randomX = Random.Range(-8f, 8f);
            Vector3 randomPosition = new Vector3(randomX, 7f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, randomPosition, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(5f);
        }
    }

    private IEnumerator SpawnPowerupRoutine()
    {
        // every 3-7 seconds, spawn in a powerup
        while(!_stopSpawning)
        {
            float randomX = Random.Range(-8f, 8f);
            Vector3 randomPosition = new Vector3(randomX, 7f, 0);
            GameObject newPowerup = Instantiate(powerups[Random.Range(0, 3)], randomPosition, Quaternion.identity);
            newPowerup.transform.parent = _powerupContainer.transform;

            yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }


    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}

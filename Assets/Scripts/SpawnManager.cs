using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] powerUps;
    private bool _isPlayerDeath = false;
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnTripleShotPowerUpRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_isPlayerDeath == false)
        {
            float randomX = Random.Range(-9.4f, 9.4f);
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(randomX, 6.38f, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator SpawnTripleShotPowerUpRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        while (_isPlayerDeath == false)
        {
            int randomPowerUp = Random.Range(0, 3);
            GameObject NewTripleShotPowerUp = Instantiate(powerUps[randomPowerUp], new Vector3(Random.Range(-9.4f, 9.4f), 6.38f, 0), Quaternion.identity);
            int wait_time = Random.Range(3, 8);
            yield return new WaitForSeconds(wait_time);
        }
    }

    public void OnPlayerDeath()
    {
        _isPlayerDeath = true;
    }

}

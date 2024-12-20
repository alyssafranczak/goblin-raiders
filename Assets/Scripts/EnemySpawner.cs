using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    public int spawnAmount = 5;
    public float spawnTime = 3;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnEnemies", spawnTime, spawnTime);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (spawnAmount == 0 || LevelManager.isGameOver)
        {
            CancelInvoke();
        }
    }

    void SpawnEnemies()
    {
        GameObject spawnedEnemy = Instantiate(enemyPrefab, transform.position, transform.rotation) as GameObject;

        spawnedEnemy.transform.parent = gameObject.transform;
        spawnAmount--;
    }
}

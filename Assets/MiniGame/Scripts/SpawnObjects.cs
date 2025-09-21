using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnObjects : MonoBehaviour
{
    [Header ("Enemies")]
    public GameObject[] enemies;
    public float repeatSpawnRateEnemies = 3f;
    public float timeSpawnEnemies = 1f; 

    [Header ("Fruits")]
    public GameObject[] fruits;
    public float repeatSpawnRateFruits = 3f;
    public float timeSpawnFruits = 1f;

    [Header ("Spawn Range")]
    public Transform xRangeLeft; // Left boundary for spawning
    public Transform xRangeRight; // Right boundary for spawning
    public Transform yRangeUp;
    public Transform yRangeDown;

    public float difficultyTime = 0;

    void Start()
    {
        StartCoroutine(EnemyDifficulty());
        StartCoroutine(FruitDifficulty());
    }

    void Update()
    {
        difficultyTime += Time.deltaTime;
        if(difficultyTime > 10 && difficultyTime< 20){
            repeatSpawnRateEnemies = 2f;
            repeatSpawnRateFruits = 3.5f;
        } 
        if(difficultyTime > 20 && difficultyTime< 30){
            repeatSpawnRateEnemies = 1f;
            repeatSpawnRateFruits = 4f;
        }
        if(difficultyTime > 30 && difficultyTime< 50){
            repeatSpawnRateEnemies = 0.75f;
            repeatSpawnRateFruits = 4.5f;   
        }
        if(difficultyTime > 50){
            repeatSpawnRateEnemies = 0.25f;
            repeatSpawnRateFruits = 5f;
        }
    }

    IEnumerator EnemyDifficulty()
    {
        yield return new WaitForSeconds(repeatSpawnRateEnemies);
        SpawnEnemies();
        StartCoroutine(EnemyDifficulty());
    }

    IEnumerator FruitDifficulty()
    {
        yield return new WaitForSeconds(repeatSpawnRateFruits);
        SpawnFruits();
        StartCoroutine(FruitDifficulty());
    }

    public void SpawnEnemies()
    {
        Vector3 spawnPosition = new Vector3(0,0,0);
        spawnPosition= new Vector3(Random.Range(xRangeLeft.position.x, xRangeRight.position.x), Random.Range(yRangeDown.position.y, yRangeUp.position.y), 0);
        GameObject enemie = Instantiate(enemies[Random.Range(0, enemies.Length)], spawnPosition, gameObject.transform.rotation);
    }

    public void SpawnFruits()
    {
        Vector3 spawnPosition = new Vector3(0,0,0);
        spawnPosition= new Vector3(Random.Range(xRangeLeft.position.x, xRangeRight.position.x), Random.Range(yRangeDown.position.y, yRangeUp.position.y), 0);
        GameObject fruit = Instantiate(fruits[Random.Range(0, fruits.Length)], spawnPosition, gameObject.transform.rotation);
    }
}

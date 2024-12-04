using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawnerScript : MonoBehaviour
{
    public GameObject[] enemieList;
    public int sabitY = 5;
    public int spawnSayac = 0;
    public bool isSpawning = false;

    void Start()
    {
        
    }

     void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("TriggerEnter");
        if (other.CompareTag("Player") && !isSpawning)
        {
            StartCoroutine(enemieSpawn());
        }
    }
    

    IEnumerator enemieSpawn()
    {
        isSpawning = true;
        
        while (spawnSayac < 15)
        {
            int randomEnemy = Random.Range(0, enemieList.Length);
            Vector2 spawnPos = new Vector2(Random.Range(-9.0f, 9.0f), sabitY);

            Instantiate(enemieList[randomEnemy], spawnPos, Quaternion.identity);

            yield return new WaitForSeconds(1.0f);

            spawnSayac++;
            Debug.Log(spawnSayac);
        }
        
        isSpawning = false;
        Debug.Log("Spawn işlemi tamamlandı.");
    }
    
}

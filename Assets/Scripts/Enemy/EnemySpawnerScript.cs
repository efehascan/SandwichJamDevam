using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawnerScript : MonoBehaviour
{
    // Spawn edilecek düşmanların listesi (Prefab nesneleri burada tanımlanır)
    public GameObject[] enemieList;

    // Düşmanların doğacağı sabit Y koordinatı
    public int sabitY = 5;

    // Kaç tane düşmanın spawn edildiğini takip eden sayaç
    public int spawnSayac = 0;

    // Spawn işleminin devam edip etmediğini takip eden bayrak
    public bool isSpawning = false;

    // Başlangıçta çalışan fonksiyon (şu an kullanılmıyor)
    void Start()
    {
        // İleride gerekli başlatma işlemleri buraya eklenebilir
    }

    // Oyuncu spawner'ın trigger alanına girdiğinde çalışır
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("TriggerEnter");

        // Eğer trigger alanına giren nesne oyuncuysa ve şu anda spawn işlemi yapılmıyorsa
        if (other.CompareTag("Player") && !isSpawning)
        {
            // Spawn işlemini başlat
            StartCoroutine(enemieSpawn());
        }
    }

    // Düşmanların belirli aralıklarla spawn edilmesini sağlayan Coroutine
    IEnumerator enemieSpawn()
    {
        isSpawning = true; // Spawn işleminin başladığını işaretle

        // Spawn işlemini 15 düşman üretilene kadar devam ettir
        while (spawnSayac < 15)
        {
            // Rastgele bir düşman seç
            int randomEnemy = Random.Range(0, enemieList.Length);

            // Rastgele bir X koordinatında düşman spawn pozisyonu oluştur
            Vector2 spawnPos = new Vector2(Random.Range(-9.0f, 9.0f), sabitY);

            // Seçilen düşmanı sahneye instantiate et
            Instantiate(enemieList[randomEnemy], spawnPos, Quaternion.identity);

            // Bir saniye bekle
            yield return new WaitForSeconds(1.0f);

            // Spawn sayacını bir artır
            spawnSayac++;
            Debug.Log(spawnSayac); // Konsola mevcut sayaç değerini yazdır
        }

        isSpawning = false; // Spawn işlemi tamamlandı
        Debug.Log("Spawn işlemi tamamlandı.");
    }
}

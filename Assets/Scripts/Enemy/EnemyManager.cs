using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // Oyuncuyu temsil eden nesne
    public GameObject player;

    // Düşmanın hareket hızı
    public float speed;

    // Oyuncuyla düşman arasındaki mesafe
    public float distance;

    // Düşmanın verdiği hasar
    public int damage = 10;

    // Oyuncunun sağlık durumunu kontrol eden MainCharacter referansı
    private MainCharacter playerHealth;

    // Düşmanın saldırıp saldırmadığını takip eden bayrak
    bool HasAttacked = false;

    // Düşmanın oyuncuya çarptığında tetiklenen olay
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Eğer çarpışan nesne oyuncuysa saldırı fonksiyonunu çağır
        if (collision.CompareTag("Player"))
        {
            AttackPlayer();
        }
    }

    // Düşmanın oyuncuya saldırmasını sağlayan fonksiyon
    void AttackPlayer()
    {
        // Eğer oyuncu sağlığı (playerHealth) geçerli bir referansa sahipse
        if (playerHealth)
        {
            playerHealth.TakeDamage(damage); // Oyuncuya hasar ver
            Debug.Log("Düşman saldırdı! Hasar: " + damage);

            HasAttacked = true; // Saldırıldığını işaretle
            Die(); // Düşmanı yok et
        }
    }

    // Düşmanın yok edilmesini sağlayan fonksiyon
    void Die()
    {
        Debug.Log("Düşman yok oldu!");
        Destroy(gameObject); // Düşman nesnesini sahneden kaldır
    }

    // Başlangıçta çalışan fonksiyon
    void Start()
    {
        // Oyuncuyu "Player" etiketiyle bul ve referansı al
        player = GameObject.FindGameObjectWithTag("Player");

        // Oyuncunun sağlık yönetimini almak için MainCharacter bileşenini bul
        if (player != null)
        {
            playerHealth = player.GetComponent<MainCharacter>();
        }
    }

    // Her karede çağrılan fonksiyon
    void Update()
    {
        // Eğer oyuncu sahnede bulunmuyorsa uyarı ver
        if (player == null)
        {
            Debug.LogWarning("Player tagine sahip bir nesne bulunamadı!");
            return; // İşlem yapmayı durdur
        }

        // Oyuncu ile düşman arasındaki mesafeyi hesapla
        distance = Vector2.Distance(transform.position, player.transform.position);

        // Oyuncu ile düşman arasındaki yönü hesapla
        Vector2 direction = player.transform.position - transform.position;

        // Düşmanı oyuncuya doğru hareket ettir
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }
}

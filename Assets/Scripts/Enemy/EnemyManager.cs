using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject player;
    public float speed;
    public float distance;
    
    public int damage = 10;
    private MainCharacter playerHealth;
    bool HasAttacked = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !HasAttacked)
        {
            AttackPlayer();
        }
    }
    
    

    void AttackPlayer()
    {
        if (playerHealth )
        {
            playerHealth.TakeDamage(damage);
            Debug.Log("Düşman saldırdı! Hasar: " + damage);

            HasAttacked = true;
            Die();
        }
    }
    
    void Die()
    {
        Debug.Log("Düşman yok oldu!");
        Destroy(gameObject);
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    
    void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("Player tagine sahip bir nesne bulunamadı!");
            return;
        }
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        
    }
}
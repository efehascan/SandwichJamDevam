using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletSpawnPoint;
    [Range(0.1f, 1f)] [SerializeField] float bulletRate = 0.5f;
    
    // Yatay hareket girdisi (sağa/sola gitmek)
    float horizontal;

    // Karakterin yürüyüş hızı
    private float speed = 5.0f;

    // Karakterin zıplama gücü
    public float jumpingPower = 5.0f;

    // Karakterin yüzü sağa mı bakıyor?
    bool isFacingRight = true;

    // Karakterin maksimum sağlık puanı
    int maxLifeEnergy = 100;

    // Karakterin mevcut sağlık puanı
    private int currentEnergy;

    private void Start()
    {
        // Oyunun başında sağlık, maksimum değer olarak başlar
        currentEnergy = maxLifeEnergy;
    }

    // Karakterin hasar almasını sağlayan fonksiyon
    public void TakeDamage(int damage = 5)
    {
        currentEnergy -= damage; // Sağlık puanını azalt
        Debug.Log("Oyuncunun Kalan Sağlığı: " + currentEnergy);

        // Sağlık sıfıra ulaştığında öl
        if (currentEnergy <= 0)
        {
            Die();
        }
    }

    // Karakterin ölmesi durumunda çağrılan fonksiyon
    void Die()
    {
        Debug.Log("Oyuncu Öldü!");
        // Öldüğünde yapılacak diğer işlemler buraya eklenebilir
    }

    // Karakterin hangi yöne baktığını hesaplayan özellik
    private int IntRight
    {
        get
        {
            return isFacingRight ? 1 : -1;
        }
    }

    // Karakterin "dash" (hızlı hareket) yapabilme durumu
    bool canDash = true;

    // Karakter şu anda dash yapıyor mu?
    private bool isDashing;

    // Dash sırasında uygulanan hız
    private float dashingPower = 30f;

    // Dash süresi
    private float dashingTime = 0.2f;

    // Dash tekrar kullanılabilir hale gelmeden önceki bekleme süresi
    float dashingCooldown = 1f;

    // Rigidbody2D bileşeni (fizik etkileşimleri için)
    private Rigidbody2D rb2d;

    // Karakterin yere temas edip etmediğini kontrol etmek için kullanılan nokta
    [SerializeField] private Transform groundCheck;

    // Yer katmanını temsil eden LayerMask
    [SerializeField] private LayerMask groundLayer;

    // Dash sırasında iz bırakmak için kullanılan TrailRenderer bileşeni
    [SerializeField] public TrailRenderer trail;

    // Karakterin SpriteRenderer bileşeni (görsel temsil için)
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        // Bileşenleri referans al
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Kullanıcıdan yatay hareket girdisini al
        horizontal = Input.GetAxisRaw("Horizontal");

        // Eğer karakter şu anda dash yapıyorsa, diğer kontrolleri atla
        if (isDashing)
        {
            return;
        }

        // Zıplama komutu (boşluk tuşu) ve yerde olup olmadığını kontrol et
        if (Input.GetKeyUp(KeyCode.Space) && IsGrounded())
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpingPower);
        }

        // Zıplama tuşu bırakıldığında, yukarı hareket devam ediyorsa hareketi sabit tut
        if (Input.GetButtonUp("Jump") && rb2d.velocity.y > 0f)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpingPower);
        }

        // Dash komutunu kontrol et (Sol Shift tuşu)
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        // Karakterin yüzünü hareket yönüne göre döndür
        Flip();
    }



    public void Shoot()
    {
        Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
    }
    

    private void FixedUpdate()
    {
        // Eğer karakter şu anda dash yapıyorsa, hareketi durdur
        if (isDashing)
        {
            return;
        }

        // Yatay hareketi uygulamak için Rigidbody2D'nin hızını ayarla
        rb2d.velocity = new Vector2(horizontal * speed, rb2d.velocity.y);
    }

    // Karakterin yere temas edip etmediğini kontrol eden fonksiyon
    bool IsGrounded()
    {
        // Yer kontrolü için bir çember çizerek LayerMask ile çakışmayı kontrol et
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    // Karakterin yüzünü hareket yönüne döndüren fonksiyon
    void Flip()
    {
        // Eğer sağa bakarken sola gidiyorsa veya sola bakarken sağa gidiyorsa döndür
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight; // Yönü tersine çevir
            spriteRenderer.flipX = !isFacingRight; // Sprite'ı yatayda ters çevir
        }
    }

    // Dash mekanizmasını yöneten IEnumerator fonksiyonu
    IEnumerator Dash()
    {
        canDash = false; // Dash kullanımını devre dışı bırak
        isDashing = true; // Dash modunu etkinleştir
        float originalGravity = rb2d.gravityScale; // Mevcut yer çekimi değerini kaydet
        rb2d.gravityScale = 0f; // Yer çekimini geçici olarak kapat
        rb2d.velocity = new Vector2(IntRight * dashingPower, 0f); // Dash hızını uygula
        trail.emitting = true; // TrailRenderer'ı etkinleştir
        yield return new WaitForSeconds(dashingTime); // Dash süresi kadar bekle
        trail.emitting = false; // TrailRenderer'ı devre dışı bırak
        rb2d.gravityScale = originalGravity; // Yer çekimini eski haline getir
        isDashing = false; // Dash modunu kapat
        yield return new WaitForSeconds(dashingCooldown); // Cooldown süresi kadar bekle
        canDash = true; // Dash kullanımını yeniden etkinleştir
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Kameranın hedeften olan pozisyon farkı (offset)
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    // Kameranın hedefe yumuşak bir şekilde hareket etme süresi
    float smoothTime = 0.25f;

    // Kameranın hızını takip eden bir vektör (SmoothDamp için gereklidir)
    Vector3 velocity = Vector3.zero;

    // Takip edilecek hedefin referansı (örneğin oyuncu)
    [SerializeField] public Transform target;

    // Her karede çağrılan fonksiyon
    void Update()
    {
        // Hedefin pozisyonunu ve offset'i toplayarak kameranın gitmesi gereken hedef pozisyonu belirle
        Vector3 targetPos = target.position + offset;

        // Kamerayı yumuşak bir şekilde hedef pozisyona taşı
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }
}
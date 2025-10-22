using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class Actions : MonoBehaviour
{
    [Header("Bala")]
    public GameObject Bullet;
    public float projectileSpeed = 10f;
    public float cooldown = 0.5f;

    [Header("SuperShot")]
    public float superCooldown = 10f;
    private float lastSuperTime = -10f;

    [Header("Moviment")]
    public float moveSpeed = 12f;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Dispar normal
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Shoot();
        }

        // Super dispar
        if (Keyboard.current.altKey.wasPressedThisFrame && Time.time - lastSuperTime >= superCooldown)
        {
            StartCoroutine(SuperShootSpread());
            lastSuperTime = Time.time;
        }

    }

    void FixedUpdate()
    {
        // Detectar teclat
        float moveDirection = 0f;
        if (Keyboard.current.dKey.isPressed) moveDirection = 1f;
        else if (Keyboard.current.aKey.isPressed) moveDirection = -1f;

        // Moviment amb física
        Vector2 movement = new Vector2(moveDirection * moveSpeed * Time.fixedDeltaTime, 0f);
        rb.MovePosition(rb.position + movement);
    }

    void Shoot()
    {
        if (Bullet == null) return;

        Vector3 spawnPos = transform.position + Vector3.up * 0.6f;
        GameObject projectile = Instantiate(Bullet, spawnPos, Quaternion.identity);

        Rigidbody2D rbBullet = projectile.GetComponent<Rigidbody2D>();
        if (rbBullet != null)
            rbBullet.linearVelocity = Vector2.up * projectileSpeed;

        // Ignorar col·lisió amb el player
        Collider2D playerCol = GetComponent<Collider2D>();
        Collider2D bulletCol = projectile.GetComponent<Collider2D>();
        if (playerCol != null && bulletCol != null)
            Physics2D.IgnoreCollision(playerCol, bulletCol, true);
    }

    private IEnumerator SuperShootSpread()
    {
        int bullets = 10;
        float spreadAngle = 180f;
        float delayBetweenShots = 0.05f;

        for (int i = 0; i < bullets; i++)
        {
            float angle = -spreadAngle / 2 + i * (spreadAngle / (bullets - 1));
            ShootAngle(angle);
            yield return new WaitForSeconds(delayBetweenShots);
        }
    }

    void ShootAngle(float angleDegrees)
    {
        if (Bullet == null) return;

        float rad = angleDegrees * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Sin(rad), Mathf.Cos(rad)).normalized;
        Vector3 spawnPos = transform.position + (Vector3)(dir * 2f);

        GameObject projectile = Instantiate(Bullet, spawnPos, Quaternion.identity);

        // Ignorar col·lisió amb el player
        Collider2D playerCol = GetComponent<Collider2D>();
        Collider2D bulletCol = projectile.GetComponent<Collider2D>();
        if (playerCol != null && bulletCol != null)
            Physics2D.IgnoreCollision(playerCol, bulletCol, true);

        Rigidbody2D rbBullet = projectile.GetComponent<Rigidbody2D>();
        if (rbBullet != null)
            rbBullet.linearVelocity = dir * projectileSpeed;
    }
}
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed = 5f;
    private float jumpForce = 14.5f;

    private Rigidbody2D rb;

    private bool isGrounded;
    private bool isInContact;

    public Transform groundCheck;
    public LayerMask groundLayer;
    public LayerMask objectLayer;

    private int jumpDirection = 0;
    private Vector3 startPosition;

    private bool isDead = false;

    [Header("Shoting")]
    public GameObject Bullet;
    public float projectileSpeed = 10f;

    [Header("SuperShot")]
    public float superCooldown = 10f;
    private float lastSuperTime = -10f;

    private PlayerHealthUI gestorVida;

    public int coins = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 3f;
        startPosition = transform.position;
        gestorVida = Object.FindFirstObjectByType<PlayerHealthUI>();
    }

    void Update()
    {
        // Comprovació si està tocant terra o objecte
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.3f, groundLayer);
        isInContact = Physics2D.OverlapCircle(groundCheck.position, 0.3f, objectLayer);
        bool isTouchingDeath = Physics2D.OverlapCircle(groundCheck.position, 0.3f, LayerMask.GetMask("Death"));

        if (isDead)
        {
            this.enabled = false;
            return;
        }

        if (isTouchingDeath)
        {
            // Si tens accés a la vida i UI des d’aquí:

            if (gestorVida != null && !gestorVida.gameOver)
            {
                gestorVida.playerVida = 0;
                gestorVida.gameOver = true;
                isDead = true;
                gestorVida.UpdateLifeUI();
                gestorVida.Text.endGame();
            }

            // Reset físic complet
            Freezeplayer();
        }
        
        if (gestorVida.playerVida == 0 && gestorVida.gameOver)
        {
            isDead = true;
            Freezeplayer();
            Invoke("reiniciarEscena", 3f);
        }

        // Disparar (només si no estàs saltant)
        if (Input.GetKeyDown(KeyCode.W))
        {
            Shoot();
        }
        else if (Input.GetKeyDown(KeyCode.LeftAlt) && Time.time - lastSuperTime >= superCooldown)
        {
            // Super dispar (amb ALT)
            StartCoroutine(SuperShootSpread());
            lastSuperTime = Time.time;
        }

        // Salt
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || isInContact))
        {
            if (Input.GetKey(KeyCode.A)) jumpDirection = -1;
            else if (Input.GetKey(KeyCode.D)) jumpDirection = 1;
            else jumpDirection = 0;

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // Moviment horitzontal
        if (isGrounded || isInContact)
        {
            if (Input.GetKey(KeyCode.A))
                rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);
            else if (Input.GetKey(KeyCode.D))
                rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
            else
                rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }
        else
        {
            // A l’aire: permet canvi de direcció amb la meitat de velocitat
            float airSpeed = moveSpeed * 0.8f;

            float moveInput = 0f;
            if (Input.GetKey(KeyCode.A)) moveInput = -1f;
            if (Input.GetKey(KeyCode.D)) moveInput = 1f;

            float targetX = moveInput * airSpeed;
            float smooth = 6f;

            float newVelX= Mathf.Lerp(rb.linearVelocity.x, targetX, Time.deltaTime*smooth);
            rb.linearVelocity = new Vector2(newVelX, rb.linearVelocity.y);
    }

    void Shoot()
    {
        if (Bullet == null) return;

        // Genera la bala lleugerament davant del personatge
        Vector3 spawnPos = transform.position + Vector3.right * 0.6f;
        GameObject projectile = Instantiate(Bullet, spawnPos, Quaternion.identity);

        Rigidbody2D rbBullet = projectile.GetComponent<Rigidbody2D>();
        if (rbBullet != null)
            rbBullet.linearVelocity = Vector2.right * projectileSpeed;

        Collider2D playerCol = GetComponent<Collider2D>();
        Collider2D bulletCol = projectile.GetComponent<Collider2D>();
        if (playerCol != null && bulletCol != null)
            Physics2D.IgnoreCollision(playerCol, bulletCol, true);
    }
    }

    private IEnumerator SuperShootSpread()
    {
        int bullets = 10;
        float spreadAngle = 190f;
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

        // Calcula la direcció i la rotació
        float rad = angleDegrees * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // Punt de generació allunyat del jugador
        Vector3 spawnPos = transform.position + (Vector3)(dir * 1.2f);
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        // Instancia la bala amb rotació
        GameObject projectile = Instantiate(Bullet, spawnPos, rotation);

        // Aplica velocitat
        Rigidbody2D rbBullet = projectile.GetComponent<Rigidbody2D>();
        if (rbBullet != null)
            rbBullet.linearVelocity = dir * projectileSpeed;

        // Ignora col·lisió amb el jugador
        Collider2D playerCol = GetComponent<Collider2D>();
        Collider2D bulletCol = projectile.GetComponent<Collider2D>();
        if (playerCol != null && bulletCol != null)
            Physics2D.IgnoreCollision(playerCol, bulletCol, true);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, 0.3f);
    }

    void Freezeplayer()
    {
        transform.position = startPosition;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void AddCoin()
    {
        coins++;
        Debug.Log("Monedes: " + coins);
        // Pots afegir actualització de UI aquí si vols
    }

    public int GetCoinCount()
    {
        return coins;
    }

    public void setLife(int life)
    {
        if (gestorVida != null)
        {
            gestorVida.playerVida = life;
            gestorVida.UpdateLifeUI();
        }
    }

    void reiniciarEscena()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}

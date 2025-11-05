using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement2D : MonoBehaviour
{
    [Header("Liikkuminen")]
    public float speed = 5f;
    private Rigidbody2D rigidbody2d;
    private Vector2 movement;

    [Header("Osumapisteet (Health)")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Kokemuspisteet (XP)")]
    public int experience = 0;         // Pelaajan kokemus
    public int levelUpThreshold = 100; // Kuinka paljon XP:tä tarvitaan tason nousuun
    private int level = 1;

    [Header("Kasvu XP:n mukaan")]
    private bool hasGrown = false;     // Kasvanutko jo ensimmäisen kerran
    public float growthScale = 1.5f;   // Kuinka paljon pelaaja kasvaa (1.5 = +50%)

    [Header("Ammus")]
    public GameObject projectilePrefab;
    private Vector2 moveDirection = new Vector2(1, 0);

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Liike (WASD / nuolinäppäimet)
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        // Suuntaa ammus viimeisimmän liikesuunnan mukaan
        if (movement.sqrMagnitude > 0.01f)
        {
            moveDirection = movement.normalized;
        }

        // Ammu Space-näppäimellä
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Launch();
        }
    }

    void FixedUpdate()
    {
        rigidbody2d.linearVelocity = movement * speed;
    }

    // ---------------------------
    // OSUMAPISTEET
    // ---------------------------
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log($"Pelaaja otti vahinkoa! HP: {currentHealth}");

        if (currentHealth <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log($"Pelaaja parani! HP: {currentHealth}");
    }

    void Die()
    {
        Debug.Log("Pelaaja kuoli!");
        gameObject.SetActive(false);
    }

    // ---------------------------
    // TRIGGER-TAPAHTUMAT
    // ---------------------------
    void OnTriggerEnter2D(Collider2D other)
    {
        // Parannusesine
        if (other.CompareTag("HealItem"))
        {
            HealItem heal = other.GetComponent<HealItem>();
            if (heal != null)
            {
                Heal(heal.healAmount);
                heal.Collect();
            }
        }

        // Kokemusesine
        if (other.CompareTag("XPItem"))
        {
            XPItem xp = other.GetComponent<XPItem>();
            if (xp != null)
            {
                GainExperience(xp.xpAmount);
                xp.Collect();
            }
        }
    }

    // ---------------------------
    // KOKEMUSPISTEET
    // ---------------------------
    public void GainExperience(int amount)
    {
        experience += amount;
        Debug.Log($"Pelaaja sai {amount} XP! Yhteensä: {experience}");

        // 🔹 Kasva kun XP >= 10 (vain kerran)
        if (experience >= 10 && !hasGrown)
        {
            GrowPlayer();
        }

        if (experience >= levelUpThreshold)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        level++;
        experience = 0; // Nollataan tai jätetään ylijäämä
        levelUpThreshold += 50;
        hasGrown = false; // Voit sallia uuden kasvun tasonousun jälkeen

        Debug.Log($"Taso nousi! Uusi taso: {level}");
    }

    // ---------------------------
    // PELAAN KASVU
    // ---------------------------
    void GrowPlayer()
    {
        transform.localScale *= growthScale; // kasvattaa kokoa
        hasGrown = true;
        Debug.Log("🎉 Pelaaja kasvoi suuremmaksi XP:n ansiosta!");
    }

    // ---------------------------
    // AMMUS
    // ---------------------------
    void Launch()
    {
        GameObject projectileObject = Instantiate(
            projectilePrefab,
            rigidbody2d.position + Vector2.up * 0.5f,
            Quaternion.identity
        );

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(moveDirection, 300);
    }
}

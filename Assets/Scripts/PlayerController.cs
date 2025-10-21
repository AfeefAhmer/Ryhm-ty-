using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement2D : MonoBehaviour
{
    [Header("Liikkuminen")]
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    [Header("Osumapisteet")]
    public int maxHealth = 100;   // Maksimi HP
    private int currentHealth;    // Nykyinen HP

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Liike: WASD ja nuolinäppäimet
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * speed;
    }

    // --- OSUMAPISTEET ---

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

    // --- PARANNUSESINEEN TUNNISTUS ---
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("HealItem"))
        {
            HealItem heal = other.GetComponent<HealItem>();
            if (heal != null)
            {
                Heal(heal.healAmount);      // Anna HP lisää
                heal.Collect();              // Poista esine
            }
        }
    }
}

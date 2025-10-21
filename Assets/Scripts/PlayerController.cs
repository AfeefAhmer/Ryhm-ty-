using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement2D : MonoBehaviour
{
    [Header("Liikkuminen")]
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    [Header("Osumapisteet (Health)")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Kokemuspisteet (XP)")]
    public int experience = 0;         // Pelaajan kokemus
    public int levelUpThreshold = 100; // Kuinka paljon XP:tä tarvitaan tason nousuun
    private int level = 1;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Liike (WASD / nuolinäppäimet)
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * speed;
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

        if (experience >= levelUpThreshold)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        level++;
        experience = 0; // tai jätä ylijäämä XP jos haluat realistisemman progression
        levelUpThreshold += 50; // kasvattaa seuraavan tason vaatimusta

        Debug.Log($"Taso nousi! Uusi taso: {level}");
    }
}

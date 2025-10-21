using UnityEngine;

public class HealItem : MonoBehaviour
{
    [Header("Parannusarvo")]
    public int healAmount = 25; // Kuinka paljon HP:tä palautetaan

    public void Collect()
    {
        Debug.Log($"Parannusesine kerätty! +{healAmount} HP");
        Destroy(gameObject); // Poista esine pelistä
    }
}

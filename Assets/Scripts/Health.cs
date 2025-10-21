using UnityEngine;

public class XPItem : MonoBehaviour
{
    [Header("Kokemuspisteiden m‰‰r‰")]
    public int xpAmount = 25; // Kuinka paljon XP:t‰ esine antaa

    public void Collect()
    {
        Debug.Log($"Kokemusesine ker‰tty! +{xpAmount} XP");
        Destroy(gameObject);
    }
}

using UnityEngine;
using TMPro;

public class Collector : MonoBehaviour, ICollector
{
    public int collectedCount { get; private set; } // Toplanan obje sayısı
    private Notifier notifier;
    [SerializeField] private TextMeshProUGUI coinText;

    private void Start()
    {
        // Eğer Notifier aynı GameObject'teyse:
        if (notifier == null)
        {
            notifier = FindObjectOfType<Notifier>(); // Diğer GameObject'lerde arama yapar
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            Destroy(other.gameObject);
            Collect();
            coinText.text = "Score: " + collectedCount.ToString();
        }
    }


    public void Collect()
    {
        collectedCount++;
        notifier.Notify(NotificationTypes.CoinIncrease);
    }
}

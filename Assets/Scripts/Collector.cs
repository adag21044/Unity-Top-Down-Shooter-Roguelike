using UnityEngine;
using TMPro;

public class Collector : MonoBehaviour, ICollector
{
    public int collectedCount { get; private set; } // Toplanan obje sayısı
    private Notifier notifier;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private ProgressBar progressBar; // Reference to the ProgressBar

    private void Start()
    {
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
            progressBar.IncreaseProgress(1f / 8f); // Increase progress by 1/8
        }
    }

    public void Collect()
    {
        collectedCount++;
        notifier.Notify(NotificationTypes.CoinIncrease);
    }
}

using UnityEngine;
using TMPro;

public class CoinDisplay : Observer
{
    public float maxInc = 5f;
    public TextMeshProUGUI coinText; 
    [SerializeField]private int currentCoinCount;
    private ProgressBar progressBar;

    private void Start()
    {
        var notifier = FindObjectOfType<Notifier>();
        if (notifier != null)
        {
            notifier.AddObserver(this);
        }

        progressBar = FindObjectOfType<ProgressBar>();
        if (progressBar == null)
        {
            Debug.LogError("ProgressBar component not found!");
        }
    }

    public override void OnNotify(NotificationTypes notificationType)
    {
        float incAmount = 1f;
        maxInc = maxInc - 1f;
        if (notificationType == NotificationTypes.CoinIncrease)
        {
            currentCoinCount++;
            coinText.text = "Coins: " + currentCoinCount;

            if (progressBar != null && maxInc > 0f)
            {
                progressBar.StartIncreasingWidth(incAmount);
            }
            else if(maxInc <= 0f) return;
            else
            {
                Debug.LogError("ProgressBar reference is null!");
            }
        }
    }

    private void OnDestroy()
    {
        var notifier = FindObjectOfType<Notifier>();
        if (notifier != null)
        {
            notifier.RemoveObserver(this);
        }
    }
}

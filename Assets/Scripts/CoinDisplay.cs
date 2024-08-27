using UnityEngine;
using TMPro;

public class CoinDisplay : Observer
{
    public TextMeshProUGUI coinText; 
    private int currentCoinCount;

    private void Start()
    {
        var notifier = FindObjectOfType<Notifier>();
        if (notifier != null)
        {
            notifier.AddObserver(this);
        }
    }

    public override void OnNotify(NotificationTypes notificationType)
    {
        if (notificationType == NotificationTypes.CoinIncrease)
        {
            currentCoinCount++;
            coinText.text = "Coins: " + currentCoinCount;
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

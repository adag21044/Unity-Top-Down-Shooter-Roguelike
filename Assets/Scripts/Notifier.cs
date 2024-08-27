using System.Collections.Generic;
using UnityEngine;

public class Notifier : MonoBehaviour
{
    private List<Observer> observers = new List<Observer>();

    public void AddObserver(Observer observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(Observer observer)
    {
        observers.Remove(observer);
    }

    public void Notify(NotificationTypes notificationType)
    {
        foreach (var observer in observers)
        {
            observer.OnNotify(notificationType);
        }
    }
}

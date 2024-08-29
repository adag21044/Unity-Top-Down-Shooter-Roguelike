using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image ProgressImage;
    [SerializeField] private float DefaultSpeed = 1f;
    //[SerializeField] private UnityEvent<float> OnProgress;
    //[SerializeField] private UnityEvent OnCompleted;

    private Coroutine AnimationCoroutine;

    private void Start()
    {
        if (ProgressImage.type != Image.Type.Filled)
        {
            Debug.LogError("ProgressImage must be of type Filled");
            this.enabled = false;
        }

        SetProgress(0);
    }

    private void SetProgress(float progress)
    {
        SetProgress(progress, DefaultSpeed);
    }

    private void SetProgress(float progress, float speed)
    {
        progress = Mathf.Clamp01(progress); // Ensure progress is within valid range

        if (progress != ProgressImage.fillAmount)
        {
            if (AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }
            AnimationCoroutine = StartCoroutine(AnimateProgress(progress, speed));
        }
    }

    public void IncreaseProgress(float amount)
    {
        SetProgress(ProgressImage.fillAmount + amount, DefaultSpeed); // Increase by specified amount
    }

    private IEnumerator AnimateProgress(float progress, float speed)
    {
        float time = 0;
        float initialProgress = ProgressImage.fillAmount;

        while (time < 1)
        {
            ProgressImage.fillAmount = Mathf.Lerp(initialProgress, progress, time);
            time += Time.deltaTime * speed;

            //OnProgress?.Invoke(ProgressImage.fillAmount);
            yield return null;
        }

        ProgressImage.fillAmount = progress;
        //OnProgress?.Invoke(progress);
        //OnCompleted?.Invoke();
    }
}

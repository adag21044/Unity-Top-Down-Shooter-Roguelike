using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform; // İlgili RectTransform
    [SerializeField] private float widthIncreaseRate = 10f; // Genişlik artış hızı
    [SerializeField] private float increaseDuration = 1f; // Artış süresi
    [SerializeField] private float maxWidth = 200f; // ProgressBar'ın maksimum genişliği

    private bool isIncreasing = false;
    private float targetWidth;
    private float initialWidth;

    private void Start()
    {
        // RectTransform'un pivot noktasını sağa ayarla
        if (rectTransform != null)
        {
            rectTransform.pivot = new Vector2(0, rectTransform.pivot.y);
            rectTransform.anchorMin = new Vector2(0, rectTransform.anchorMin.y);
            rectTransform.anchorMax = new Vector2(0, rectTransform.anchorMax.y);

            // ProgressBar'ın başlangıç genişliğini sakla
            initialWidth = rectTransform.sizeDelta.x;
        }
    }

    private void Update()
    {
        if (isIncreasing)
        {
            // Genişliği belirtilen süreye göre artır
            float step = widthIncreaseRate * Time.deltaTime;
            Vector2 size = rectTransform.sizeDelta;
            size.x = Mathf.Clamp(size.x + step, initialWidth, targetWidth);
            rectTransform.sizeDelta = size;

            if (size.x >= targetWidth)
            {
                isIncreasing = false;
            }
        }
    }

    public void StartIncreasingWidth(float increaseAmount)
    {
        if (!isIncreasing)
        {
            targetWidth = Mathf.Clamp(rectTransform.sizeDelta.x + increaseAmount, initialWidth, maxWidth);
            StartCoroutine(IncreaseWidthForDuration());
        }
    }

    private IEnumerator IncreaseWidthForDuration()
    {
        isIncreasing = true;
        yield return new WaitForSeconds(increaseDuration);
        isIncreasing = false;
    }
}

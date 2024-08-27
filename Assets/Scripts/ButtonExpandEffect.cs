using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonExpandEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rectTransform;
    private Vector3 originalScale;
    public Vector3 expandedScale = new Vector3(1.1f, 1.1f, 1.1f); // Scale to expand to on hover
    public float expandSpeed = 0.2f; // Speed of the expand effect

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleTo(expandedScale));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleTo(originalScale));
    }

    private IEnumerator ScaleTo(Vector3 targetScale)
    {
        Vector3 initialScale = rectTransform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < expandSpeed)
        {
            rectTransform.localScale = Vector3.Lerp(initialScale, targetScale, (elapsedTime / expandSpeed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.localScale = targetScale;
    }
}

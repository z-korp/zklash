using System.Collections;
using UnityEngine;

public class IndicatorManager : MonoBehaviour
{
    public GameObject indicatorPrefab;
    public Transform target;
    private GameObject indicator;
    private bool isAnimating = false; // Add flag to indicate animation is in progress

    public void CreateIndicator()
    {
        if (indicator != null)
        {
            StopAnimation(); // Stop annimation before destroying indicator or transform will trigger error
            Destroy(indicator);
        }

        indicator = Instantiate(indicatorPrefab, target.position, Quaternion.identity);
        StartAnimation();
    }

    public void DestroyIndicator()
    {
        if (indicator != null)
        {
            StopAnimation();
            Destroy(indicator);
            indicator = null;
        }
    }

    private void StartAnimation()
    {
        isAnimating = true;
        StartCoroutine(AnimateIndicator());
    }

    private void StopAnimation()
    {
        isAnimating = false;
    }

    private IEnumerator AnimateIndicator()
    {
        float duration = 0.5f;
        Vector3 scaleUp = new Vector3(1.5f, 1.5f, 1f);
        Vector3 scaleDown = new Vector3(1.2f, 1.2f, 1f);

        // Utilisez isAnimating pour contrôler la boucle
        while (isAnimating)
        {
            // Animation de grossissement
            float timer = 0f;
            while (timer <= duration && isAnimating)
            {
                indicator.transform.localScale = Vector3.Lerp(indicator.transform.localScale, scaleUp, timer / duration);
                timer += Time.deltaTime;
                yield return null;
            }

            // Animation de rétrécissement
            timer = 0f;
            while (timer <= duration && isAnimating)
            {
                indicator.transform.localScale = Vector3.Lerp(indicator.transform.localScale, scaleDown, timer / duration);
                timer += Time.deltaTime;
                yield return null;
            }
        }
    }
}

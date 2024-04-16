using System.Collections;
using UnityEngine;
using TMPro;

public class MobDamageText : MonoBehaviour
{
    public TextMeshProUGUI damageText;
    public float moveSpeed = 1f;
    public float displayTime = 1f;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = damageText.transform.position;
        damageText.gameObject.SetActive(false);
    }
    public void ShowDamage(int damageAmount)
    {
        Rigidbody2D rb = GetComponentInParent<Rigidbody2D>();
        if (rb != null)
        {
            // Update the position to follow the Rigidbody2D component each time damage is shown
            initialPosition = rb.position + Vector2.up * 0.8f;
            damageText.transform.position = initialPosition; // Ensure the text starts at the updated position
        }
        else
        {
            Debug.LogError("No Rigidbody2D found in parent hierarchy!");
            return; // Exit the method as there's no point to continue without a position
        }

        damageText.text = "-" + damageAmount.ToString();
        damageText.gameObject.SetActive(true);
        StartCoroutine(MoveAndFade());
    }


    IEnumerator MoveAndFade()
    {
        float elapsedTime = 0;
        Vector3 startPosition = damageText.transform.position; // Ensure this is updated each time the coroutine starts

        while (elapsedTime < displayTime)
        {
            float fadeAmount = elapsedTime / displayTime;
            damageText.transform.position = Vector3.Lerp(startPosition, startPosition + Vector3.up * 0.5f, fadeAmount);
            damageText.color = new Color(damageText.color.r, damageText.color.g, damageText.color.b, 1 - fadeAmount);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        damageText.gameObject.SetActive(false);
        // Reset position and color after fading is done for potential next use
        damageText.transform.position = initialPosition;
        damageText.color = new Color(damageText.color.r, damageText.color.g, damageText.color.b, 1);
    }

}

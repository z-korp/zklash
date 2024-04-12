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
        damageText.text = "-" + damageAmount.ToString();
        damageText.gameObject.SetActive(true);
        StartCoroutine(MoveAndFade());
    }

    IEnumerator MoveAndFade()
    {
        float elapsedTime = 0;
        Vector3 startPosition = damageText.transform.position;

        while (elapsedTime < displayTime)
        {
            damageText.transform.position = Vector3.Lerp(startPosition, startPosition + Vector3.up * 0.5f, elapsedTime / displayTime);
            damageText.color = new Color(damageText.color.r, damageText.color.g, damageText.color.b, 1 - (elapsedTime / displayTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        damageText.gameObject.SetActive(false);
        damageText.transform.position = initialPosition;
        damageText.color = new Color(damageText.color.r, damageText.color.g, damageText.color.b, 1);
    }

}

using System.Collections;
using UnityEngine;
using TMPro;

public class MobHealth : MonoBehaviour
{
    public int health;

    public Animator animator;

    public SpriteRenderer spriteRenderer;

    public TextMeshProUGUI txtLife;

    public GameObject canvas;

    public bool isBlinking = false;
    public float blinkDuration = 0.2f;
    public float blinkTimeAfterHit = 1f;

    void Start()
    {
        SetTextHealth(health);
    }

    public bool TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            SetTextHealth(health);
            Die();
            return true;
        }
        SetTextHealth(health);

        // Animation
        isBlinking = true;
        StartCoroutine(BlinkDamageFlash());
        StartCoroutine(HandleBlinkDelay());
        return false;
    }

    public void HealPlayer(int amount)
    {
        health += amount;
        SetTextHealth(health);

        isBlinking = true;
        StartCoroutine(BlinkHealFlash());
        StartCoroutine(HandleBlinkDelay());
    }

    public void Die()
    {
        animator.SetTrigger("Death");
        canvas.SetActive(false);
    }

    public void SetTextHealth(int amount)
    {
        txtLife.text = amount.ToString();
    }

    public IEnumerator BlinkHealFlash()
    {
        while (isBlinking)
        {
            spriteRenderer.color = new Color(0f, 1f, 0f, 1f);
            yield return new WaitForSeconds(blinkDuration);
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(blinkDuration);
        }
    }

    public IEnumerator BlinkDamageFlash()
    {
        while (isBlinking)
        {
            spriteRenderer.color = new Color(1f, 0f, 0f, 1f);
            yield return new WaitForSeconds(blinkDuration);
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(blinkDuration);
        }
    }

    public IEnumerator HandleBlinkDelay()
    {
        yield return new WaitForSeconds(blinkTimeAfterHit);
        isBlinking = false;
    }
}

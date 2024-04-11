using System.Collections;
using UnityEngine;
using TMPro;

public class MobHealth : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public MobData mobData;
    private int health;

    public TextMeshProUGUI txtLife;

    public GameObject canvas;

    [HideInInspector]
    public bool isBlinking = false;
    public float blinkDuration = 0.2f;
    public float blinkTimeAfterHit = 1f;

    void Awake()
    {
        health = mobData.health;

        animator = GetComponentInParent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found in parent GameObject.", this);
        }

        spriteRenderer = GetComponentInParent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found in parent GameObject.", this);
        }

        SetTextHealth(health);
    }

    public bool TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            SetTextHealth(health);
            TriggerDie();
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

    public void TriggerDie()
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

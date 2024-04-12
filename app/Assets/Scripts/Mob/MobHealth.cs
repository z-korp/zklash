using System.Collections;
using UnityEngine;
using TMPro;
using System;

public class MobHealth : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public MobData mobData;

    [HideInInspector]
    public int health;

    [HideInInspector]
    public bool isDead = false;

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

    public IEnumerator TakeDamage(int amount)
    {
        MobDamageText damageTextComponent = GetComponent<MobDamageText>();
        if (damageTextComponent == null)
        {
            Debug.LogWarning("DamageTextComponent not found on the mob.");
        }
        else
        {
            damageTextComponent.ShowDamage(amount);
        }

        health -= amount;
        SetTextHealth(Math.Max(0, health));

        // Animation
        isBlinking = true;
        StartCoroutine(BlinkDamageFlash());
        yield return StartCoroutine(HandleBlinkDelay());

        if (health <= 0)
        {
            yield return TriggerDie();
            canvas.SetActive(false);
        }
    }

    public void HealPlayer(int amount)
    {
        health += amount;
        SetTextHealth(health);

        isBlinking = true;
        StartCoroutine(BlinkHealFlash());
        StartCoroutine(HandleBlinkDelay());
    }

    public IEnumerator TriggerDie()
    {
        animator.SetTrigger("Death");

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Dead"));
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8f);
    }

    public void TriggerDeathEffect()
    {
        Debug.Log("TriggerDeathEffect");
        isDead = true;
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

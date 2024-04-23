using System.Collections;
using UnityEngine;
using TMPro;
using System;
using zKlash.Game.Roles;

public class MobHealth : MonoBehaviour
{
    private MobController mobController;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public MobData mobData;

    public int Health
    {
        get
        {
            Debug.Assert(mobController != null, "MobController is not set on " + gameObject.name);
            return mobController ? mobController.Character.Health : 0;
        }
    }

    [HideInInspector]
    public bool isDead = false;

    public TextMeshProUGUI txtLife;

    [HideInInspector]
    public bool isBlinking = false;
    public float blinkDuration = 0.2f;
    public float blinkTimeAfterHit = 1f;

    [HideInInspector]
    public MobHealth source;

    void Awake()
    {
        //health = mobData.health;

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
    }

    void Start()
    {
        // Get the MobController component from the GameObject
        mobController = GetComponent<MobController>();

        if (mobController == null)
        {
            Debug.LogError("MobController component not found on the GameObject.", this);
            return;
        }

        // Initialize health from the Character object
        //health = mobController.Character.Health;
        SetTextHealth(Health);
    }

    void Update()
    {
        SetTextHealth(Health);
    }

    public IEnumerator TakeDamage(int amount)
    {
        if (amount == 0) yield break;

        int real_dmg = mobController.Character.TakeDamage(amount);

        MobDamageText damageTextComponent = GetComponent<MobDamageText>();

        if (damageTextComponent == null)
        {
            Debug.LogWarning("DamageTextComponent not found on the mob.");
        }
        else
        {
            Debug.Log($"Taking {real_dmg} damage on {mobData.name}");
            damageTextComponent.ShowDamage(amount);
        }

        //health -= amount;
        SetTextHealth(Math.Max(0, Health));

        // Animation
        isBlinking = true;
        StartCoroutine(BlinkDamageFlash());
        yield return StartCoroutine(HandleBlinkDelay());
    }

    /*public void HealPlayer(int amount)
    {
        health += amount;
        SetTextHealth(health);

        isBlinking = true;
        StartCoroutine(BlinkHealFlash());
        StartCoroutine(HandleBlinkDelay());
    }*/

    public IEnumerator TriggerDie()
    {
        animator.SetTrigger("Death");

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Dead"));
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f);

        gameObject.SetActive(false);
    }

    public IEnumerator TriggerDeathEffect()
    {
        /*if (mobData.role == Role.Bomboblin)
        {
            yield return source.TakeDamage(99);
        }
        else
        {*/
        yield return null;
        //}
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

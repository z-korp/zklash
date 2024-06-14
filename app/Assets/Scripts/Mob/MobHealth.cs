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

    public int Health
    {
        get
        {
            if (mobController == null)
            {
                Debug.LogError("MobController is not set on " + gameObject.name, this);
                return 0;
            }
            return mobController.Character != null ? mobController.Character.Health : 0;
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
            Debug.Log($"Taking {real_dmg} damage");
            damageTextComponent.ShowDamage(amount);
        }

        SetTextHealth(Math.Max(0, Health));

        // Animation
        isBlinking = true;
        StartCoroutine(BlinkDamageFlash());
        yield return StartCoroutine(HandleBlinkDelay());
    }

    public IEnumerator TriggerDie()
    {
        animator.SetTrigger("Death");

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Dead"));
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f);

        // Trigger DeathRattle effects
        //TriggerDeathRattleEffects();

        gameObject.SetActive(false);
    }

    public IEnumerator TriggerDeathEffect()
    {
        yield return null;
    }


    private void TriggerDeathRattleEffects()
    {
        DeathRattleEffect[] deathRattles = GetComponents<DeathRattleEffect>();
        foreach (var effect in deathRattles)
        {
            effect.Trigger(gameObject);
        }
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

    public IEnumerator BlinkPowerUpFlash()
    {
        while (isBlinking)
        {
            spriteRenderer.color = new Color(0f, 0f, 1f, 1f);
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

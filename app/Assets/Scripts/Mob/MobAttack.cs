using System.Collections;
using UnityEngine;
using TMPro;

public class MobAttack : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public MobData mobData;
    private int baseDamage;
    private int bonusDamage = 0;
    private int damage => baseDamage + bonusDamage;

    public TextMeshProUGUI txtAttack;

    [HideInInspector]
    public bool isBlinking = false;
    public float blinkDuration = 0.2f;
    public float blinkTimeAfterHit = 1f;

    [HideInInspector]
    public MobHealth target;

    void Awake()
    {
        baseDamage = mobData.damage;

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

        SetTextAttack(damage);
    }

    void Start()
    {
    }

    public IEnumerator TriggerAttackCoroutine()
    {
        animator.SetTrigger("Attack");

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"));
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8f);

        Debug.Log($"Attacking {target.name} for {damage} damage");
        yield return target.TakeDamage(damage);
    }

    public void TakeDamageOnTarget()
    {
        //target.TakeDamage(damage);
    }

    public void IncreaseDamage(int amount)
    {
        bonusDamage += amount;

        SetTextAttack(damage);

        isBlinking = true;
        StartCoroutine(BlinkPowerUpFlash());
        StartCoroutine(HandleBlinkDelay());
    }

    public void SetTextAttack(int amount)
    {
        txtAttack.text = amount.ToString();
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

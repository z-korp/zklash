using System.Collections;
using UnityEngine;
using TMPro;

public class MobDamage : MonoBehaviour
{    
    public int damage;

    public Animator animator;

    public SpriteRenderer spriteRenderer;
    public TextMeshProUGUI txtAttack;

    public bool isBlinking = false;
    public float blinkDuration = 0.2f;
    public float blinkTimeAfterHit = 1f;

    void Start()
    {
        SetTextAttack(damage);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            DealDamage(1);
        }
    }

    public void DealDamage(int amount)
    {
        animator.SetTrigger("Attack");
    }

    public void IncreaseDamage(int amount)
    {
        damage += amount;
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

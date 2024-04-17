using System.Collections;
using UnityEngine;
using TMPro;

public class ElementData : MonoBehaviour
{
    public uint indexFromShop;
    public int index = -1;
    public string entity = null;

    public int maxHealth = 10;
    public int currentHealth = 3;

    public int currentDamage = 2;
    public int currentXp = 0;
    public int currentLevel = 1;

    public Animator animator;

    public SpriteRenderer spriteRenderer;

    public TextMeshProUGUI txtLife;
    public TextMeshProUGUI txtAttack;

    public bool isBlinking = false;
    public float blinkDuration = 0.2f;
    public float blinkTimeAfterHit = 1f;

    void Start()
    {
        SetTextHealth(currentHealth);
        SetTextAttack(currentDamage);
    }

    void Update()
    {

    }

    public bool TakeDamage(int amount)
    {
        Debug.Log($"TakeDamage: currentHealth: {currentHealth}");
        currentHealth -= amount;
        Debug.Log($"TakeDamage: {amount}, currentHealth: {currentHealth}");

        //healthBar.SetHealth(currentHealth);

        // Verifier si le joueur est mort
        if (currentHealth <= 0)
        {
            //currentHealth = 0;
            SetTextHealth(currentHealth);
            Death();
            //gameObject.SetActive(false);
            return true;
        }
        else
        {
            SetTextHealth(currentHealth);

            isBlinking = true;
            StartCoroutine(BlinkDamageFlash());
            StartCoroutine(HandleBlinkDelay());
            // TBD: Update text from health bar
            //animator.SetTrigger("IsHurt");
        }

        return false;
    }

    public void DealDamage(int amount)
    {
        // TBD: Update text from health bar
        animator.SetTrigger("Attack");
    }

    public void StopAnimAttack()
    {
        animator.SetTrigger("Attack");
    }

    public void PowerUp(int amount)
    {
        currentDamage += amount;
        SetTextAttack(currentDamage);

        isBlinking = true;
        StartCoroutine(BlinkPowerUpFlash());
        StartCoroutine(HandleBlinkDelay());
    }

    public void HealPlayer(int amount)
    {
        if (currentHealth + amount > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth += amount;
        }
        SetTextHealth(currentHealth);

        isBlinking = true;
        StartCoroutine(BlinkHealFlash());
        StartCoroutine(HandleBlinkDelay());
    }

    public void Death()
    {
        animator.SetTrigger("Death");
    }

    public void SetTextHealth(int amount)
    {
        txtLife.text = amount.ToString();
    }

    public void SetTextAttack(int amount)
    {
        txtAttack.text = amount.ToString();
    }

    public void MoveAlly()
    {
        //animator.SetBool("IsWalking", true);
        gameObject.transform.position += new Vector3(2f, 0, 0);
        //animator.SetBool("IsWalking", false);

    }

    public void MoveEnemy()
    {
        //animator.SetBool("IsWalking", true);
        gameObject.transform.position -= new Vector3(2f, 0, 0);
        //animator.SetBool("IsWalking", false);

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

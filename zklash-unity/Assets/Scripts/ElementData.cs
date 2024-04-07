using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ElementData : MonoBehaviour
{
    public uint indexFromShop;
    public int index = -1;
    public string entity;

    public int maxHealth = 10;
    public int currentHealth = 3;

    public int currentDamage = 2;

    public Animator animator;

    public SpriteRenderer spriteRenderer;

    public TextMeshProUGUI txtLife;
    public TextMeshProUGUI txtAttack;

    public GameObject vfxHandler;

    public GameObject canvas;

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
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(2);
            if (gameObject.CompareTag("Enemy"))
            {
                vfxHandler.GetComponent<VfxManager>().PlayExplodeVFX();
            }
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            HealPlayer(1);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            DealDamage(1);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            PowerUp(1);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        //healthBar.SetHealth(currentHealth);

        // Verifier si le joueur est mort
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            SetTextHealth(currentHealth);
            Death();
            //gameObject.SetActive(false);
            return;
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
        canvas.SetActive(false);
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
        animator.SetBool("IsWalking", true);
        gameObject.transform.position += new Vector3(2f, 0, 0);
        animator.SetBool("IsWalking", false);

    }

    public void MoveEnemy()
    {
        animator.SetBool("IsWalking", true);
        gameObject.transform.position -= new Vector3(2f, 0, 0);
        animator.SetBool("IsWalking", false);

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

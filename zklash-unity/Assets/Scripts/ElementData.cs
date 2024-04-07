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

    public Animator animator;

    public SpriteRenderer spriteRenderer;

    public TextMeshProUGUI txtLife;

    public GameObject vfxHandler;

    public bool isInvicible = false;
    public float invicibilityBlinkDuration = 0.2f;
    public float invicibilityTimeAfterHit = 2f;



    void Start()
    {
        if (gameObject.CompareTag("Enemy"))
        {
            SetTextHealth(currentHealth);
        }
        else
        {

        }
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
            if (gameObject.CompareTag("Enemy"))
            {
                SetTextHealth(currentHealth);
            }
            isInvicible = true;
            StartCoroutine(InvincibilityFlash());
            StartCoroutine(HandleInvincibilityDelay());
            // TBD: Update text from health bar
            //animator.SetTrigger("IsHurt");
        }
    }

    public IEnumerator InvincibilityFlash()
    {
        while (isInvicible)
        {
            spriteRenderer.color = new Color(1f, 0f, 0f, 1f);
            yield return new WaitForSeconds(invicibilityBlinkDuration);
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(invicibilityBlinkDuration);
        }
    }

    public IEnumerator HandleInvincibilityDelay()
    {
        yield return new WaitForSeconds(invicibilityTimeAfterHit);
        isInvicible = false;
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

    }

    public void Death()
    {
        animator.SetTrigger("Death");
    }

    public void SetTextHealth(int amount)
    {
        txtLife.text = amount.ToString();
    }
}

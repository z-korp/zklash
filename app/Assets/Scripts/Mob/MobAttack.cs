using System.Collections;
using UnityEngine;
using TMPro;
using zKlash.Game.Roles;

public class MobAttack : MonoBehaviour
{
    private MobController mobController;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private int _attack;

    public int Attack
    {
        get
        {
            if (mobController == null)
            {
                Debug.LogError("MobController is not set on " + gameObject.name, this);
                return 0;
            }

            return mobController.Character != null ? mobController.Character.Attack : 0;
        }
        private set
        {
            _attack = value;
        }
    }

    public TextMeshProUGUI txtAttack;

    [HideInInspector]
    public bool isBlinking = false;
    public float blinkDuration = 0.2f;
    public float blinkTimeAfterHit = 1f;

    [HideInInspector]
    public MobHealth target;

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

        SetTextAttack(Attack);
    }

    void Update()
    {
        SetTextAttack(Attack);
    }

    public IEnumerator TriggerAttackCoroutine(int dmg)
    {
        animator.SetTrigger("Attack");

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"));
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8f);

        Debug.Log($"Attacking {target.name} for {dmg} damage");
        yield return target.TakeDamage(dmg);
    }

    public IEnumerator TriggerPostMortemCoroutine(int dmg)
    {
        if (GetComponent<MobController>().Character.RoleInterface == Role.Bomboblin) // TBD standardize the PostMortem animation
        {
            animator.SetTrigger("PostMortem");

            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("PostMortem"));
            yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8f);
        }

        Debug.Log($"PostMorteming {target.name} for {dmg} damage, target has {target.Health} health left.");
        yield return target.TakeDamage(dmg);
    }

    public void TakeDamageOnTarget()
    {
        //target.TakeDamage(damage);
    }

    public void SetTextAttack(int amount)
    {
        txtAttack.text = amount.ToString();
    }

    public IEnumerator BlinkPowerUp()
    {
        float blinkDuration = 0.2f;
        spriteRenderer.color = new Color(0f, 0f, 1f, 1f);
        yield return new WaitForSeconds(blinkDuration);
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(blinkDuration);
        spriteRenderer.color = new Color(0f, 0f, 1f, 1f);
        yield return new WaitForSeconds(blinkDuration);
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(blinkDuration);
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

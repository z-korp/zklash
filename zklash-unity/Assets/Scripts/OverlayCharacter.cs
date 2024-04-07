using UnityEngine;
using System.Collections; // Assurez-vous d'avoir TextMesh Pro dans votre projet si vous utilisez cette ligne

public class OverlayCharacter : MonoBehaviour
{
    public Transform healthBarGreen; // Assurez-vous que c'est le Transform de l'objet avec le Sprite Renderer
    public Transform healthBarBlack;

    private float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
        StartCoroutine(DamageOverTimeCoroutine());
    }

    private IEnumerator DamageOverTimeCoroutine()
    {
        while (true)
        {
            TakeDamage(10);
            yield return new WaitForSeconds(2);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        //UpdateHealthBar();
    }


    private void UpdateHealthBar()
    {
        float healthRatio = currentHealth / maxHealth;

        float originalWidth = healthBarGreen.GetComponent<SpriteRenderer>().sprite.bounds.size.x * healthBarGreen.lossyScale.x;

        healthBarGreen.localScale = new Vector3(healthRatio, 1f, 1f);

        float widthDifference = originalWidth - (healthBarGreen.GetComponent<SpriteRenderer>().sprite.bounds.size.x * healthBarGreen.lossyScale.x);

        healthBarGreen.position = new Vector3(healthBarGreen.position.x - widthDifference / 2, healthBarGreen.position.y, healthBarGreen.position.z);
    }
}
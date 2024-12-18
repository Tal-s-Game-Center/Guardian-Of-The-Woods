using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HPBar : MonoBehaviour
{
    public Image hpBarForeground;    // The foreground Image of the HP bar
    public Text hpText;              // Text to display current HP / max HP
    public float maxHP = 100f;       // Maximum health
    private float currentHP;
    public float lowHealthThreshold = 30f; // HP threshold for low health visual effect
    public Color lowHealthColor = Color.red; // Color to flash when HP is low
    public Color normalHealthColor = Color.green; // Normal color of the HP bar

    private bool isLowHealth = false;

    private void Start()
    {
        currentHP = maxHP; // Start with full health
        UpdateHPBar();     // Initial update of the HP bar
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        if (currentHP < 0) currentHP = 0; // Prevent HP from going below 0
        UpdateHPBar();
        CheckLowHealth();
    }

    public void Heal(float amount)
    {
        currentHP += amount;
        if (currentHP > maxHP) currentHP = maxHP; // Prevent HP from exceeding max
        UpdateHPBar();
        CheckLowHealth();
    }

    private void UpdateHPBar()
    {
        // Update the fill amount based on the percentage of remaining health
        hpBarForeground.fillAmount = currentHP / maxHP;

        // Update the text to display the current and max HP
        hpText.text = $"{currentHP}/{maxHP}";

        // Flash when HP is low
        if (isLowHealth)
        {
            hpBarForeground.color = lowHealthColor;
        }
        else
        {
            hpBarForeground.color = normalHealthColor;
        }
    }

    // Coroutine for smooth health change animation
    public void SmoothHPChange(float targetHP, float duration)
    {
        StartCoroutine(SmoothHPChangeCoroutine(targetHP, duration));
    }

    private IEnumerator SmoothHPChangeCoroutine(float targetHP, float duration)
    {
        float startHP = currentHP;
        float timeElapsed = 0f;
        
        while (timeElapsed < duration)
        {
            currentHP = Mathf.Lerp(startHP, targetHP, timeElapsed / duration);
            UpdateHPBar();
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        currentHP = targetHP;
        UpdateHPBar();
    }

    private void CheckLowHealth()
    {
        if (currentHP <= lowHealthThreshold)
        {
            if (!isLowHealth)
            {
                isLowHealth = true;
                StartCoroutine(FlashLowHealthEffect()); // Trigger flash effect
            }
        }
        else
        {
            if (isLowHealth)
            {
                isLowHealth = false;
                StopCoroutine(FlashLowHealthEffect()); // Stop the flashing effect when HP is no longer low
                hpBarForeground.color = normalHealthColor; // Set back to normal color
            }
        }
    }

    private IEnumerator FlashLowHealthEffect()
    {
        while (isLowHealth)
        {
            hpBarForeground.color = lowHealthColor;
            yield return new WaitForSeconds(0.5f);
            hpBarForeground.color = normalHealthColor;
            yield return new WaitForSeconds(0.5f);
        }
    }

    // Optional: Draw detection radius in the scene view for debugging
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 5f);
    }
}

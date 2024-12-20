using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public Slider healthSlider;
    public Text healthText;
    public Color healthyColor;
    public Color injuredColor;

    int currentHealth;
    LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
        healthSlider.value = currentHealth;
        healthText.text = currentHealth.ToString();
        healthText.color = healthyColor;
        levelManager = GameObject.FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damageAmt)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damageAmt;
            healthSlider.value = currentHealth;
            healthText.text = currentHealth.ToString();
        }

        if (currentHealth < startingHealth/2)
        {
            healthText.color = injuredColor;
        }

        if (currentHealth <= 0)
        {
            PlayerDies();
            levelManager.LevelLost();
        }
    }

    void PlayerDies()
    {
        Debug.Log("Player dead");
        transform.Rotate(-90, 0, 0, Space.Self);
    }

    public void Heal(int healthAmt)
    {
        if (currentHealth < 100)
        {
            currentHealth += healthAmt;
            healthSlider.value = Mathf.Clamp(currentHealth, 0, 100);
        }
    }
}

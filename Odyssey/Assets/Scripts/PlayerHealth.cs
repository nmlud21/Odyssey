using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public EnemyController enemy;
    public HealthBar healthBar;

    public float maxHealth = 100;
    public float currentHealth;
    private float regenDelay = 5f;
    public float regenSpeed = 1f;
    private float timeSinceLastCall;

    private bool isRegenActive;

    // Start is called before the first frame update
    void Start()
    {
        enemy.GetComponent<EnemyController>();
        healthBar.SetMaxHealth(maxHealth);
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastCall += Time.deltaTime;
        
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if (currentHealth < maxHealth && timeSinceLastCall >= regenDelay)
        {
            if (!isRegenActive)
            {
                InvokeRepeating(nameof(HealthRegen), .1f, regenSpeed);
                isRegenActive = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TakePlayerDamage(enemy.enemyDamage);
        Destroy(other.gameObject);
    }

    public void TakePlayerDamage(float amount)
    {
        if (isRegenActive)
        {
            CancelInvoke(nameof(HealthRegen));
        }
        timeSinceLastCall = 0;
        isRegenActive = false;
        currentHealth -= amount;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            PlayerDeath();
        }
    }

    void PlayerDeath()
    {
        Destroy(gameObject);
    }

    void HealthRegen()
    {
        currentHealth += 20;
        healthBar.SetHealth(currentHealth);
    }
    
    // IEnumerator HealthRegen()
    // {
    //      isRegenActive = true;
    //     // yield return new WaitForSeconds(regenDelay);
    //
    //     
    //     while (currentHealth < maxHealth)
    //     {
    //         //Start the regen cycle
    //         Debug.Log("Success");
    //         currentHealth += 20;
    //         healthBar.SetHealth(currentHealth);
    //         yield return new WaitForSeconds(regenSpeed); //Wait for regen speed
    //     }
    //
    //     isRegenActive = false;
    // }
}
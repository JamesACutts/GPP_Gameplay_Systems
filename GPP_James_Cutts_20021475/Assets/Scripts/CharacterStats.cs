using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] 
    protected int maxHealth;
    [SerializeField]
    protected int health;
    [SerializeField]
    protected bool isDead;
    public float attackSpeed;

    private void Start()
    {
        // Initialize the variables for this object.
        InitVariables();
    }
    public virtual void CheckHealth()
    {
        if(health <= 0 )
        {
            health = 0;
            Die();
        }
        if(health >= maxHealth)
        {
            health = maxHealth;
        }
    }

    public virtual void Die()
    {
        isDead = true;
    }

    public void SetHealthTo(int healthToSetTo)
    {
        health = healthToSetTo;
        CheckHealth();
    }

    public void TakeDamage(int damage)
    {
        int healthAfterDamage = health - damage;
        SetHealthTo(healthAfterDamage);
    }

    public void Heal(int heal)
    {
        int healthAfterHeal = health + heal;
        SetHealthTo(healthAfterHeal);
    }

    public virtual void InitVariables()
    {
        maxHealth = 100; // Set the maximum health to 100.
        SetHealthTo(maxHealth); // Set the object's health to the maximum health.
        isDead = false; // Set the isDead flag to false to indicate that the object is not dead.
        attackSpeed = 0.5f; // Set the object's attack speed to 0.5f.
    }
}

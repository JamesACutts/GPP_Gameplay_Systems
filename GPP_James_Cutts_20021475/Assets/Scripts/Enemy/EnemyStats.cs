using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    [SerializeField]
    private int damage;
    enum slimeSizes
    {
        Large,
        Mid,
        Small
    }
    slimeSizes slimeSize;
    private void Start()
    {
        InitVariables();
    }
    public void DealDamage(CharacterStats statsToDamage)
    {
        statsToDamage.TakeDamage(damage);
    }

    public override void Die()
    {
        base.Die();
        Split();
    }
    void Split()
    {
        GameObject slimeSplit = gameObject;
        
        if (slimeSize == slimeSizes.Large)
        {
            MidSlime(slimeSplit);
            MidSlime(slimeSplit);
        }
        else if (slimeSize == slimeSizes.Mid)
        {
            SmallSlime(slimeSplit);
            SmallSlime(slimeSplit);
            SmallSlime(slimeSplit);
        }
        Destroy(gameObject);
    }
    void MidSlime(GameObject slime)
    {
        slime = Instantiate(slime);
        slime.GetComponent<EnemyStats>().slimeSize = slimeSizes.Mid;
        slime.GetComponent<EnemyStats>().maxHealth = 20;
        slime.GetComponent<EnemyController>().lookRadius = 8;
        slime.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }
    void SmallSlime(GameObject slime)
    {
        slime = Instantiate(slime);
        slime.GetComponent<EnemyStats>().slimeSize = slimeSizes.Small;
        slime.GetComponent<EnemyStats>().maxHealth = 10;
        slime.GetComponent<EnemyController>().lookRadius = 6;
        slime.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
    }
    public override void InitVariables()
    {
        SetHealthTo(maxHealth);
        isDead = false;
        damage = 2;
        attackSpeed = 1.2f;
    }

}

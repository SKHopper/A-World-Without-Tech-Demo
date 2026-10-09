using System.Collections;
using UnityEngine;

public class CombatComponent : MonoBehaviour
{
    bool canAttack = true;
    [SerializeField] float attackCooldown;
    [SerializeField] float maxHealth;
    float health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
    }

    void Die()
    {
        //die
    }

    public float ChangeHealth(float delta)
    {
        health += delta;
        Mathf.Clamp(health, 0, maxHealth);
        if (health == 0)
        {
            Die(); 
        }
        return health;
    }

    public float GetHealth()
    {
        return health;
    }

    public bool GetCanAttack()
    {
        return canAttack;
    }

    private IEnumerator doAttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    public void Attack(CombatComponent recipient, float damage)
    {
        if (canAttack)
        {
            recipient.ChangeHealth(-damage);
            StartCoroutine(doAttackCooldown());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

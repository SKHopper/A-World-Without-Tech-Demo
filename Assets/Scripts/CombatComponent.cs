using System.Collections;
using UnityEngine;

public class CombatComponent : MonoBehaviour, ICombat
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

    //heal or damage effect
    public float ChangeHealth(float delta)
    {
        health = Mathf.Clamp(health + delta, 0, maxHealth);
        if (health == 0)
        {
            Die(); 
        }
        Debug.Log("Health: " + health);
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

    //safe damage gameObject
    public bool Attack(GameObject recipient, float damage)
    {
        bool attacked = false;

        if (canAttack)
        {
            
            if (recipient.TryGetComponent(out ICombat other))
            {
                other.ChangeHealth(-damage);
                StartCoroutine(doAttackCooldown());
                attacked = true;
            }

        }

        return attacked;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

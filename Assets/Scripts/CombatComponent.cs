using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CombatComponent : MonoBehaviour, ISliderUser
{
    bool canAttack = true;
    [SerializeField] float attackCooldown;
    [SerializeField] float maxHealth;
    float health;
    ICombat parent;
    ISlider slider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parent = GetComponent<ICombat>();
        health = maxHealth;
    }

    //heal or damage effect
    public float ChangeHealth(float delta)
    {
        health = Mathf.Clamp(health + delta, 0, maxHealth);
        parent.HealthChanged(health);
        slider.UpdateBar(health);
        return health;
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public bool GetCanAttack()
    {
        return canAttack;
    }

    public string Name() { return "HealthSlider"; }

    public void Setup(ISlider sliderComponent)
    {
        slider = sliderComponent;
        slider.Setup(maxHealth, maxHealth);
    }

    private IEnumerator DoAttackCooldown()
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
            
            if (recipient.TryGetComponent(out CombatComponent other))
            {
                other.ChangeHealth(-damage);
                StartCoroutine(DoAttackCooldown());
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

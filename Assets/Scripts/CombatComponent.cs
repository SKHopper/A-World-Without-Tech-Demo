using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class CombatComponent : MonoBehaviour, ISliderUser
{
    bool canAttack = true;
    [SerializeField] float attackCooldown;
    [SerializeField] float maxHealth;
    float health;
    ICombat parent;
    ISlider slider;
    //incoming damage mult
    float vulnerability = 1;
    //slider default colour
    Color fillDefault;
    [SerializeField] Color fillVulnerable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parent = GetComponent<ICombat>();
        health = maxHealth;
    }

    //heal or damage effect
    public float ChangeHealth(float delta)
    {
        float nDelta = (delta < 0) ? delta * vulnerability : delta;
        health = Mathf.Clamp(health + nDelta, 0, maxHealth);
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
        fillDefault = slider.GetFill();
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

    //become vulnerable for duration
    public IEnumerator ToggleVulnerability(float dmgMult, float duration)
    {
        vulnerability = dmgMult;
        slider.SetFill(fillVulnerable);

        yield return new WaitForSeconds(duration);

        vulnerability = 1;
        slider.SetFill(fillDefault);
    }
    
    //apply vulnerability for duration
    public void ApplyVulnerability(GameObject recipient, float dmgMult, float duration)
    {
        if (vulnerability == 1 && recipient.TryGetComponent(out CombatComponent other))
        {
            StartCoroutine(other.ToggleVulnerability(dmgMult, duration));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

//health and damage store and transfer, including vulnerability (dmg mult) condition
//can be given slider to update
public class CombatComponent : MonoBehaviour, ISliderUser
{
    [SerializeField] float attackCooldown;
    [SerializeField] float maxHealth;
    [SerializeField] bool canBeVulnerable;
    [SerializeField] Color fillVulnerable;

    ICombat parent;
    ISlider slider;

    //incoming damage mult
    float vulnerability = 1;
    float health;
    bool canAttack = true;
    bool hasSlider = false;
    //slider default colour
    Color fillDefault;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parent = GetComponent<ICombat>();
        health = maxHealth;
    }

    //heal or damage effect on This
    public float ChangeHealth(float delta)
    {
        float nDelta = (delta < 0) ? delta * vulnerability : delta;
        health = Mathf.Clamp(health + nDelta, 0, maxHealth);
        parent.HealthChanged(health);
        if (hasSlider) slider.UpdateBar(health);
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

    //override ISliderUser
    public string Name() { return "HealthSlider"; }

    //set initial values for slider
    public void Setup(ISlider sliderComponent)
    {
        hasSlider = true;
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

    //safe damage other gameObject's comp
    public bool Attack(GameObject recipient, float damage)
    {
        bool attacked = false;

        if (canAttack && recipient.TryGetComponent(out CombatComponent other))
        {
            other.ChangeHealth(-damage);
            StartCoroutine(DoAttackCooldown());
            attacked = true;
        }

        return attacked;
    }

    //become vulnerable for duration
    IEnumerator ToggleVulnerability(float dmgMult, float duration)
    {
        vulnerability = dmgMult;
        if (hasSlider) slider.SetFill(fillVulnerable);

        yield return new WaitForSeconds(duration);

        vulnerability = 1;
        if (hasSlider) slider.SetFill(fillDefault);
    }
    
    //apply vulnerability for duration
    public void ApplyVulnerability(float dmgMult, float duration)
    {
        if (canBeVulnerable && vulnerability == 1) StartCoroutine(ToggleVulnerability(dmgMult, duration));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

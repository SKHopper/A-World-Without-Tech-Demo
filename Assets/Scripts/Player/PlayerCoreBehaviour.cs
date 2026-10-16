using System;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//core between player component scripts
//inc. main UI handler
public class PlayerCoreBehaviour : MonoBehaviour, ICombat
{
    CombatComponent combat;
    OxygenComponent oxygen;
    PlayerMovement movement;
    //UI
    GameObject hud;
    ISlider healthSlider;
    DamageEffect dmgFx;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        combat = GetComponent<CombatComponent>();
        oxygen = GetComponent<OxygenComponent>();
        movement = GetComponent<PlayerMovement>();
        //as the FirstPersonController is prefab, player cannot access hard ref before runtime
        hud = GameObject.FindGameObjectWithTag("Hud");
        dmgFx = hud.GetComponentInChildren<DamageEffect>();

        foreach (Slider slider in hud.GetComponentsInChildren<Slider>())
        {
            //setup each HUD slider
            //also runs other setup for components here to order execution
            ISlider compIF = slider.GetComponent<ISlider>();
            switch (slider.name)
            {
                case "HealthSlider":

                    healthSlider = compIF;
                    healthSlider.Setup(combat.GetMaxHealth(), combat.GetMaxHealth());
                    break;

                case "OxygenSlider":

                    oxygen.Setup(compIF);
                    break;

                case "DashSlider":

                    movement.Setup(compIF);
                    break;

                default:
                    break;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ICombat
    public void HealthChanged(float health) 
    {
        healthSlider.UpdateBar(health);
        dmgFx.StartEffect();
    }
}

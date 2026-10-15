using System;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//core between player component scripts
public class PlayerCoreBehaviour : MonoBehaviour, ICombat
{
    //set in inspector
    GameObject hud;
    SliderComp healthSlider;
    CombatComponent combat;
    OxygenComponent oxygen;
    PlayerMovement movement;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        combat = GetComponent<CombatComponent>();
        oxygen = GetComponent<OxygenComponent>();
        movement = GetComponent<PlayerMovement>();

        //as the FirstPersonController is prefab, player cannot access hard ref before runtime
        hud = GameObject.FindGameObjectWithTag("Hud");
        foreach (Slider slider in hud.GetComponentsInChildren<Slider>())
        {
            //setup each HUD slider
            //also runs other setup for components here to order execution
            SliderComp comp = slider.GetComponent<SliderComp>();
            switch (slider.name)
            {
                case "HealthSlider":

                    healthSlider = comp;
                    healthSlider.Setup(combat.GetMaxHealth(), combat.GetMaxHealth());
                    break;

                case "OxygenSlider":

                    oxygen.Setup(comp);
                    break;

                case "DashSlider":

                    movement.Setup(comp);
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
    }
}

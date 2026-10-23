using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//core between player component scripts
//inc. main UI handler
public class PlayerCoreBehaviour : MonoBehaviour, ICombat
{
    //UI
    GameObject hud;
    DamageEffect dmgFx;
    SortedDictionary<string, ISliderUser> sliderUserMap = new SortedDictionary<string, ISliderUser>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //as the FirstPersonController is prefab, player cannot access hard ref before runtime
        hud = GameObject.FindGameObjectWithTag("Hud");
        dmgFx = hud.GetComponentInChildren<DamageEffect>();

        //hand over each HUD slide bar to controlling components
        //includes: HealthSlider, BossBarSlider, DashSlider, OxygenSlider
        ISliderUser[] sliderUsers = GetComponents<ISliderUser>();

        foreach (ISliderUser user in sliderUsers) 
            sliderUserMap.Add(user.Name(), user);

        foreach (Slider slider in hud.GetComponentsInChildren<Slider>()) 
            sliderUserMap[slider.name].Setup(slider.GetComponent<ISlider>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ICombat
    public void HealthChanged(float health) 
    {
        dmgFx.StartEffect();
    }
}

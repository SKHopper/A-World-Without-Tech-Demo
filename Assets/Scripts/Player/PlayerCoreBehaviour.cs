using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//core between player component scripts
//inc. main UI connector
public class PlayerCoreBehaviour : MonoBehaviour, ICombat
{
    //UI
    [SerializeField] public GameObject hud = null;

    DamageEffect dmgFx;
    public IGameState gameState;

    SortedDictionary<string, ISliderUser> sliderUserMap = new SortedDictionary<string, ISliderUser>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (hud == null) return;
        dmgFx = hud.GetComponentInChildren<DamageEffect>();

        //hand over each HUD slide bar to controlling components
        //inc string names: HealthSlider, BossBarSlider, DashSlider, OxygenSlider
        foreach (ISliderUser user in GetComponents<ISliderUser>()) sliderUserMap.Add(user.Name(), user);
        foreach (Slider slider in hud.GetComponentsInChildren<Slider>()) sliderUserMap[slider.name].Setup(slider.GetComponent<ISlider>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //override ICombat
    public void HealthChanged(float health) 
    {
        dmgFx.StartEffect();

        if (health == 0)
        {
            gameState.ReportDeath(this.gameObject, DeathType.zeroHealth);
        }
    }
}

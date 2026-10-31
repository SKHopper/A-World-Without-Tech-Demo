using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//core between player component scripts
//inc. main UI connector
public class PlayerCoreBehaviour : MonoBehaviour, ICombat, IPlayerCoreInput
{
    //UI
    [SerializeField] GameObject hud = null;
    [SerializeField] GameObject camera;
    [SerializeField] LayerMask interactable;
    [SerializeField] float interactDistance;

    DamageEffect dmgFx;
    public IGameState gameState;
    FuelRewardComponent fuelSystem;

    SortedDictionary<string, ISliderUser> sliderUserMap = new SortedDictionary<string, ISliderUser>();
    bool canInteract = true;
    float interactCooldown = .5f;


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

    IEnumerator WaitInteractCooldown()
    {
        canInteract = false;
        yield return new WaitForSeconds(interactCooldown);
        canInteract = true;
    }

    //override IPlayerCoreInput
    //E-to-Interact with furnace obj (to deposite fuel)
    public void OnInteract()
    {
        if (!canInteract) return;

        WaitInteractCooldown();
        
        if (Physics.Linecast(
                camera.transform.position,
                camera.transform.position + (camera.transform.forward * interactDistance),
                out RaycastHit hit,
                interactable
        ))
        {
            fuelSystem.PlayerInteractFurnace();
        }


    }
}

using System.Collections;
using TMPro;
using UnityEngine;

public class FuelRewardComponent : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countText;
    [SerializeField] TextMeshProUGUI gainNotifText;
    [SerializeField] FurnaceBehaviour furnace;
    [SerializeField] float notifScreentime = 2;
    [SerializeField] int killFuelReward = 2;

    int fuel = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    public void PlayerInteractFurnace()
    {
        if (fuel > 0 && !furnace.InUse) 
        {
            furnace.Burn();
        }
    }

    //set fuel gain hud notif text
    IEnumerator NotifyChange(int delta)
    {
        gainNotifText.text = "+" + delta.ToString() + " FUEL";
        yield return new WaitForSeconds(notifScreentime);
        gainNotifText.text = "";
    }

    //give player fuel (for killing enemy)
    public void Reward()
    {
        ChangeFuel(killFuelReward);
    }

    void ChangeFuel(int delta)
    {
        fuel = Mathf.Clamp(fuel + delta, 0, int.MaxValue);
        countText.text = fuel.ToString();
        StartCoroutine(NotifyChange(delta));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

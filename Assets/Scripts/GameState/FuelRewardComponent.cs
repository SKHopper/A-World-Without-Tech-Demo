using TMPro;
using UnityEngine;

public class FuelRewardComponent : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countText;
    [SerializeField] FurnaceBehaviour furnace;

    int fuel = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChangeFuel(2);
    }

    public void PlayerInteractFurnace()
    {

    }

    void ChangeFuel(int delta)
    {
        fuel = Mathf.Clamp(fuel + delta, 0, int.MaxValue);
        countText.text = fuel.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

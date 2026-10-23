using StarterAssets;
using System.Collections;
using UnityEngine;

//tracker for player oxygen and oxygen abilities
public class OxygenComponent : MonoBehaviour, ISliderUser
{
    [Header("Base Values")]
    [SerializeField] float initialOxygen;
    [SerializeField] float maxOxygen;
    //per second
    [SerializeField] float passiveLossOxygen;
    float oxygen;
    ISlider slider;

    void Start() {}

    //set initial values with slider
    public void Setup(ISlider newSlider)
    {
        oxygen = initialOxygen;
        slider = newSlider;

        slider.Setup(maxOxygen, oxygen);
    }

    public string Name() { return "OxygenSlider"; }

    public float ChangeOxygen(float delta)
    {
        return oxygen = Mathf.Clamp(oxygen, 0, maxOxygen);
    }

    //the player loses oxygen at a steady rate passively
    IEnumerator OxygenTick()
    {
        while (true) {
            yield return new WaitForSeconds(1);

        }
    }
}

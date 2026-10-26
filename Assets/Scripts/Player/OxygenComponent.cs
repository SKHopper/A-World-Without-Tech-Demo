using StarterAssets;
using System.Collections;
using UnityEngine;

//tracker for player oxygen
//stores current with passive loss/time
public class OxygenComponent : MonoBehaviour, ISliderUser
{
    [Header("Base Values")]
    [SerializeField] float initialOxygen;
    [SerializeField] float maxOxygen;
    //per second
    [SerializeField] float passiveLossOxygen;

    //seconds
    float oxygenTickPeriod = 0.1f;
    float oxygen;
    bool doOxygenTick;

    ISlider slider;

    void Start() {}

    //set initial values with slider
    public void Setup(ISlider newSlider)
    {
        oxygen = initialOxygen;
        slider = newSlider;

        slider.Setup(maxOxygen, oxygen);
        StartOxygenTick();
    }

    public void StartOxygenTick()
    {
        doOxygenTick = true;
        StartCoroutine(OxygenTick());
    }

    public void EndOxygenTick()
    {
        doOxygenTick = false;
    }

    public string Name() { return "OxygenSlider"; }

    public float ChangeOxygen(float delta)
    {
        oxygen = Mathf.Clamp(oxygen + delta, 0, maxOxygen);
        slider.UpdateBar(oxygen);
        return oxygen;
    }

    //the player loses oxygen at a steady rate passively
    IEnumerator OxygenTick()
    {
        while (doOxygenTick) {

            yield return new WaitForSeconds(oxygenTickPeriod);
            ChangeOxygen(-passiveLossOxygen * oxygenTickPeriod);
        }
    }
}

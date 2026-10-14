using UnityEngine;
using UnityEngine.UI;

public class SliderComp : MonoBehaviour
{
    //used to turn values into percentages
    float percentileValue;
    Slider slider;

    //set initial values
    public void Setup(float newMax, float current)
    {
        percentileValue = 100 / newMax;
        slider = GetComponent<Slider>();
        UpdateBar(current);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void UpdateBar(float health)
    {
        slider.value = health * percentileValue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

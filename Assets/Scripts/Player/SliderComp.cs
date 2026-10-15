using System.Reflection.Metadata.Ecma335;
using UnityEngine;
using UnityEngine.UI;

public class SliderComp : MonoBehaviour
{
    //used to turn values into percentages
    float percentileValue;
    Vector3 defaultSize;
    Slider slider;

    //set initial values
    public void Setup(float newMax, float current)
    {
        percentileValue = 100 / newMax;
        slider = GetComponent<Slider>();
        defaultSize = slider.transform.localScale;
        UpdateBar(current);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    //set visibility of slider on hud
    public void Toggle(bool active)
    {
        slider.transform.localScale = active ? defaultSize : Vector3.zero;
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

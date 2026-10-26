using System.Reflection.Metadata.Ecma335;
using UnityEngine;
using UnityEngine.UI;

//controller for UI elem of type Slider, between relevant component and hud elem
public class SliderComp : MonoBehaviour, ISlider
{
    //used to turn values into percentages
    float percentileValue;
    Vector3 defaultSize;

    Slider slider;
    Image sliderFill;

    //set initial values
    //reset slider
    public void Setup(float newMax, float current)
    {
        percentileValue = 100 / newMax;
        slider = GetComponent<Slider>();
        sliderFill = slider.transform.GetChild(1).GetComponentInChildren<Image>();
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

    //set slider value
    public void UpdateBar(float health)
    {
        slider.value = health * percentileValue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Color GetFill()
    {
        return sliderFill.color;
    }

    public void SetFill(Color fill)
    {
        sliderFill.color = fill;
    }
}

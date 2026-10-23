using UnityEngine;

public interface ISlider
{
    //set new value
    public void UpdateBar(float health);
    //set initial values
    public void Setup(float newMax, float current);
    //set visibility of slider on hud
    public void Toggle(bool active);
    public void SetFill(Color fill);
    public Color GetFill();
}

using UnityEngine;

//a slider user is a component that will control a dedicated on-screen slider bar
public interface ISliderUser
{
    public void Setup(ISlider sliderComponent);
    public string Name();
}

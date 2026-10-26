using UnityEngine;

//healthbar for first enemy of tag "Enemy" found in game
//essentially a connector between the enemy's CombatComponent and the hud's SliderComponent
public class BossBar : MonoBehaviour, ISliderUser
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup(ISlider sliderComp)
    {
        //ISliderUser will be CombatComponent
        GameObject.FindGameObjectWithTag("Enemy").GetComponent<ISliderUser>().Setup(sliderComp);
    }

    public string Name() { return "BossBarSlider"; }
}

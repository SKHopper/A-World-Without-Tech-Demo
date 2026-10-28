using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//UI handler for death screen
public class DeathScreen : MonoBehaviour
{
    [SerializeField] Image deathImage;
    [SerializeField] Button deathButton;

    public IGameState gameState;

    TextMeshProUGUI deathText;
    Image deathFill;

    bool active;
    Color activeColour;
    Color activeBackground;
    Color deactiveColour = new Color(0, 0, 0, 0);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameState = GetComponent<IGameState>();
        deathText = deathButton.GetComponentInChildren<TextMeshProUGUI>();
        deathFill = deathButton.GetComponent<Image>();
        activeColour = deathFill.color;
        activeBackground = deathImage.color;
        Toggle(false);
    }

    public bool GetActive() { return active; }

    public void Toggle(bool isActive, string text = "")
    {
        active = isActive;
        deathButton.interactable = active;

        if (active)
        {
            deathText.text = text;
            deathFill.color = activeColour;
            deathImage.color = activeBackground;
        }
        else
        {
            deathText.text = "";
            deathFill.color = deactiveColour;
            deathImage.color = deactiveColour;
        }
    }

    //on deathbutton press
    public void OnClick()
    {
        gameState.ReportQuit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

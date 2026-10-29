using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialWidget : MonoBehaviour
{

    [SerializeField] Button tutorialButton;
    [SerializeField] Image buttonFill;
    [SerializeField] TextMeshProUGUI tutorialText;
    [SerializeField] TextMeshProUGUI buttonText;
    [SerializeField] string[] tutorialLines;
    [SerializeField] TextMeshProUGUI startText;
    [SerializeField] Image startFill;
    [SerializeField] Button startButton;

    public IGameState mainMenu;

    Color invis;
    Color buttonColour;
    int tutorialIndex;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttonColour = buttonFill.color;
        invis = new Color(0, 0, 0, 0);

        //deactivate
        buttonColour = buttonFill.color;
        buttonFill.color = invis;
        tutorialButton.interactable = false;
        buttonText.text = tutorialText.text = "";
    }

    //cycle through text lines on click until entre game
    public void OnNext()
    {
        if (tutorialIndex >= tutorialLines.Count())
        {
            mainMenu.ReportQuit();
        }
        else
        {
            tutorialText.text = tutorialLines[tutorialIndex++];
        }
    }

    //activate and start cycle
    //OnClick of startbutton
    public void StartTutorial()
    {
        buttonFill.color = buttonColour;
        tutorialButton.interactable = true;
        buttonText.text = "Next";

        startFill.color = invis;
        startText.text = "";
        startButton.interactable = false;

        tutorialIndex = 0;
        OnNext();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

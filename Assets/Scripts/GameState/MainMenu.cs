using UnityEngine;
using UnityEngine.SceneManagement;

//behaviour for main menu ui/game state
public class MainMenu : MonoBehaviour, IGameState
{
    [SerializeField] PlayerCoreBehaviour player;

    TutorialWidget tutorialWidget;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tutorialWidget = GetComponent<TutorialWidget>();
        tutorialWidget.mainMenu = this;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    //override IGameState

    //entre game
    public void ReportQuit()
    {
        SceneManager.LoadScene("AWorldWithout");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

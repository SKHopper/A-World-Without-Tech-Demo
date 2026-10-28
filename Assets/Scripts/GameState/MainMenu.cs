using UnityEngine;
using UnityEngine.SceneManagement;

//behaviour for main menu ui/game state
public class MainMenu : MonoBehaviour
{
    [SerializeField] PlayerCoreBehaviour player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OnClickStartGame()
    {
        SceneManager.LoadScene("AWorldWithout");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

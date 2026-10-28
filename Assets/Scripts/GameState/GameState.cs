using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//manager for game state (win/loss/mode)
//this manager is exposed to many types, defer to its interface for communication
public class GameState : MonoBehaviour, IGameState
{
    [SerializeField] PlayerCoreBehaviour player;
    [SerializeField] EnemyAIController enemyOriginal;
    [SerializeField] string menuScene;

    FirstPersonController playerCtrl;
    DeathScreen deathScreen;

    SortedDictionary<DeathType, string> deathMessage = new SortedDictionary<DeathType, string>() 
    {
        { DeathType.zeroHealth, "INTEGRITY ZERO" },
        { DeathType.zeroOxygen, "OXYGEN ZERO" },
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCtrl = player.GetComponent<FirstPersonController>();
        deathScreen = GetComponent<DeathScreen>();

        player.gameState = this;
        enemyOriginal.gameState = this;
        player.GetComponent<OxygenComponent>().gameState = this;

        ToggleInput(false);

        //StartCoroutine(TestKillPlayer());
    }

    /*
    IEnumerator TestKillPlayer()
    {
        yield return new WaitForSeconds(2);
        if (true)
        {
            player.GetComponent<OxygenComponent>().ChangeOxygen(-100);
        }
        else
        {
            player.GetComponent<CombatComponent>().ChangeHealth(-100);
        }
    }
    */

    // Update is called once per frame
    void Update()
    {
        
    }

    void ToggleInput(bool UI)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = UI;
    }

    void EndGame(DeathType causeOfDeath)
    {
        ToggleInput(true);
        deathScreen.Toggle(true, deathMessage[causeOfDeath]);
    }

    //override IGameState

    public void ReportDeath(GameObject corpse, DeathType causeOfDeath)
    {
        if (corpse == player.gameObject)
        {
            EndGame(causeOfDeath);
        }
        else if (corpse == enemyOriginal.gameObject)
        {
            //enemy died
        }
    }

    public void ReportQuit()
    {
        SceneManager.LoadScene(menuScene);
    }
}

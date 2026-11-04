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
    [SerializeField] GameObject enemySpawnPointsParent;
    [SerializeField] float respawnDelay = 10f;
    [SerializeField] int deadEnemiesToWin = 3;

    FirstPersonController playerCtrl;
    FuelRewardComponent playerFuelSystem;
    DeathScreen deathScreen;

    int enemiesKilled = 0;
    List<Vector3> enemySpawnPoints = new List<Vector3>();
    SortedDictionary<DeathType, string> deathMessage = new SortedDictionary<DeathType, string>() 
    {
        { DeathType.zeroHealth, "INTEGRITY ZERO" },
        { DeathType.zeroOxygen, "OXYGEN ZERO" },
        { DeathType.winCondition, "YOU WIN" }
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCtrl = player.GetComponent<FirstPersonController>();
        playerFuelSystem = player.GetComponent<FuelRewardComponent>();
        deathScreen = GetComponent<DeathScreen>();

        foreach (Transform spawnPoint in enemySpawnPointsParent.GetComponentsInChildren<Transform>())
        {
            enemySpawnPoints.Add(spawnPoint.position);
        }
        enemySpawnPoints.Remove(enemySpawnPointsParent.transform.position);

        deathScreen.gameState = player.gameState = enemyOriginal.gameState = player.GetComponent<OxygenComponent>().gameState = this;

        ToggleInput(false);

        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        yield return new WaitForEndOfFrame();
        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        enemyOriginal.Activate(enemySpawnPoints[Random.Range(0, enemySpawnPoints.Count)]);
        player.NotifyEnemySpawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ToggleInput(bool UI)
    {
        Cursor.lockState = UI ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = UI;
    }

    void EndGame(DeathType causeOfDeath)
    {
        ToggleInput(true);
        deathScreen.Toggle(true, deathMessage[causeOfDeath]);
    }

    IEnumerator RespawnSequence()
    {
        yield return new WaitForSeconds(respawnDelay);
        SpawnEnemy();
    }

    IEnumerator WinSequence()
    {
        yield return new WaitForSeconds(1f);
        EndGame(DeathType.winCondition);
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
            if (++enemiesKilled >= deadEnemiesToWin)
            {
                StartCoroutine(WinSequence());
            } 
            else
            {
                playerFuelSystem.Reward();
                StartCoroutine(RespawnSequence());
            }
        }
    }

    public void ReportQuit()
    {
        SceneManager.LoadScene(menuScene);
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameState {Freeroam, Pause, Battle, Dialog, Inventory, Shopping, QuestLog, SpellLog, QuestBoard, Cooking, StylistSelect }
public class GameManager : MonoBehaviour
{
    // - KarateKamiOW
    public static GameManager instance;
    public GameState state;
    public QuestLogUI playerQuestLog;
    public SpellLogUI playerSpellLog;
    public CookingLogUI playerCookingLog;
    [Header("Quest board UI Script. Can be left blank")]
    public QuestBoardUI sceneQuestBoard; //Can Be left blank if no quest board around

    

    public GameObject displayPlayerInv;
    public bool NearCampFire { get; set; }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            SingletonData.GameManagerInstanceData = this;
        }
        NearCampFire = false;
        LeanTween.scale(displayPlayerInv, Vector3.zero, 0f);
        playerQuestLog.CloseUI();

        if(playerSpellLog != null)
            playerSpellLog.CloseUI();

        
    }

    void Update()
    {
        if (state == GameState.Freeroam)
        {
            PlayerController.instance.HandleUpdate();
            PlayerController.instance.Paused = false;
            if (Input.GetKeyDown(KeyCode.I))
            {
                LeanTween.scale(displayPlayerInv, new Vector3(2.2f, 2.2f, 1f), .3f);
                PlayerController.instance.Paused = true;
                state = GameState.Inventory;
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                playerQuestLog.GenerateQuestPanelsUI();
                PlayerController.instance.Paused = true;
                state = GameState.QuestLog;
            }
            if (Input.GetKeyDown(KeyCode.F))
            {

                playerSpellLog.OpenUI();
                playerSpellLog.OpenRhokanList();
                PlayerController.instance.Paused = true;
                state = GameState.SpellLog;
            }
            if (Input.GetKeyDown(KeyCode.C)) 
            {
                if (PlayerController.instance.InATownMap || NearCampFire)
                {
                    PlayerController.instance.Paused = true;
                    playerCookingLog.OpenCookingLog();
                    state = GameState.Cooking;
                }
            }
        }
        else if (state == GameState.Pause)
        {
            PlayerController.instance.Paused = true;
        }
        else if (state == GameState.Dialog)
        {
            PlayerController.instance.Paused = true;
        }
        else if (state == GameState.Inventory)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                LeanTween.scale(displayPlayerInv, Vector3.zero, .1f);
                PlayerController.instance.Paused = false;
                state = GameState.Freeroam;
            }

        }
        else if (state == GameState.QuestLog)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                playerQuestLog.CloseUI();
                PlayerController.instance.Paused = false;
                state = GameState.Freeroam;
            }
        }
        else if (state == GameState.SpellLog)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.F))
            {
                playerSpellLog.DestroyPanels();
                playerSpellLog.CloseUI();
                PlayerController.instance.Paused = false;
                state = GameState.Freeroam;
            }
        }
        else if (state == GameState.QuestBoard)
        {
            //sceneQuestBoard.HandleUpdate();
            PlayerController.instance.Paused = true;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                sceneQuestBoard.CloseQuestBoard();
                PlayerController.instance.Paused = false;
                state = GameState.Freeroam;
            }
        }
        else if (state == GameState.Cooking) 
        {
            playerCookingLog.HandleUpdate();
            if (Input.GetKeyDown(KeyCode.Escape) && playerCookingLog.cookingState != CookingStates.Cooking) 
            {
                PlayerController.instance.Paused = false;
                state = GameState.Freeroam;
                playerCookingLog.CloseAll();
            }
        }

    }
}

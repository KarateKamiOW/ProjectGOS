using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public enum TestingServerRoomState {MainMenu, JoiningRoom, CreatingRoom, EnteringUsername }
public class TestingServerMenuController : MonoBehaviour
{
    string versionNum = "0.1";
    [SerializeField] TMP_InputField joinRoomInputField;
    [SerializeField] TMP_InputField createRoomInputField;
    [SerializeField] TMP_InputField usernameInputField;
    [SerializeField] GameObject joinGameIFGameObject;
    [SerializeField] GameObject createGameIFGameObject;
    [SerializeField] GameObject usernameScreenGameObject;
    [SerializeField] List<GameObject> joinCreateSelectors;
    [SerializeField] List<GameObject> EnterKeysHints;
    TestingServerRoomState testServerRoomState;
    int mainMenuSelectorNum;

    
    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings(versionNum);
    }

    private void OnConnectedToMaster() 
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("Connected");
    }
    private void Start()
    {
        testServerRoomState = TestingServerRoomState.MainMenu;
        mainMenuSelectorNum = 0;
        joinRoomInputField.text = "";
        createRoomInputField.text = "";
        usernameInputField.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (testServerRoomState == TestingServerRoomState.MainMenu)
        {
            HandleMainMenu();
            joinGameIFGameObject.SetActive(false);
            createGameIFGameObject.SetActive(false);
            usernameScreenGameObject.SetActive(false);
        }
        else if (testServerRoomState == TestingServerRoomState.JoiningRoom)
        {
            HandleJoiningRoom();
            joinGameIFGameObject.SetActive(true);
            createGameIFGameObject.SetActive(false);
            usernameScreenGameObject.SetActive(false);
        }
        else if (testServerRoomState == TestingServerRoomState.CreatingRoom)
        {
            HandleCreatingRoom();
            joinGameIFGameObject.SetActive(false);
            createGameIFGameObject.SetActive(true);
            usernameScreenGameObject.SetActive(false);

        }
        else if (testServerRoomState == TestingServerRoomState.EnteringUsername) 
        {
            HandleEnteringUsername();
            joinGameIFGameObject.SetActive(false);
            createGameIFGameObject.SetActive(false);
            usernameScreenGameObject.SetActive(true);
        }
    }

    public void HandleJoinCreateSelectors(int selectorNum) 
    {
        for (int i = 0; i < joinCreateSelectors.Count; i++) 
        {
            if (i == selectorNum)
                joinCreateSelectors[selectorNum].SetActive(true);
            else
                joinCreateSelectors[i].SetActive(false);
        }
    }

    public void HandleMainMenu() 
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.DownArrow)) 
        {
            mainMenuSelectorNum--;

            if (mainMenuSelectorNum < 0)
                mainMenuSelectorNum = 1;

            HandleJoinCreateSelectors(mainMenuSelectorNum);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow)) 
        {
            mainMenuSelectorNum++;

            if (mainMenuSelectorNum > 1)
                mainMenuSelectorNum = 0;

            HandleJoinCreateSelectors(mainMenuSelectorNum);
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) 
        {
            if (mainMenuSelectorNum == 0)
            {
                testServerRoomState = TestingServerRoomState.JoiningRoom;
            }
            else if (mainMenuSelectorNum == 1)
            {
                testServerRoomState = TestingServerRoomState.CreatingRoom;
            }
            else 
            {
                Debug.Log("Error! Main Menu Selector Num = " + mainMenuSelectorNum);
            }
        }
    }
    public void HandleJoiningRoom() 
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            testServerRoomState = TestingServerRoomState.MainMenu;
            joinRoomInputField.text = "";
        }
        if (joinRoomInputField.text.Length == 0 && Input.GetKeyDown(KeyCode.Backspace)) 
        {
            testServerRoomState = TestingServerRoomState.MainMenu;
            joinRoomInputField.text = "";
        }

        if (joinRoomInputField.text.Length >= 3)
            EnterKeysHints[0].SetActive(true);
        else
            EnterKeysHints[0].SetActive(false);

        if (joinRoomInputField.text.Length >= 3 && Input.GetKeyDown(KeyCode.Return))
        {
            testServerRoomState = TestingServerRoomState.EnteringUsername;
        }

    }

    public void HandleCreatingRoom() 
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            testServerRoomState = TestingServerRoomState.MainMenu;
            createRoomInputField.text = "";
        }
        if (createRoomInputField.text.Length == 0 && Input.GetKeyDown(KeyCode.Backspace)) 
        {
            testServerRoomState = TestingServerRoomState.MainMenu;
            createRoomInputField.text = "";
        }

        if (createRoomInputField.text.Length >= 3)
            EnterKeysHints[1].SetActive(true);
        else
            EnterKeysHints[1].SetActive(false);

        if (createRoomInputField.text.Length >= 3 && Input.GetKeyDown(KeyCode.Return)) 
        {
            testServerRoomState = TestingServerRoomState.EnteringUsername;
        }
    }
    public void HandleEnteringUsername() 
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            testServerRoomState = TestingServerRoomState.MainMenu;
        }
        if (usernameInputField.text.Length == 0 && Input.GetKeyDown(KeyCode.Backspace))
        {
            testServerRoomState = TestingServerRoomState.MainMenu;
            usernameInputField.text = "";
        }

        if (usernameInputField.text.Length >= 3)
            EnterKeysHints[2].SetActive(true);
        else
            EnterKeysHints[2].SetActive(false);

        if (usernameInputField.text.Length >= 3 && Input.GetKeyDown(KeyCode.Return)) 
        {
            RoomJoining();
        }
    }

    public void RoomCreation() 
    {
        PhotonNetwork.CreateRoom(createRoomInputField.text, new RoomOptions() { MaxPlayers = 2 }, null);
        OnJoinedRoom();
    }

    public void RoomJoining() 
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.JoinOrCreateRoom(joinRoomInputField.text, roomOptions, TypedLobby.Default);
        OnJoinedRoom();
    }

    private void OnJoinedRoom() 
    {
        PhotonNetwork.LoadLevel("SampleScene");


    }


}

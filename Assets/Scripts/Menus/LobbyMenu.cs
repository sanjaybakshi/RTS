using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class LobbyMenu : MonoBehaviour
{
    [SerializeField] private GameObject lobbyUI = null;
    [SerializeField] private Button startGameButton = null;

    [SerializeField] private Text[] playerNameTexts = new Text[4];

    void Start()
    { 
        print("Start called");
        lobbyUI.SetActive(false);
        
        RTSNetworkManager.ClientOnConnected += HandleClientConnected;

        RTSPlayer.AuthorityOnPartyOwnerStateUpdated += AuthorityHandlePartyOwnerStateUpdated;
    
        RTSPlayer.ClientOnInfoUpdated += ClientHandleInfoUpdated;
    }

    void OnDestroy()
    {
        RTSNetworkManager.ClientOnConnected -= HandleClientConnected;

        RTSPlayer.ClientOnInfoUpdated -= ClientHandleInfoUpdated;
    }

    private void HandleClientConnected()
    {
        lobbyUI.SetActive(true);

        print("HandleClientConnected");
    }   

    private void ClientHandleInfoUpdated()
    {
        List<RTSPlayer> players = ((RTSNetworkManager)NetworkManager.singleton).Players;


        for (int i = 0; i < players.Count; i++)
        {
            playerNameTexts[i].text = players[i].GetDisplayName();
        }

        for (int i = players.Count; i < playerNameTexts.Length; i++)
        {
            playerNameTexts[i].text = "Waiting for player...";
        }

        startGameButton.interactable = players.Count >= 2;
    }

    private void AuthorityHandlePartyOwnerStateUpdated(bool state)
    {
        startGameButton.gameObject.SetActive(state);
    }

    public void StartGame()
    {
        NetworkClient.connection.identity.GetComponent<RTSPlayer>().CmdStartGame();
    }
    public void LeaveLobby()
    {
        if (NetworkServer.active && NetworkClient.isConnected) {
            NetworkManager.singleton.StopHost();
        } else {
            NetworkManager.singleton.StopClient();

            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
}

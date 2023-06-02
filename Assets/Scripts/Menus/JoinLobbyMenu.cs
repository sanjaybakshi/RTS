using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class JoinLobbyMenu : MonoBehaviour
{

    [SerializeField] private GameObject landingPagePanel = null;
    [SerializeField] private InputField ipAddressInputField = null;
    [SerializeField] private Button joinButton = null;


    private void OnEnable()
    {
        RTSNetworkManager.ClientOnConnected    += HandleClientConnected;
        RTSNetworkManager.ClientOnDisconnected += HandleClientDisconnected; 
    }

    private void OnDisable()
    {
        RTSNetworkManager.ClientOnConnected    -= HandleClientConnected;
        RTSNetworkManager.ClientOnDisconnected -= HandleClientDisconnected; 
    }

    public void Join()
    {
        string address = ipAddressInputField.text;

        NetworkManager.singleton.networkAddress = address;
        NetworkManager.singleton.StartClient();

        joinButton.interactable = false;
    }

    private void HandleClientConnected()
    {
        joinButton.interactable = true;

        gameObject.SetActive(false);
        landingPagePanel.SetActive(false);
    }

    private void HandleClientDisconnected()
    {
        joinButton.interactable = true;
    }
}
 
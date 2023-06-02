using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class RTSPlayer : NetworkBehaviour
{

    [SerializeField] private List<Unit> myUnits = new List<Unit>();


    [SyncVar(hook = nameof(AuthorityHandlePartyOwnerStateUpdated))]
    private bool isPartyOwner = false;    

    [SyncVar(hook = nameof(ClientHandleDisplayNameUpdated))]
    private string displayName;

    public static event Action ClientOnInfoUpdated;

    public static event Action<bool> AuthorityOnPartyOwnerStateUpdated;

    public string GetDisplayName()
    {
        return displayName;
    }

    public List<Unit> GetMyUnits()
    {
        return myUnits;
    }
    
    #region Server
    public override void OnStartServer()
    {
        Unit.ServerOnUnitSpawned   += ServerHandleUnitSpawned;
        Unit.ServerOnUnitDespawned += ServerHandleUnitDespawned;

        DontDestroyOnLoad(gameObject);
    }
    [Command]
    public void CmdStartGame()
    {
        if (!isPartyOwner) { return; }

        ((RTSNetworkManager)NetworkManager.singleton).StartGame();
    }

    public override void OnStopServer()
    {
        Unit.ServerOnUnitSpawned   -= ServerHandleUnitSpawned;
        Unit.ServerOnUnitDespawned -= ServerHandleUnitDespawned;
    }

    private void ServerHandleUnitSpawned(Unit unit)
    {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId) { return; }

        myUnits.Add(unit);
    }

    private void ServerHandleUnitDespawned(Unit unit)
    {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId) { return; }

        myUnits.Remove(unit);
    }

    [Server]
    public void SetDisplayName(string displayName)
    {
        this.displayName = displayName;
    }

    [Server]
    public void SetPartyOwner(bool state)
    {
        isPartyOwner = state;
    }

    #endregion

    #region Client

    public override void OnStartAuthority()
    {
        if (NetworkServer.active) { return; }

        Unit.AuthorityOnUnitSpawned += AuthorityHandleUnitSpawned;
        Unit.AuthorityOnUnitDespawned += AuthorityHandleUnitDespawned;
    }
    public override void OnStartClient()
    {
        if (NetworkServer.active) { return; }   

        DontDestroyOnLoad(gameObject);

        ((RTSNetworkManager)NetworkManager.singleton).Players.Add(this);
    }

    public override void OnStopClient()
    {
        ClientOnInfoUpdated?.Invoke();

        if (!isClientOnly) { return; }

        ((RTSNetworkManager)NetworkManager.singleton).Players.Remove(this);

        if (!hasAuthority) { return; }


        Unit.AuthorityOnUnitSpawned   -= AuthorityOnUnitSpawned;
        Unit.AuthorityOnUnitDespawned -= AuthorityOnUnitDespawned;
    }   


    private void AuthorityOnUnitSpawned(Unit unit)
    {
        if (!hasAuthority) { return; }

        myUnits.Add(unit);
    }

    private void AuthorityOnUnitDespawned(Unit unit)
    {
        if (!hasAuthority) { return; }

        myUnits.Remove(unit);
    }   

    private void AuthorityHandlePartyOwnerStateUpdated(bool oldState, bool newState)
    {
        if (!hasAuthority) { return; }

        AuthorityOnPartyOwnerStateUpdated?.Invoke(newState);
    }

    private void AuthorityHandleUnitSpawned(Unit unit)
    {
        myUnits.Add(unit);
    }

    private void AuthorityHandleUnitDespawned(Unit unit)
    {
        myUnits.Remove(unit);
    }

    private void ClientHandleDisplayNameUpdated(string oldDisplayName, string newDisplayName)
    {
        ClientOnInfoUpdated?.Invoke();
    }
    #endregion

}

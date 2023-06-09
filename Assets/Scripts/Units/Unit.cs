using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Mirror;

public class Unit : NetworkBehaviour
{
    [SerializeField] private UnitMovement unitMovement = null;
    [SerializeField] private UnityEvent onSelected = null;
    [SerializeField] private UnityEvent onDeselected = null;
    [SerializeField] private Targeter targeter = null;

    public static event System.Action<Unit> ServerOnUnitSpawned;
    public static event System.Action<Unit> ServerOnUnitDespawned;

    public static event System.Action<Unit> AuthorityOnUnitSpawned;
    public static event System.Action<Unit> AuthorityOnUnitDespawned;


    public UnitMovement GetUnitMovement()
    {
        return unitMovement;
    }   

    public Targeter GetTargeter()
    {
        return targeter;
    }
    
    #region Server

    public override void OnStartServer()
    {
        ServerOnUnitSpawned?.Invoke(this);
    }

    public override void OnStopServer()
    {
        ServerOnUnitDespawned?.Invoke(this);
    }

    #endregion



    #region Client
    [Client]

    public override void OnStartClient()
    {
        if (!isClientOnly || !hasAuthority) { return; }

        AuthorityOnUnitSpawned?.Invoke(this);
    }

    public override void OnStopClient()
    {
        if (!isClientOnly || !hasAuthority) { return; }
        
        AuthorityOnUnitDespawned?.Invoke(this);
    }

    public void Select()
    {
        if (!hasAuthority) { return; }
        onSelected?.Invoke();
    }

    public void Deselect()
    {
        if (!hasAuthority) { return; }
        onDeselected?.Invoke();
    }


    #endregion
}
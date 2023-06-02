using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Mirror;

public class UnitSpawner : NetworkBehaviour,  IPointerClickHandler
{
    [SerializeField] private GameObject unitPrefab = null;
    [SerializeField] private Transform unitSpawnPoint = null;
    
    #region Server

    [Command]
    private void CmdSpawnUnit()
    {
        GameObject unitInstance = Instantiate(
            unitPrefab, 
            unitSpawnPoint.position, 
            unitSpawnPoint.rotation);

        NetworkServer.Spawn(unitInstance, connectionToClient);
    }

    #endregion

    #region Client

    public void OnPointerClick(PointerEventData eventData)
    {
        print("UnitSpawner::OnPointerClick");
        if (eventData.button != PointerEventData.InputButton.Left) {
            return;
        }
        
        print("UnitSpawner::OnPointerClick::hasAuthority: " + hasAuthority);
        if (!hasAuthority) {
            return;
        }

        CmdSpawnUnit();
    }

    #endregion


}

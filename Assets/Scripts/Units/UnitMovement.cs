
using UnityEngine;
using UnityEngine.AI;
using Mirror;


public class UnitMovement : NetworkBehaviour
{

    [SerializeField] private UnityEngine.AI.NavMeshAgent agent = null;
    [SerializeField] private Targeter targeter = null;

    private Camera mainCamera;

    #region Server

    [ServerCallback]
    private void Update()
    {
        if (!agent.hasPath) { return; }

        if (agent.remainingDistance > agent.stoppingDistance) { return; }

        agent.ResetPath();
    }




    [Command]
    public void CmdMove(Vector3 position)
    {
        targeter.ClearTarget();
        
        if (!UnityEngine.AI.NavMesh.SamplePosition(position, out UnityEngine.AI.NavMeshHit hit, 1f, UnityEngine.AI.NavMesh.AllAreas)) {
            return;
        }

        agent.SetDestination(hit.position);
    }

    #endregion
}

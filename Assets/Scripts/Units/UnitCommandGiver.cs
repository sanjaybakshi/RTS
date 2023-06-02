using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitCommandGiver : MonoBehaviour
{
    [SerializeField] private UnitSelectionHandler unitSelectionHandler = null;
    [SerializeField] private LayerMask layerMask = new LayerMask();

    private Camera mainCamera;


    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {


        if (!Mouse.current.rightButton.wasPressedThisFrame) {
            return;
        }

        print("ok calling CommandGiver::Update");

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) {
            return;
        }
        print("...... raycast hit!");



        if (hit.collider.TryGetComponent<Targetable>(out Targetable target)) {
            if (target.hasAuthority) {
                TryMove(hit.point);
                return;
            }
            TryTarget(target);
            return;
        }


        print("...... try to move!");

        TryMove(hit.point);
    }

    private void TryMove(Vector3 point)
    {
        foreach (Unit unit in unitSelectionHandler.SelectedUnits) {
            unit.GetUnitMovement().CmdMove(point);
        }
    }

    private void TryTarget(Targetable target)
    {
        foreach (Unit unit in unitSelectionHandler.SelectedUnits) {
            unit.GetTargeter().CmdSetTarget(target.gameObject);
        }
    }

}

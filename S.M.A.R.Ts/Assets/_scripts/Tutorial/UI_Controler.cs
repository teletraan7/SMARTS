using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Controler : MonoBehaviour {

    public GameObject MovementUI;
    private bool MovementUIOff;
    public GameObject SwapUI;
    public GameObject abilityUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !MovementUIOff)
        {
            DisableMovementUI();
        }
    }

    void DisableMovementUI ()
    {
        MovementUI.SetActive(false);
        SwapUI.SetActive(true);
        abilityUI.SetActive(true);
        MovementUIOff = true;
    }
	
}

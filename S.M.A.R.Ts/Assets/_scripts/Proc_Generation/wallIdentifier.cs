using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallIdentifier : MonoBehaviour {

	public GameObject standard;
	public GameObject reinforced;

    private void Start()
    {
        reinforced.gameObject.SetActive(false);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAbilityGuides : MonoBehaviour {

    public GameObject demolitions;
    public GameObject hacker;
    public GameObject support;
    public GameObject defender;

    public GameObject demoleft;
    public GameObject hackerleft;
    public GameObject supportleft;
    public GameObject defleft;

    public GameObject demoright;
    public GameObject hackerright;
    public GameObject supportright;
    public GameObject defright;

    private void LateUpdate()
    {
        if (demolitions.activeInHierarchy == true)
        {
            if (demolitions.GetComponent<PlayerBehaviour>().isLeft == true)
            {
                demoleft.SetActive(true);
                demoright.SetActive(false);
            } else
            {
                demoright.SetActive(true);
                demoleft.SetActive(false);
            }
        } else
        {
            demoright.SetActive(false);
            demoleft.SetActive(false);
        }

        if (hacker.activeInHierarchy == true)
        {
            if (hacker.GetComponent<PlayerBehaviour>().isLeft == true)
            {
                hackerleft.SetActive(true);
                hackerright.SetActive(false);
            }
            else
            {
                hackerright.SetActive(true);
                hackerleft.SetActive(false);
            }
        }
        else
        {
            hackerleft.SetActive(false);
            hackerright.SetActive(false);
        }

        if (support.activeInHierarchy == true)
        {
            if (support.GetComponent<PlayerBehaviour>().isLeft == true)
            {
                supportleft.SetActive(true);
                supportright.SetActive(false);
            }
            else
            {
                supportleft.SetActive(false);
                supportright.SetActive(true);
            }
        }
        else
        {
            supportleft.SetActive(false);
            supportright.SetActive(false);
        }

        if (defender.activeInHierarchy == true)
        {
            if (defender.GetComponent<PlayerBehaviour>().isLeft == true)
            {
                defleft.SetActive(true);
                defright.SetActive(false);
            }
            else
            {
                defright.SetActive(true);
                defleft.SetActive(false);
            }
        }
        else
        {
            defleft.SetActive(false);
            defright.SetActive(false);
        }
    }

}

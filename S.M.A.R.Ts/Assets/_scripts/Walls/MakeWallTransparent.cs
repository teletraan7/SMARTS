using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeWallTransparent : MonoBehaviour {

    //all needed mats
    public Material transparent;
    public Material regular;
    public Material reinforced;

    //specific pieces of the reinforced walls
    public GameObject Reinforced_1;
    public GameObject Reinforced_2;
    public GameObject bump1;
    public GameObject bump2;
    public GameObject bump3;

    public bool IsTransparent;

    //when this gameobject is hit by a ray
    public void HitbyRay ()
    {   
       if (IsTransparent == false)
        {
            //check what type of wall it is
            if (this.gameObject.tag == "Wall" || this.gameObject.tag == "doorWay")
            {
                //make transparent
                MeshRenderer[] Meshs = this.gameObject.GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer Mr in Meshs)
                {            

                    Mr.material = transparent;
                    IsTransparent = true;
                }
            }
            if (this.gameObject.tag == "ReinforcedWall")
            {
                //make transparent
                Reinforced_1.GetComponent<MeshRenderer>().material = transparent;
                Reinforced_2.GetComponent<MeshRenderer>().material = transparent;
                bump1.GetComponent<MeshRenderer>().material = transparent;
                bump2.GetComponent<MeshRenderer>().material = transparent;
                bump3.GetComponent<MeshRenderer>().material = transparent;
                IsTransparent = true;
            }
        }
    }


    //when the ray leaves this gameobject
    public void RayExit()
    {
        //find out what kind of wall this is
        if (this.gameObject.tag == "Wall" || this.gameObject.tag == "doorWay")
        {
            //make regular
            MeshRenderer[] Meshs = this.gameObject.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer Mr in Meshs)
            {
                Mr.material = regular;
                IsTransparent = false;
            }
        }
        if (this.gameObject.tag == "ReinforcedWall")
        {
            //make regular
            Reinforced_1.GetComponent<MeshRenderer>().material = regular;
            Reinforced_2.GetComponent<MeshRenderer>().material = reinforced;
            bump1.GetComponent<MeshRenderer>().material = reinforced;
            bump2.GetComponent<MeshRenderer>().material = reinforced;
            bump3.GetComponent<MeshRenderer>().material = reinforced;
            IsTransparent = false;
        }
    }


}

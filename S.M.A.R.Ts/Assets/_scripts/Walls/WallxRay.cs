using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallxRay : MonoBehaviour {

    //all of the players and the camera
    public GameObject[] Players;
    private GameObject cam;
    private List<GameObject> WallsHitLastFrame = new List<GameObject>();


    //get the camera
    private void Start()
    {
        cam = this.gameObject;
    }

    private void LateUpdate()
    {
        List<GameObject> WallsHitThisFrame = new List<GameObject>();
        //foreach of the players
        foreach (GameObject Player in Players)
        {             
            //make your ray and output
            Ray RaytoPlayer = new Ray(this.transform.position, Player.transform.position - transform.position);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(RaytoPlayer, 100f);
            //draw for debuging 
            Debug.DrawRay(RaytoPlayer.origin, RaytoPlayer.direction * 50f, Color.red);
            //go through hits             
            for (int i = 0; i < hits.Length; i++)
            {              
                //compare distances from cam to this object and player, if the player is further then continue
                if (Vector3.Distance(cam.transform.position, hits[i].transform.position) < Vector3.Distance(cam.transform.position, Player.transform.position))
                {
                    //if the ray hits a wall
                    if (hits[i].collider.gameObject.tag == "Wall" || hits[i].collider.gameObject.tag == "ReinforcedWall" || hits[i].collider.gameObject.tag == "doorWay")
                    {
                        if (!WallsHitLastFrame.Contains(hits[i].collider.gameObject))
                        {
                            //tell that wall it was hit
                            hits[i].collider.gameObject.GetComponent<MakeWallTransparent>().HitbyRay();
                            WallsHitThisFrame.Add(hits[i].collider.gameObject);
                        } else if(WallsHitLastFrame.Contains(hits[i].collider.gameObject))
                        {
                            WallsHitThisFrame.Add(hits[i].collider.gameObject);
                        } 
                    }                  
                } 
            }
        }
        foreach (GameObject wall in WallsHitLastFrame)
        {
            if (!WallsHitThisFrame.Contains(wall))
            {
                wall.GetComponent<MakeWallTransparent>().RayExit();
            }
        }
        WallsHitLastFrame = WallsHitThisFrame;
    }
}

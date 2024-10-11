using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;


[CustomEditor (typeof (RobotFoV))]
public class FoVEditor : Editor {

    

    void OnSceneGUI()
    {
        RobotFoV fov = (RobotFoV)target;
        Handles.color = Color.red;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.FovRadius);
        Vector3 ViewAngleA = fov.GetDirFromAngle(-fov.FovAngle / 2, false);
        Vector3 ViewAngleB = fov.GetDirFromAngle(fov.FovAngle / 2, false);

        Handles.DrawLine(fov.transform.position, fov.transform.position + ViewAngleA * fov.FovRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + ViewAngleB * fov.FovRadius);

        Handles.color = Color.white;
        foreach(Transform VisiblePlayer in fov.VisiblePlayers)
        {
            Handles.DrawLine(fov.transform.position, VisiblePlayer.position);
        }
    }

}

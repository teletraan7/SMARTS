using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotFoV : MonoBehaviour {

    private PointsManager psm;

    public float FovRadius;

    [Range(0, 360)]
    public float FovAngle;

    public LayerMask TargetLayer;
    public LayerMask ObstacleLayer;
    public NavMeshAgent botNav;
    public RobotAI botAI;
    private Transform TargetPlayer;
    public bool TargetAquired;
    public float MeshRes;
    public MeshFilter ViewMeshFilter;
    private Mesh ViewMesh;
    private bool TargetInSight;
    private AudioSource audioSor;
    public AudioClip FireWeapon;

   [HideInInspector]
    public List<Transform> VisiblePlayers = new List<Transform>();


    //Components used for the Turret
    public GameObject TurretCap;
    public float RotationSpeed;
    public bool IsDisabled;
    public int alertRaise = 10;
    private bool firing;
    public float cooldownTime;
    public ParticleSystem charge;
    public ParticleSystem bolt;
    private PlayerShocked playershocked;
    private botShock botshock;
    private bool Recharging;
   
    //check what type of robot this is and get some needed universal references
    private void Start()
    {
        if (this.gameObject.tag == "drone")
        {
            botAI = this.gameObject.GetComponent<RobotAI>();
            botNav = this.gameObject.GetComponent<NavMeshAgent>();
        }
        if (this.gameObject.tag == "turret")
        {
            StartCoroutine(TurnTurret());
            botshock = this.gameObject.GetComponent<botShock>();
        }
        ViewMesh = new Mesh();
        ViewMesh.name = "FoV Mesh";
        ViewMeshFilter.mesh = ViewMesh;
        TargetAquired = false;
        StartCoroutine(SearchDelay(.5f));
        psm = GameObject.FindWithTag("GameController").GetComponent<PointsManager>();
    }

    //turn the turret
    IEnumerator TurnTurret ()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            IsDisabled = botshock.shocked;
            //if able then spin in a 360
            if (TargetAquired == false && IsDisabled == false || Recharging)
            {
                transform.Rotate(Vector3.up, Time.deltaTime * RotationSpeed, Space.World);
            }
            //get the target position relative to this gameobject and look at it. look position is changed too prevent turret from leaning up or down
            else if (TargetAquired == true && IsDisabled == false)
            {
                Vector3 lookposition = TargetPlayer.position - transform.position;
                lookposition.y = 0;
                Quaternion rotation = Quaternion.LookRotation(lookposition);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * RotationSpeed);
                
            }
            //if disabled then stop turning
            else if (IsDisabled)
            {
                transform.Rotate(new Vector3(0f, 0f, 0f));
            }
        }
    }

    //just a delay before the robot continues searching after a target is shocked
    IEnumerator SearchDelay(float Delay)
    {
        while(true)
        {
            yield return new WaitForSeconds(Delay);
            FindTargets();
        }
    }


    private void LateUpdate()
    {
        VisualizeFoV();
       
    }

    //this will draw a custom mesh to visualize the foz of robots
    void VisualizeFoV()
    {
        //how many rays will be cast
        int RayCount = Mathf.RoundToInt(FovAngle * MeshRes);
        //dividing the mesh into pieces by this angle size
        float SectionAngleSize = FovAngle / RayCount;
        List<Vector3> viewPoints = new List<Vector3>();

        //loop through rays, get info about what they hit and how far, add that to viewpoints
        for (int i = 0; i <= RayCount; i++)
        {
            float angle = transform.eulerAngles.y - FovAngle / 2 + SectionAngleSize * i;
            RayCastInfo newViewCast = ViewCast(angle);
            viewPoints.Add(newViewCast.endpoint);
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        //draw the mesh/create the vertices for it
        vertices[0] = Vector3.zero;

        for (int i = 0; i < vertexCount -1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {             
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        ViewMesh.Clear();
        ViewMesh.vertices = vertices;
        ViewMesh.triangles = triangles;
        ViewMesh.RecalculateNormals();
    }

    //this struct will store info about the ray like what was the direction, how far it went, and what it hit
    RayCastInfo ViewCast (float globalAngle)
    {
        Vector3 dir = GetDirFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, FovRadius, ObstacleLayer))
        {
            return new RayCastInfo(true, hit.point, hit.distance, globalAngle);
        } else
        {
            return new RayCastInfo(false, transform.position + dir * FovRadius, FovRadius, globalAngle);
        }
    }

    //find targets
    public void FindTargets ()
    {
        //list will be used to store all players in site, clear it first
        VisiblePlayers.Clear();
        //initial sphere to detect objects, look only for players by using a target layer
        Collider[] TargetsInRadius = Physics.OverlapSphere(transform.position, FovRadius, TargetLayer);
        
        for (int i = 0; i < TargetsInRadius.Length; i++)
        {
            //for each player found get their info, angle to them from the robots center 
            Transform target = TargetsInRadius[i].transform;
            Vector3 AngleToTarget = (target.position - transform.position).normalized;
            //if the player is within 45 deg fov both + or -
            if (Vector3.Angle(transform.forward, AngleToTarget) < FovAngle / 2)
            {
                //check if its within thhe proper range
                float DistToTarget = Vector3.Distance(transform.position, target.position);
                //draw a ray and if robot isnt already moving on a target then set this player to the target
                if (!Physics.Raycast(transform.position, AngleToTarget, DistToTarget, ObstacleLayer) && TargetAquired == false)
                {
                    VisiblePlayers.Add(target);
                    SelectTarget();
                    TargetInSight = true;
                    psm.TimesDetected += 1;
                 
                }
                //if the player is obscured or out of sight then continue but check if is still out of sight in 3 secs
                else if (Physics.Raycast(transform.position, AngleToTarget, DistToTarget, ObstacleLayer))
                {
                    TargetInSight = false;
                    Invoke("CheckIfTargetIsHidden", 3f);
                    psm.TimesDetected += 1;
                    
                }
            }
        }
    }

    //if target is out of site still then call the robot off and set it back to patrol
    private void CheckIfTargetIsHidden ()
    {
        if (TargetInSight == false && this.gameObject.tag == "drone")
        {
            TargetPlayer = null;
            TargetAquired = false;
        }
    }

    //this is what actually chooses the target from all available ones, here in case the robot can see more than one player
    public void SelectTarget ()
    {
        int i = Random.Range(0, VisiblePlayers.Count);
        TargetPlayer = VisiblePlayers[i];
        TargetAquired = true;
        if (this.gameObject.tag == "drone")
        {
            StartCoroutine(FollowTarget(TargetPlayer));
        } 
        if (this.gameObject.tag == "turret")
        {
            playershocked = TargetPlayer.GetComponent<PlayerShocked>();
            Charge();
        }
    }

    //gets the angle from bot to target
    public Vector3 GetDirFromAngle(float angleNdegrees, bool AngleIsGlobal)
    {
        if (!AngleIsGlobal)
        {
            angleNdegrees += transform.eulerAngles.y;
        }
      
        return new Vector3(Mathf.Sin(angleNdegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleNdegrees * Mathf.Deg2Rad));
    }

    //struct will store if a ray hit something, where it ended, how far, and at what angle
    public struct RayCastInfo
    {
        public bool hit;
        public Vector3 endpoint;
        public float distance;
        public float angl;

        public RayCastInfo(bool _hit, Vector3 _endpoint, float _distance, float _angl)
        {
            hit = _hit;
            endpoint = _endpoint;
            distance = _distance;
            angl = _angl;
        }

    }

    //DRONE CONTROLS

    IEnumerator FollowTarget(Transform Target)
    {
        botAI.target = Target.gameObject;
        while (TargetAquired == true && !botAI.targetShocked)
        {
            this.gameObject.transform.LookAt(Target.transform.position);
            botNav.SetDestination(Target.transform.position);
            yield return new WaitForFixedUpdate();
        }

        if (TargetAquired == false || botAI.targetShocked)
        {
            botNav.SetDestination(botAI.patrolPoints[botAI.patrolDes].position);
            yield break;
        }
    }

    //TURRET CONTROLS

    //charge the shot and fire charge animation, firing true lets the Turret know it is firing already
    void Charge()
    {

        firing = true;
        FloatingTextController.CreateFloatingText("Intruder detected, Alert + " + alertRaise.ToString(), transform);

        // add to security alert after bomb use
        GameSessionManager gsm = GameObject.FindWithTag("GameController").GetComponent<GameSessionManager>();

        gsm.AdjustSecurity(alertRaise);
        charge.Play();
        Invoke("Fire", 5f);
    }

    //fire the shot, send for the shockplayer script in playershocked, start cooldown
    void Fire()
    {
        Debug.Log("firing");
        if (TargetAquired == true && !IsDisabled)
        {
            bolt.Play();
            audioSor.clip = FireWeapon;
            audioSor.loop = false;
            audioSor.Play();
            playershocked.Invoke("ShockPlayerFromTurret", 0.25f);
            Invoke("Cooldown", 5f);
            Recharging = true;
        }
        else if (TargetAquired == false || IsDisabled)
        {
            bolt.Stop();
            charge.Stop();
            return;
        }
    }

    //cooldown for turret of five seconds, also make the turn scan the area, and tell it it is no longer firing
    void Cooldown()
    {
        cooldownTime = Time.time + cooldownTime;
        TargetAquired = false;
        firing = false;
        Recharging = false;
    }
}

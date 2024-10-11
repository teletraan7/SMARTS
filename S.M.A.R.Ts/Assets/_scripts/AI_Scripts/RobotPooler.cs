using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotPooler : MonoBehaviour {

	public Dictionary<string, Queue<GameObject>> poolDictionary;
	public List<Pool> pools;
	private int numOfBots;
	public int maxBotNum;
	public float delay;
	private float delayTime;

	public GameObject Exit;

	public GameObject BotSpawner;

    private bool GuardSpawned;

	[System.Serializable]
	public class Pool {
		public string tag;
		public GameObject botPrefab;
		public int size;
	}

	// Use this for initialization
	void Start () {
        BotSpawner = this.gameObject;
		delay = 20f;
		numOfBots = 0;
		maxBotNum = 2;
		poolDictionary = new Dictionary<string, Queue<GameObject>>();

		foreach (Pool pool in pools) {

			if(pool == pools[0])
            {
                Queue<GameObject> robotPool = new Queue<GameObject>();

                for (int i = 0; i < pool.size; i++)
                {
                    pool.botPrefab.GetComponent<NavMeshAgent>().enabled = false;
                    GameObject botObj = Instantiate(pool.botPrefab, this.gameObject.transform.forward, Quaternion.identity);
                    botObj.SetActive(false);
                    robotPool.Enqueue(botObj);
                }

                poolDictionary.Add(pool.tag, robotPool);
            }
            if (pool == pools[1])
            {
                Queue<GameObject> robotPool = new Queue<GameObject>();

                for (int i = 0; i < pool.size; i++)
                {
                    pool.botPrefab.GetComponent<NavMeshAgent>().enabled = false;
                    GameObject botObj = Instantiate(pool.botPrefab, this.gameObject.transform.forward, Quaternion.identity);
                    botObj.SetActive(false);
                    robotPool.Enqueue(botObj);
                }

                poolDictionary.Add(pool.tag, robotPool);
            }
		}
	}

	void FixedUpdate() {
		if (numOfBots < maxBotNum  && Time.time > delayTime) {
			SpawnRobot ("Bots", transform.forward, Quaternion.identity);
		}
	}

	//adjust the max number of drones able to spawn
	public void AdjustBots (int securityLevel) {
		if (securityLevel < 33) {
			maxBotNum = 2;
		} else if (securityLevel > 33 && securityLevel < 66) {
			maxBotNum = 5;
            if (!GuardSpawned)
            {
                SpawnSpecialBot("Terminal Guard", transform.forward, Quaternion.identity);
            }
		} else if (securityLevel > 66) {
			maxBotNum = 8;
		}
	}

	public void SpawnRobot (string tag, Vector3 position, Quaternion rotation) {

		if (!poolDictionary.ContainsKey(tag)) {
			Debug.Log ("Dictionary does not contain" + tag);
			return;
		}

		GameObject robot = poolDictionary [tag].Dequeue ();
        robot.GetComponent<NavMeshAgent>().enabled = true;
		robot.transform.position = Exit.transform.position;
		robot.transform.rotation = this.gameObject.transform.rotation;
		robot.SetActive (true);
		

		numOfBots++;
		delayTime = Time.time + delay;
	}

    //get security level
    //spawn a special bot
    //make its destination the AI terminal
    //give it a special tag so if it sees the player it sends out an alert
    //add this to the num of bots

    private void SpawnSpecialBot (string tag, Vector3 position, Quaternion rotation)
    {
        GuardSpawned = true;
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.Log("Dictionary does not contain" + tag);
            return;
        }

        GameObject robot = poolDictionary[tag].Dequeue();
        robot.GetComponent<NavMeshAgent>().enabled = true;
        robot.transform.position = Exit.transform.position;
        robot.transform.rotation = this.gameObject.transform.rotation;        
        GameObject Terminal = GameObject.FindGameObjectWithTag("AI").gameObject;

        robot.GetComponent<TerminalGuardAI>().Terminal = Terminal;
        robot.SetActive(true);
        numOfBots++;
        delayTime = Time.time + delay;
    }
		
}

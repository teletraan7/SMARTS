using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemolitionBomb : MonoBehaviour {

	public Dictionary<string, Queue<GameObject>> bombDictionary;
	public List<Bomb> bombs;
	public float numBombs;
	public float maxBombs;
	public bool canPlace;

	public GameObject notPooledBomb;

    public float ChargeFor;
    public float ChargeTime;

	[System.Serializable]
	public class Bomb {
		public string tag;
		public GameObject bombPrefab;
		public int size;
	}



	void Awake () {
		bombDictionary = new Dictionary<string, Queue<GameObject>> ();
		numBombs = 0f;
		foreach (Bomb bomb in bombs) {
			Queue<GameObject> bombPool = new Queue<GameObject> ();

			for (int i = 0; i < bomb.size; i++) {
				GameObject bombObj = Instantiate (bomb.bombPrefab);
				bombObj.SetActive (false);
				bombPool.Enqueue (bombObj);
			}

			bombDictionary.Add (bomb.tag, bombPool);
		}
			
	}

	public void PlaceBomb(string tag, Vector3 position, Quaternion rotation){
		if (!bombDictionary.ContainsKey (tag)) {
			Debug.Log ("Dictionary does not contain" + tag);
			return;
		}
			

			if (numBombs < maxBombs && Time.time > ChargeTime)
        {
            GameObject bomb = bombDictionary[tag].Dequeue();
            bomb.GetComponent<bombExpolosion>().CleanLists();
            bomb.SetActive(true);
            bomb.transform.position = position;
            bomb.transform.rotation = rotation;

            numBombs++;
            ChargeTime = ChargeFor + Time.time;

            bombDictionary[tag].Enqueue(bomb);

            Invoke("SubtractNumofBombs", 4f);

        }


    }

    private void SubtractNumofBombs ()
    {
        numBombs--;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//singleton used to store all info needed to assign players in a level
public class InfoManager : MonoBehaviour {

    public static InfoManager Info { get; private set; }
    //list of these storage classes
    public List<PNum> pNum;
    //list of players already assigned/picked
    public List<int> IndexOfCharactersAssigned = new List<int>();
    //custom class store the id of the controller, what player they are, the characters tied to them, if those characters start benched, and which side of the controller they start on
    public class PNum
    {
        public int ID;
        public int PlyrNum;
        public List<int> CharacterIDs = new List<int>();
        public List<bool> Benched = new List<bool>();
        public List<bool> isLeft = new List<bool>();

        public PNum(int ID, int PlyrNum)
        {
            this.ID = ID;
            this.PlyrNum = PlyrNum;
        }
    }
    //dont destroy this on load and if one already exists then destory the extra copy
    private void Awake()
    {
        if (Info == null)
        {
            Info = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    //used to reset the lists when players back out of the character select screen
    public void ResetList()
    {
        pNum = new List<PNum>();
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;

public class SelectController : MonoBehaviour {


    public bool Benched;
    public bool Active;
    private bool Ready;
    public Player RewiredPlayer;
    private GameObject Panel;
    private GameObject ButtonBar;
    private bool moveLeft;
    private bool moveRight;
    private bool accept;
    private bool back;
    public List<Vector3> BarPos;
    public int StartPos;
    public int CurrentPos;
    private Vector3 current;
    private Vector3 next;
    private int ListIndex;
    private InfoManager.PNum pNum;
    public SelectController nextPanel;
    public PlayerSelection SelectionHub;

    //positions used to simulate a scrolling ui effect
    private void Start()
    {
        BarPos = new List<Vector3>();
        BarPos.Add(new Vector3(300, 0, 0));
        BarPos.Add(new Vector3(100, 0, 0));
        BarPos.Add(new Vector3(-100, 0, 0));
        BarPos.Add(new Vector3(-300, 0, 0));

       
        CurrentPos = StartPos;
    }

    //get necessary info to assign controller
    public void InitializeController (InfoManager.PNum pnum, int IndexNum)
    {
        pNum = pnum;
        ListIndex = IndexNum;
        RewiredPlayer = ReInput.players.GetPlayer(pnum.ID);
        Panel = this.gameObject;
        ButtonBar = Panel.transform.Find("ButtonHolder").gameObject;
    }

    //check for ui input
    private void Update()
    {
        if (RewiredPlayer != null && Active)
        {
            GetInput();
            ProcessInput();                           
        }        
    }

    private void GetInput ()
    {
        moveLeft = RewiredPlayer.GetNegativeButtonDown("UIHorizontal");
        moveRight = RewiredPlayer.GetButtonDown("UIHorizontal");
        accept = RewiredPlayer.GetButtonDown("UISubmit");
        back = RewiredPlayer.GetButtonDown("UICancel");
    }

    //process
    private void ProcessInput ()
    {
        if (moveLeft)
        {   
            //vector to see where the next "button" would be
            Vector3 compair = new Vector3(ButtonBar.transform.localPosition .x + 200, 0, 0);
            //if that is good
            if (BarPos.Contains(compair))
            {
                //get the current position on the bar
                current = BarPos[CurrentPos];
                //get the next position on the bar
                next = BarPos[CurrentPos - 1];    
                //change index reference to current position
                CurrentPos = CurrentPos - 1;
                //transform the bar position to align the button to the panels center
                ButtonBar.transform.localPosition = next;
     
            } 
        }
        //same as above but in positive direction
        if (moveRight)
        {            
            Vector3 compair = new Vector3(ButtonBar.transform.localPosition.x - 200, 0, 0);
            
            if (BarPos.Contains(compair))
            {
               
                current = BarPos[CurrentPos];
                next = BarPos[CurrentPos + 1];
                CurrentPos = CurrentPos + 1;
                ButtonBar.transform.localPosition = next;
          
            } 
        }
        //confirm selection
        if (accept)
        {
            //if this character number isn't already in the index of choosen characters
            if (!InfoManager.Info.IndexOfCharactersAssigned.Contains(CurrentPos)) 
            { 
                //add the characters id to the pnum packet gotten in initialize controller  
                pNum.CharacterIDs.Add(CurrentPos);
                //add this id to the choosen characters list
                InfoManager.Info.IndexOfCharactersAssigned.Add(CurrentPos);
                //let the button know it should be no longer active, change its color
                Active = false;
                //this character is now ready to go
                Ready = true;
                //change the header from P whatever to ready
                Panel.transform.Find("Header").gameObject.GetComponent<Text>().text = "READY";
                //used to let the color change effect know which character to dim
                SelectionHub.CharacterPicked(CurrentPos);
                //store benched info
                pNum.Benched.Add(Benched);
                //activate the next panel if it exists
                if (nextPanel != null)
                {
                    nextPanel.Active = true;
                }
            }
         
        }
    }

}

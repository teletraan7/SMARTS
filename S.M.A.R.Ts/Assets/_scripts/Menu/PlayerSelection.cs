using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSelection : MonoBehaviour {

    //bool for how many people are playing
    public bool Solo;
    public bool Two;
    public bool Three;
    public bool Four;
    private List<int> ControllersAssigned;
    public GameObject[] Panels;

    private int MaxPlayerNum = 4;
    private int NumOfPlayers;

    public bool visible;
    public List<Image> Demolition = new List<Image>();
    public List<Image> Hacking = new List<Image>();
    public List<Image> Defender = new List<Image>();
    public List<Image> Support = new List<Image>();
    
    //reset lists
    private void Start()
    {
        ResetFields();       
    }

    //reset it all back to default
    public void ResetFields ()
    {
        InfoManager.Info.ResetList();
        InfoManager.Info.IndexOfCharactersAssigned = new List<int>();
        ResetCharacterPicked();
        ControllersAssigned = new List<int>();
        NumOfPlayers = 0;
        Solo = false;
        Two = false;
        Three = false;
        Four = false;
        foreach (GameObject Panel in Panels)
        {
            Panel.transform.Find("Header").GetComponent<Text>().text = "WAITING";
            Panel.GetComponent<SelectController>().CurrentPos = Panel.GetComponent<SelectController>().StartPos;
        }
    }

    //check for start inputs and A inputs
    private void Update()
    {
        for (int i = 0; i < ReInput.players.playerCount; i++)
        {
            if (ReInput.players.GetPlayer(i).GetButtonDown("JoinGame") && visible)
            {
                AssignController(i);
            }
            if (ReInput.players.GetPlayer(i).GetButton("UISubmit") && visible && InfoManager.Info.IndexOfCharactersAssigned.Count >= 4)
            {
                SceneManager.LoadScene(2);  
            }
        }
    }

    //assigning the controllers to players and characters
    private void AssignController(int RewiredID)
    {
       if (this.gameObject.activeInHierarchy)
        {
            //if 4 players already exist then do nothing
            if (NumOfPlayers >= MaxPlayerNum)
            {
                return;
            }
            //if this controller is already assigned then do nothing
            if (ControllersAssigned.Contains(RewiredID))
            {
                return;
            }
            //create a new Pnum pocket and store the id for this controller and what player number it is
            InfoManager.Info.pNum.Add(new InfoManager.PNum(RewiredID, NumOfPlayers += 1));
            //add this controller to a list of assigned controllers
            ControllersAssigned.Add(RewiredID);
          
          
            if (NumOfPlayers == 1)
            {
                Solo = true;
                Two = false;
                Three = false;
                Four = false;
            }
            if (NumOfPlayers == 2)
            {
                Two = true;
                Solo = false;
                Three = false;
                Four = false;
            }
            if (NumOfPlayers == 3)
            {
                Three = true;
                Two = false;
                Solo = false;
                Four = false;
            }
            if (NumOfPlayers == 4)
            {
                Four = true;
                Two = false;
                Three = false;
                Solo = false;
            }
            ChangePlayerText();
        }

    }

    
    //change the text to refelct the number of characters
    private void ChangePlayerText()
    {
        //check number of players
        if (Solo)
        {
            //make new text variable
            Text Txt;
            //get the text to the panel
            Txt = Panels[0].transform.Find("Header").GetComponent<Text>();
            //change it
            Txt.text = "P1";
            //assign this players pnum packet to this panel
            Panels[0].GetComponent<SelectController>().InitializeController(InfoManager.Info.pNum[0], 0);
            //change the active bool
            Panels[0].GetComponent<SelectController>().Active = true;
            //get the next panel ready
            Panels[0].GetComponent<SelectController>().nextPanel = Panels[1].GetComponent<SelectController>();
            //store which side of the controller this will be on in the pnum packet
            InfoManager.Info.pNum[0].isLeft.Add(true);
            //store its benched status
            Panels[0].GetComponent<SelectController>().Benched = false;
            //repeat

            Txt = Panels[1].transform.Find("Header").GetComponent<Text>();
            Txt.text = "P1";
            Panels[1].GetComponent<SelectController>().InitializeController(InfoManager.Info.pNum[0], 0);
            Panels[1].GetComponent<SelectController>().nextPanel = Panels[2].GetComponent<SelectController>();
            InfoManager.Info.pNum[0].isLeft.Add(false);
            Panels[1].GetComponent<SelectController>().Benched = false;

            Txt = Panels[2].transform.Find("Header").GetComponent<Text>();
            Txt.text = "Back Up";
            Panels[2].GetComponent<SelectController>().Benched = true;
            Panels[2].GetComponent<SelectController>().InitializeController(InfoManager.Info.pNum[0], 0);
            Panels[2].GetComponent<SelectController>().nextPanel = Panels[3].GetComponent<SelectController>();
            InfoManager.Info.pNum[0].isLeft.Add(false);

            Txt = Panels[3].transform.Find("Header").GetComponent<Text>();
            Txt.text = "Back Up";
            Panels[3].GetComponent<SelectController>().InitializeController(InfoManager.Info.pNum[0], 0);
            Panels[3].GetComponent<SelectController>().Benched = true;
            InfoManager.Info.pNum[0].isLeft.Add(false);
        }
        if (Two)
        {
            Text Txt;
            Txt = Panels[0].transform.Find("Header").GetComponent<Text>();
            Txt.text = "P1";
            Panels[0].GetComponent<SelectController>().InitializeController(InfoManager.Info.pNum[0], 0);
            Panels[0].GetComponent<SelectController>().Active = true;
            Panels[0].GetComponent<SelectController>().nextPanel = Panels[1].GetComponent<SelectController>();
            InfoManager.Info.pNum[0].isLeft.Add(true);
            Panels[0].GetComponent<SelectController>().Benched = false;

            Txt = Panels[1].transform.Find("Header").GetComponent<Text>();
            Txt.text = "P1";
            Panels[1].GetComponent<SelectController>().InitializeController(InfoManager.Info.pNum[0], 0);
            Panels[1].GetComponent<SelectController>().nextPanel = Panels[2].GetComponent<SelectController>();
            InfoManager.Info.pNum[0].isLeft.Add(false);
            Panels[1].GetComponent<SelectController>().Benched = false;

            Txt = Panels[2].transform.Find("Header").GetComponent<Text>();
            Txt.text = "P2";
            Panels[2].GetComponent<SelectController>().InitializeController(InfoManager.Info.pNum[1], 1);
            Panels[2].GetComponent<SelectController>().Active = true;
            Panels[2].GetComponent<SelectController>().nextPanel = Panels[3].GetComponent<SelectController>();
            InfoManager.Info.pNum[1].isLeft.Add(true);
            Panels[2].GetComponent<SelectController>().Benched = false;

            Txt = Panels[3].transform.Find("Header").GetComponent<Text>();
            Txt.text = "P2";
            Panels[3].GetComponent<SelectController>().InitializeController(InfoManager.Info.pNum[1], 1);
            InfoManager.Info.pNum[1].isLeft.Add(false);
            Panels[3].GetComponent<SelectController>().Benched = false;

        }
        if (Three)
        {
            Text Txt;
            Txt = Panels[0].transform.Find("Header").GetComponent<Text>();
            Txt.text = "P1";
            Panels[0].GetComponent<SelectController>().InitializeController(InfoManager.Info.pNum[0], 0);
            Panels[0].GetComponent<SelectController>().Active = true;
            Panels[0].GetComponent<SelectController>().nextPanel = Panels[3].GetComponent<SelectController>();
            InfoManager.Info.pNum[0].isLeft.Add(false);
            Panels[0].GetComponent<SelectController>().Benched = false;

            Txt = Panels[1].transform.Find("Header").GetComponent<Text>();
            Txt.text = "P2";
            Panels[1].GetComponent<SelectController>().InitializeController(InfoManager.Info.pNum[1], 1);
            Panels[1].GetComponent<SelectController>().Active = true;
            InfoManager.Info.pNum[1].isLeft.Add(false);
            Panels[1].GetComponent<SelectController>().Benched = false;

            Txt = Panels[2].transform.Find("Header").GetComponent<Text>();
            Txt.text = "P3";
            Panels[2].GetComponent<SelectController>().InitializeController(InfoManager.Info.pNum[2], 2);
            Panels[2].GetComponent<SelectController>().Active = true;
            InfoManager.Info.pNum[2].isLeft.Add(false);
            Panels[2].GetComponent<SelectController>().Benched = false;

            Txt = Panels[3].transform.Find("Header").GetComponent<Text>();
            Txt.text = "Back Up";
            Panels[3].GetComponent<SelectController>().InitializeController(InfoManager.Info.pNum[0], 0);
            Panels[3].GetComponent<SelectController>().Benched = true;
            InfoManager.Info.pNum[0].isLeft.Add(false);
            
        }
        if (Four)
        {
            Text Txt;
            Txt = Panels[0].transform.Find("Header").GetComponent<Text>();
            Txt.text = "P1";
            Panels[0].GetComponent<SelectController>().InitializeController(InfoManager.Info.pNum[0], 0);
            Panels[0].GetComponent<SelectController>().Active = true;
            InfoManager.Info.pNum[0].isLeft.Add(false);
            Panels[0].GetComponent<SelectController>().Benched = false;

            Txt = Panels[1].transform.Find("Header").GetComponent<Text>();
            Txt.text = "P2";
            Panels[1].GetComponent<SelectController>().InitializeController(InfoManager.Info.pNum[1], 1);
            Panels[1].GetComponent<SelectController>().Active = true;
            InfoManager.Info.pNum[1].isLeft.Add(false);
            Panels[1].GetComponent<SelectController>().Benched = false;

            Txt = Panels[2].transform.Find("Header").GetComponent<Text>();
            Txt.text = "P3";
            Panels[2].GetComponent<SelectController>().InitializeController(InfoManager.Info.pNum[2], 2);
            Panels[2].GetComponent<SelectController>().Active = true;
            InfoManager.Info.pNum[2].isLeft.Add(false);
            Panels[2].GetComponent<SelectController>().Benched = false;

            Txt = Panels[3].transform.Find("Header").GetComponent<Text>();
            Txt.text = "P4";
            Panels[3].GetComponent<SelectController>().InitializeController(InfoManager.Info.pNum[3], 3);
            Panels[3].GetComponent<SelectController>().Active = true;
            InfoManager.Info.pNum[3].isLeft.Add(false);
            Panels[3].GetComponent<SelectController>().Benched = false;
        }
    }

    //let the game know a character was picked by the player and change its color to show that
    public void CharacterPicked(int Index)
    {
        List<Image> Character = new List<Image>();

        if (Index == 0)
        {
            Character = Demolition;
        }
        if (Index == 1)
        {
            Character = Hacking;
        }
        if (Index == 2)
        {
            Character = Defender;
        }
        if (Index == 3)
        {
            Character = Support;
        }

        foreach (Image C in Character)
        {
            Color col = C.color;
            col.a = .25f;
            C.color = col;
        }
    }

    //set back to default
    public void ResetCharacterPicked()
    {

        foreach (Image I in Demolition)
        {
            Color col = I.color;
            col.a = 1f;
            I.color = col;
        }
        foreach (Image I in Hacking)
        {
            Color col = I.color;
            col.a = 1f;
            I.color = col;
        }
        foreach (Image I in Defender)
        {
            Color col = I.color;
            col.a = 1f;
            I.color = col;
        }
        foreach (Image I in Support)
        {
            Color col = I.color;
            col.a = 1f;
            I.color = col;
        }
    }

}

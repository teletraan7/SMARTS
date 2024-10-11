using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsManager : MonoBehaviour {

    //the base amount that the players can recieve without counting any bonus or penality. Different for each home
    static public int BasePay = 3350;
    //penalty for destroying walls will subtract from the score
    public int WallsDestroyed = 0;
    //they will be a set bonus amount that the players can recieve for not being detected. Each detection subtracts from that bonus till its gone
    public int TimesDetected = 0;
    //they will be a set bonus amount that the players can recieve for not being stunned. Each detection subtracts from that bonus till its gone
    public int TimesStuned = 0;
    //shutting down the generator will give a bonus
    public bool BotGeneratorShutDown = false;
    //not destroying machines will give a bonus that will be suvbtracted from for each destroyed machine
    public int MachinesDestroyed = 0;
    //destroying furnature will subtract from the overall score
    public int FurnitureDestroyed = 0;
    //get the alert level
    private int CurrentAlert = 0;
    //keep track of the highest it goes in the run
    private int HighestAlert = 0;
    //get the game manager
    private GameSessionManager gsm;
    //get the end scene ui manager
    public GameObject UICanvas;

    private void Start()
    {
        gsm = this.gameObject.GetComponent<GameSessionManager>();
        CurrentAlert = 0;
        HighestAlert = 0;
    }

    private void Update ()
    {
        CurrentAlert = gsm.currentSecurity;
        if (CurrentAlert > HighestAlert)
        {
            HighestAlert = CurrentAlert;
        }
    }

    public void CalculateScore ()
    {
        //take the number of wall & furnitrue destroyed and multiply them by a value, and subtract that sum from the base
        int wallpenalty = WallsDestroyed * 300;
        int furniturepenalty = FurnitureDestroyed * 25;
        int machinepenalty = MachinesDestroyed * 100;
        int penalties = wallpenalty + furniturepenalty + machinepenalty;
        //take all of the times the players where detected or stunned, compare them to the max number of times this can happen before no score is awarded, if able award a bonus based of of the ratio of time counted/max to the overall score
        int stunbonus = 500;
        if (TimesStuned >= 10)
        {
            stunbonus = 0;
        }
        if (TimesStuned >= 1 && TimesStuned < 10)
        {
            stunbonus = stunbonus / TimesStuned;
        }
        if (TimesStuned == 0)
        {
            stunbonus = 500;
        }
        int detectbonus = 500;
        if (TimesDetected >= 10)
        {
            detectbonus = 0;
        }
        if (TimesDetected >=1 && TimesDetected < 10)
        {
            detectbonus = detectbonus / TimesDetected;
        }
        if (TimesDetected == 0)
        {
            detectbonus = 500;
        }
        int cumulativebonus = detectbonus + stunbonus;
        //gather up other one time bonuses like turing off the generator and add them to the overall score
        int alertbonus = 900;
        if (HighestAlert >= 100)
        {
            alertbonus = 0;
        } if (HighestAlert > 66 & HighestAlert <= 99)
        {
            alertbonus = 300;
        } if (HighestAlert <= 66 & HighestAlert > 33)
        {
            alertbonus = 600;
        } if (HighestAlert <= 33 & HighestAlert >= 0)
        {
            alertbonus = 900;
        }
        int botgeneratorbonus = 1000;
        if (!BotGeneratorShutDown)
        {
            botgeneratorbonus = 0;
        }
        int lastbonus = alertbonus + botgeneratorbonus;
        //once score is calculated take that amount and send it to the end UI, display it on the end screen UI, lerp from 0 for effect
        int finalscore = (BasePay + lastbonus + cumulativebonus) - penalties;

        UICanvas.GetComponent<FinalManager>().FinalScore(finalscore);
    }
}

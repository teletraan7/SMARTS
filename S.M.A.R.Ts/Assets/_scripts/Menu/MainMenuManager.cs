using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    private AudioSource audiosource;

    public AudioClip Select;
    public AudioClip Click;

    public GameObject Facility;
    public GameObject Contract;


    private void Start()
    {
        audiosource = this.gameObject.GetComponent<AudioSource>();
    }

    public void Hover()
    {
        audiosource.clip = Click;
        audiosource.Play();
    }

    public void Submit ()
    {
        audiosource.clip = Select;
        audiosource.Play();
    }

    public void TestFacility ()
    {
        Contract.SetActive(false);
        Facility.SetActive(true);
    }

    public void Contracts ()
    {
        Facility.SetActive(false);
        Contract.SetActive(true);
    }

    public void DemoFacility ()
    {
        InfoManager.PNum demoInfo = new InfoManager.PNum(0, 1);
        bool[] BenchedBools = { false, true, true, true };
        bool[] LeftBools = { false, false, false, false };
        int[] CharIDs = { 0, 1, 2, 3 };
        AddToListBools(demoInfo, true, BenchedBools);
        AddToListBools(demoInfo, false, LeftBools);
        AddToListInts(demoInfo, CharIDs);
        InfoManager.Info.pNum = new List<InfoManager.PNum>();
        InfoManager.Info.pNum.Add(demoInfo);
        SceneManager.LoadScene(3);
    }

    public void HackFacility()
    {
        InfoManager.PNum hackInfo = new InfoManager.PNum(0, 1);
        bool[] BenchedBools = { true, false, true, true };
        bool[] LeftBools = { false, false, false, false };
        int[] CharIDs = { 0, 1, 2, 3 };
        AddToListBools(hackInfo, true, BenchedBools);
        AddToListBools(hackInfo, false, LeftBools);
        AddToListInts(hackInfo, CharIDs);
        InfoManager.Info.pNum = new List<InfoManager.PNum>();
        InfoManager.Info.pNum.Add(hackInfo);
        SceneManager.LoadScene(4);
    }

    public void DefFacility()
    {
        InfoManager.PNum defInfo = new InfoManager.PNum(0, 1);
        bool[] BenchedBools = { true, true, false, true };
        bool[] LeftBools = { false, false, false, false };
        int[] CharIDs = { 0, 1, 2, 3 };
        AddToListBools(defInfo, true, BenchedBools);
        AddToListBools(defInfo, false, LeftBools);
        AddToListInts(defInfo, CharIDs);
        InfoManager.Info.pNum = new List<InfoManager.PNum>();
        InfoManager.Info.pNum.Add(defInfo);
    }

    public void SuppFacility()
    {
        InfoManager.PNum suppInfo = new InfoManager.PNum(0, 1);
        bool[] BenchedBools = { true, true, true, false };
        bool[] LeftBools = { false, false, false, false };
        int[] CharIDs = { 0, 1, 2, 3 };
        AddToListBools(suppInfo, true, BenchedBools);
        AddToListBools(suppInfo, false, LeftBools);
        AddToListInts(suppInfo, CharIDs);
        InfoManager.Info.pNum = new List<InfoManager.PNum>();
        InfoManager.Info.pNum.Add(suppInfo);
    }

    public void TeamFacility ()
    {
        InfoManager.PNum teamInfo = new InfoManager.PNum(0, 1);
        bool[] BenchedBools = { false, true, true, false };
        bool[] LeftBools = { true, false, false, false };
        int[] CharIDs = { 0, 1, 2, 3 };
        AddToListBools(teamInfo, true, BenchedBools);
        AddToListBools(teamInfo, false, LeftBools);
        AddToListInts(teamInfo, CharIDs);
        InfoManager.Info.pNum = new List<InfoManager.PNum>();
        InfoManager.Info.pNum.Add(teamInfo);
        SceneManager.LoadScene(1);
    }

    private void AddToListBools(InfoManager.PNum pnum, bool Benched, bool[] bools)
    {
        if (Benched)
        {
            for (int i = 0; i < bools.Length; i++)
            {
                pnum.Benched.Add(bools[i]);
            }
        }
        if (!Benched)
        {
            for (int i = 0; i < bools.Length; i++)
            {
                pnum.isLeft.Add(bools[i]);
            }
        }
    }

    private void AddToListInts(InfoManager.PNum pnum, int[] IDs)
    {
        for (int i = 0; i < IDs.Length; i++)
        {
            pnum.CharacterIDs.Add(IDs[i]);
        }
    } 
}

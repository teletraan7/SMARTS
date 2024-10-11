using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseManager : MonoBehaviour {

    public static bool GamePaused = false;

    public GameObject PauseMenu;

    private Player player1;

    private Player player2;

    private Player player3;

    private Player player4;

    private Player WhoPaused = null;

    private List<Player> players = new List<Player>();

    public AudioClip ToggleOn;

    public AudioClip ToggleOff;

    public AudioClip Select;

    public AudioClip Click;

    private AudioSource audiosource;

    public GameObject GameManager;

    public GameObject ResumeButton;

    public bool GameOver;

    private void Awake()
    {
        //get any players in the game and add them to the list
        player1 = ReInput.players.GetPlayer(0);
        players.Add(player1);
        if (ReInput.players.GetPlayer(1) != null)
        {
            player2 = ReInput.players.GetPlayer(1);
            players.Add(player2);
        }

        if (ReInput.players.GetPlayer(2) != null)
        {
            player3 = ReInput.players.GetPlayer(2);
            players.Add(player3);
        }

        if (ReInput.players.GetPlayer(3) != null)
        {
            player4 = ReInput.players.GetPlayer(3);
            players.Add(player4);
        }
        audiosource = this.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
        //for every player in the list look out for them pressing pause
        foreach (Player player in players)
        {
           if (player.GetButtonDown("Paused") && !GameOver)
            {
                //check if game is paused and if this is the player who did it
                if (GamePaused && player == WhoPaused)
                {
                    Resume();//resume game
                }
                //else 
                else if (!GamePaused && WhoPaused == null)
                {
                    //pause and set the player so the game knows who is in control of this pause
                    Pause();
                    WhoPaused = player;
                }
            }
        }
	}

    private void Pause()//pause bool is true, show the menu, stop time
    {
        GamePaused = true;
        GameManager.GetComponent<AudioSource>().volume = .25f;
        audiosource.clip = ToggleOn;
        audiosource.Play();
        PauseMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(HighlightFirstButton());
        Time.timeScale = 0f;
    }

    IEnumerator HighlightFirstButton ()
    {
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(ResumeButton);
        yield break;
    }

    public void Resume()//pause bool is false, hide the menu, start time, set whopaused to null
    {
        GamePaused = false;
        GameManager.GetComponent<AudioSource>().volume = .50f;
        audiosource.clip = ToggleOff;
        audiosource.Play();
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        WhoPaused = null;
    }

    public void OptionsMenu()
    {
        
        audiosource.clip = Select;
        audiosource.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()//go to home menu
    {
        GamePaused = false;
        audiosource.clip = Select;
        audiosource.Play();
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()//quit game
    {
        GamePaused = false;
        audiosource.clip = Select;
        audiosource.Play();
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        Application.Quit();
    }

    public void EnterHover()
    {
        audiosource.clip = Click;
        audiosource.Play();
    }
}

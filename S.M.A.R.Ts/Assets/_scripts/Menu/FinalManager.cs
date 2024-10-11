using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class FinalManager : MonoBehaviour {

    public GameObject EndUI;
    public GameObject EyeCanvas;
    public GameObject ActiveCanvas;
    public Text PayText;
    private float lerp = 0f;
    public float duration;
    private bool UIisUp;
    private int FinalPay;
    private int BasePay;
    private float CustomDeltaTime;
    public AudioClip ToggleOn;
    public AudioClip ToggleOff;
    public AudioClip Select;
    public AudioClip Click;
    private AudioSource audiosource;
    public GameObject GameManger;
    public GameObject PlayAgainButton;
    public AudioClip ScoreUp;
    public AudioClip ScoreDown;

    private void Start()
    {
        BasePay = PointsManager.BasePay;
        CustomDeltaTime = Time.deltaTime;
        audiosource = this.gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
       if (UIisUp)
        {
            Time.timeScale = 0f;
            StartCoroutine(LerpPay());
        } 
    }

    private IEnumerator LerpPay ()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1);
            lerp += CustomDeltaTime / duration;
            int score = (int)Mathf.Lerp(BasePay, FinalPay, lerp);
            PayText.text = "$ " + score.ToString();
            if (score >= BasePay)
            {
                Color greenText = new Color();
                ColorUtility.TryParseHtmlString("#09FF00FF", out greenText);
                PayText.color = greenText;
            }
            else if (score < BasePay)
            {
                Color redText = new Color();
                ColorUtility.TryParseHtmlString("#FF0000FF", out redText);
                PayText.color = redText;
            }
        }
    }

    public void PlayAgain ()
    {
        audiosource.clip = Select;
        audiosource.Play();
        int Scene = SceneManager.GetActiveScene().buildIndex;
        Time.timeScale = 1f;
        SceneManager.LoadScene(Scene);
    }

    public void MainMenu ()
    {
        audiosource.clip = Select;
        audiosource.Play();
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void FinalScore (int payday)
    {
        GameManger.GetComponent<AudioSource>().volume = .25f;
        StartCoroutine(HighlightFirstButton());
        this.gameObject.GetComponent<PauseManager>().GameOver = true;
        FinalPay = payday;
        PayText.text = "$ " + BasePay.ToString();
        EndUI.SetActive(true);
        EyeCanvas.SetActive(false);
        ActiveCanvas.SetActive(false);
        UIisUp = true;
        if (payday >= BasePay)
        {
            audiosource.clip = ScoreUp;
            audiosource.Play();
        } else if (payday <= BasePay)
        {
            audiosource.clip = ScoreDown;
            audiosource.Play();
        }
    }

    IEnumerator HighlightFirstButton()
    {
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(PlayAgainButton);
        yield break;
    }

}

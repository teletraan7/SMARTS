using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;
using UnityEngine.UI;

// controls main menu stuff as well as maintains settings to be loaded in gameplay levels
public class GameSettingsManager : MonoBehaviour {

	// TO DO: transfer settings to a singleton or something on level load depending on needs

	public Animator mainMenuPanelAnim;
	public Animator settingsPanelAnim;

	// Rewired stuff
	private Player player;
	public int playerId = 0;

	// menu stuff
	public GameObject mainMenuPanel;
	public GameObject settingsPanel;
    public GameObject gametypePanel;
	public Button firstDefaultButton;
	public Button settingsDefaultButton;
	public Button settingsDoneButton;
    public Button tutorialButton;
    public Button SelBtn;
    public GameObject Facilities;
    public PlayerSelection PlSel;
    public Button FacilitiesBtn;

	void Awake() {
        Cursor.visible = false;
        Time.timeScale = 1f;
		player = ReInput.players.GetPlayer(playerId);
        StartCoroutine(CancelUI());
	}

	IEnumerator CancelUI() {
		while (true)
        {
            yield return new WaitForFixedUpdate();
            if (player.GetAnyButton() && !mainMenuPanel.activeInHierarchy)
            {
                mainMenuPanelAnim.SetTrigger("startPressed");
                StartCoroutine(HighlightButton(firstDefaultButton));
            }

            if (player.GetButton("UICancel") && settingsPanelAnim.GetBool("settingsEnabled"))
            {
                settingsPanelAnim.SetBool("settingsEnabled", false);
                StartCoroutine(HighlightButton(settingsDefaultButton));
            }

            if (player.GetButton("UICancel") && mainMenuPanelAnim.GetBool("gametypeEnabled") && mainMenuPanelAnim.GetBool("selectionEnabled") == false && mainMenuPanelAnim.GetBool("facilitesEnabled") == false)
            {
                mainMenuPanelAnim.SetBool("gametypeEnabled", false);
                StartCoroutine(HighlightButton(firstDefaultButton));
            }

            if (player.GetButton("UICancel") && mainMenuPanelAnim.GetBool("selectionEnabled"))
            {
                mainMenuPanelAnim.SetBool("selectionEnabled", false);
                StartCoroutine(HighlightButton(tutorialButton));
                PlSel.visible = false;
                yield return new WaitForSeconds(.5f);
                
            }

            if (player.GetButton("UICancel") && mainMenuPanelAnim.GetBool("facilitesEnabled"))
            {
                mainMenuPanelAnim.SetBool("facilitesEnabled", false);
                StartCoroutine(HighlightButton(tutorialButton));
                yield return new WaitForSeconds(.5f);

            }
        }

    }

	// turn on/off credits panel
	public void EnableCredits() {
		// check for settings first
		if (settingsPanelAnim.GetBool ("settingsEnabled")) {
			settingsPanelAnim.SetBool ("settingsEnabled", false);
		}
		
		if (mainMenuPanelAnim.GetBool("creditsEnabled")) {
			mainMenuPanelAnim.SetBool ("creditsEnabled", false);
		} else {
			mainMenuPanelAnim.SetBool ("creditsEnabled", true);
		}
	}

	// turn on/off settings panel
	public void EnableSettings() {
		// check for credits first
		if (mainMenuPanelAnim.GetBool ("creditsEnabled")) {
			mainMenuPanelAnim.SetBool ("creditsEnabled", false);
		}
			
		if (settingsPanel.activeInHierarchy) {
			settingsPanelAnim.SetBool ("settingsEnabled", false);
			StartCoroutine (HighlightButton (settingsDefaultButton));
		} else {
			settingsPanelAnim.SetBool ("settingsEnabled", true);
			StartCoroutine (HighlightButton (settingsDoneButton));
		}
	}

	IEnumerator HighlightButton(Button btn) {
		yield return new WaitForSeconds (1.05f);

		btn.Select ();
		btn.OnSelect (null);
	}

    public void GameTypeMenu ()
    {
        mainMenuPanelAnim.SetBool("gametypeEnabled", true);
        StartCoroutine(HighlightButton(tutorialButton));
    }

    public void SelectionMenu ()
    {
        mainMenuPanelAnim.SetBool("selectionEnabled", true);
        PlSel.visible = true;
        StartCoroutine(HighlightButton(SelBtn));
    }

    public void FacilitiesMenu ()
    {
        mainMenuPanelAnim.SetBool("facilitesEnabled", true);
        StartCoroutine(HighlightButton(FacilitiesBtn));
    }

	public void ExitGame() {
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}
}

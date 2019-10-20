using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public float delayForGettingDark = 2;
    public float timeToFullBlack = 6f;
    public GameObject gameName, newGame, howToPlay, about, exit;

    private void Start(){
        StartCoroutine(GettingDark());
    }
	
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            PlayGame();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            Tutorial();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            About();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            ExitGame();
        }
    }
    public void PlayGame(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void Tutorial(){
		Debug.Log("Tutorial");
	}

	public void About(){
		Debug.Log("About");
	}

	public void ExitGame(){
		Debug.Log("Exit");
		Application.Quit();
	}

    IEnumerator GettingDark()
    {
        yield return new WaitForSeconds(delayForGettingDark);
        gameName.GetComponent<Text>().CrossFadeColor(Color.black, timeToFullBlack, true, false);
        newGame.GetComponent<Text>().CrossFadeColor(Color.black, timeToFullBlack, true, false);
        howToPlay.GetComponent<Text>().CrossFadeColor(Color.black, timeToFullBlack, true, false);
        about.GetComponent<Text>().CrossFadeColor(Color.black, timeToFullBlack, true, false);
        exit.GetComponent<Text>().CrossFadeColor(Color.black, timeToFullBlack, true, false);
    }
}

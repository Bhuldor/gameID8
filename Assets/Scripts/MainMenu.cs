using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	public GameObject mainCamera;
	public AudioClip letsPlay;
    public float delayForGettingDark = 0.25f;
    public float timeToFullBlack = 2f;
    public GameObject gameName, newGame, howToPlay, about, exit;
	private AudioSource source {get {return GetComponent<AudioSource>();}}

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
		mainCamera.GetComponent<AudioSource>().Stop();
		StartCoroutine(LetsPlay());
    }
	void PlaySound (){
        source.PlayOneShot(letsPlay);
    }

	public void Tutorial(){
        SceneManager.LoadScene(3); 
	}

	public void About(){
        SceneManager.LoadScene("MainMenuAbout");
    }

	public void ExitGame(){
		Application.Quit();
	}

    IEnumerator GettingDark()
    {
        yield return new WaitForSeconds(delayForGettingDark);
        if (gameName != null)
        {
            gameName.GetComponent<Text>().CrossFadeColor(Color.black, timeToFullBlack, true, false);
            newGame.GetComponent<Text>().CrossFadeColor(Color.black, timeToFullBlack, true, false);
            howToPlay.GetComponent<Text>().CrossFadeColor(Color.black, timeToFullBlack, true, false);
            about.GetComponent<Text>().CrossFadeColor(Color.black, timeToFullBlack, true, false);
            exit.GetComponent<Text>().CrossFadeColor(Color.black, timeToFullBlack, true, false);
        }
    }

	IEnumerator LetsPlay(){
		PlaySound ();
        yield return new WaitForSeconds(3f);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);   
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

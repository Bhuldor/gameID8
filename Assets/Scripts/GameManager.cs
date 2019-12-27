    using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour{

    /*##privateSettings##*/
    //score
    public Text countText ;
    public Text countTextFinal;
    //floors
    public GameObject floorsPrefab;
    public GameObject floorsFaixaPrefab;
    public float maxTime = 0.1f;

    //Obstacles Prefabs
    public GameObject bus   ;
    public GameObject car;
    public GameObject cavalete;
    public GameObject trash;
    public GameObject trashBig;
    public GameObject vase;
    public GameObject glasses;

    //player
    public GameObject player;
    //Camera
    public GameObject mainCamera;
    public GameObject tutorialSound;

    //background
    public GameObject backgroundPrefab;
    public GameObject backgroundPrefabMiddle;
    public GameObject backgroundPrefabFront;
    public GameObject blindStartEffect;

    //audioSource
    public AudioSource audioSource;
    public AudioClip musicFast;
    public AudioClip musicMiddle;
    public AudioClip musicSlow;
    public AudioClip perigoFrente;
    public AudioClip perigoLado;
    public AudioClip pularIntroducao;
    public AudioClip buttonA;
    public AudioClip buttonS;
    public AudioClip buttonD;

    /*##privateSettings##*/
    //score
    public int score = 0;
    private float nextActionTimeBackground = 0.0f;
    private float nextActionTimeObstacle = 0.0f;
    private float nextActionTime = 0.0f;
    private float period = 0.25f;
    private float periodBackground = 10f;
    private float periodObstacle = 2f;
    //Spawn
    private float x = -22f;
    private float xBackground = 500f;
    private float xObstacle = 0f;
    private float yObstacle = 1.2f;
    private float zObstacle = 0f;
    private int i;
    private int countWaitFaixa = 0;
    private bool faixaPut = false;
    //general
    private int wichMusic = 0;
    private bool initialPause = false;

    public bool gameOver = false;
    public bool tutorialOn = true;
    public bool tutorialA = false;
    public bool tutorialS = false;
    public bool tutorialD = false;
    public bool canPlayTutorialSound = false;


    //pause

    private bool startGame = true;
    public bool gameIsPaused = false;
    public GameObject pausePanel;

    //Tutorial Visual

    private bool StartingTutorial;
    public GameObject TutorialPanel;
    public Text text_Tutorial;
    public RawImage img_barra;
    public GameObject PressioneAPanel;
    public Text text_A;
    public RawImage img_A;
    public GameObject PreesioneSPanel;
    public Text text_S;
    public RawImage img_S;
    public GameObject PreesioneDPanel;
    public RawImage img_D;
    public Text text_D;


    private void Start(){    
        gameOver = false;
        startGame = true;
        StartingTutorial = true;
        score = 0;
        countText.text = "";

        if (!tutorialOn){
            blindStartEffect.SetActive(false);
        }else{
            blindStartEffect.SetActive(true);
        }
        Time.timeScale = 1f;
        //Starts Floors
        SpawnFloors(35);
    }
    
    IEnumerator WaitToStart(){
        if (startGame)
        {
            Time.timeScale = 0f;
            PlaySound();
            float pauseEndTime = Time.realtimeSinceStartup + 3.5f;
            while (Time.realtimeSinceStartup < pauseEndTime)
            {
                yield return 0;
            }
            Time.timeScale = 1;
            startGame = false;
        }
        

        if (tutorialOn){
            mainCamera.GetComponent<AudioSource>().clip = pularIntroducao;
            mainCamera.GetComponent<AudioSource>().Play();
            mainCamera.GetComponent<AudioSource>().loop=false;
        }
    }

    void PlaySound (){
        mainCamera.GetComponent<AudioSource>().Play();
        mainCamera.GetComponent<AudioSource>().loop=false;
    }

    public void RestartGame(){
        var currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void ExitGame(){
		Application.Quit();
	}

    IEnumerator WaitToStartToPlaySound(){
        float pauseEndTime = Time.realtimeSinceStartup + 0.5f;
        while (Time.realtimeSinceStartup < pauseEndTime){
            yield return 0;
        }
        canPlayTutorialSound = true;
    }

    void Update () {
        if (!gameOver)
        {


            if (!initialPause)
            {
                StartCoroutine(WaitToStart());
                initialPause = true;
            }

            if (Time.time > nextActionTime)
            {
                nextActionTime += period;
                if (!tutorialOn)
                {
                    SetCountText();
                }
                SpawnFloors(1);
            }


            if (Time.time > nextActionTimeBackground)
            {
                nextActionTimeBackground += periodBackground;
                SpawnBackground();
            }

            if (!tutorialOn)
            {
                blindStartEffect.SetActive(false);
                if (player.transform.position.x < 1)
                {
                    nextActionTimeObstacle = Time.time;
                }
                else if (Time.time > nextActionTimeObstacle)
                {
                    periodObstacle = Random.Range(3f, 6f);
                    nextActionTimeObstacle += periodObstacle;
                    SpawnRandomObstacle();
                }
            }
            else if (tutorialOn)
            {
                Tutorial();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.W)){
                RestartGame();
            }
            else if (Input.GetKeyDown(KeyCode.D)){
                ExitGame();
            }
        }
        PauseGame();
    }

    void Tutorial()
    {
        blindStartEffect.SetActive(true);
        if (StartingTutorial)
        {
            ActivateSlowly(TutorialPanel, text_Tutorial, img_barra);
            StartCoroutine(WaitToShowPanel());
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            mainCamera.GetComponent<AudioSource>().Stop();
            tutorialOn = false;
            DesactivateSlowly(TutorialPanel, text_Tutorial, img_barra);
            DesactivateSlowly(PressioneAPanel, text_A, img_A);
        }
        if (Input.GetKeyDown(KeyCode.A) && !tutorialA)
        {
            canPlayTutorialSound = false;
            tutorialSound.GetComponent<AudioSource>().clip = buttonS;
            tutorialSound.GetComponent<AudioSource>().Play();
            tutorialSound.GetComponent<AudioSource>().loop = false;
            tutorialA = true;
            DesactivateSlowly(PressioneAPanel, text_A, img_A);
            ActivateSlowly(PreesioneSPanel, text_S, img_S);
        }
        if (Input.GetKeyDown(KeyCode.S) && tutorialA && !tutorialS)
        {
            canPlayTutorialSound = false;
            tutorialSound.GetComponent<AudioSource>().clip = buttonD;
            tutorialSound.GetComponent<AudioSource>().Play();
            tutorialSound.GetComponent<AudioSource>().loop = false;
            tutorialS = true;
            DesactivateSlowly(PreesioneSPanel, text_S, img_S);
            ActivateSlowly(PreesioneDPanel, text_D, img_D);
        }
        if (Input.GetKeyDown(KeyCode.D) && tutorialA && tutorialS && !tutorialD)
        {
            tutorialOn = false;
            tutorialD = true;
            DesactivateSlowly(PreesioneDPanel, text_D, img_D);
            DesactivateSlowly(TutorialPanel, text_Tutorial, img_barra);
            StartCoroutine(WaitAndDestroy());
        }
    }

    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(3f);
        Destroy(TutorialPanel);
        Destroy(PressioneAPanel);
        Destroy(PreesioneDPanel);
        Destroy(PreesioneSPanel);
    }
    IEnumerator WaitToShowPanel()
    {
        while (StartingTutorial)
        {
            yield return new WaitForSeconds(3f);
            if (tutorialOn && !tutorialA)
            {
                ActivateSlowly(PressioneAPanel, text_A, img_A);
                StartingTutorial = false;
            }
        }
    }

    void ActivateSlowly(GameObject obj, Text txt, RawImage rawimg)
    {
        Image img = obj.GetComponent<Image>();
        float rawimg_alpha = rawimg.color.a;
        float img_alpha = img.color.a;
        float txt_alpha = txt.color.a;
        img.CrossFadeAlpha(0.0f, 0.0f, true);
        txt.CrossFadeAlpha(0.0f, 0.0f, true);
        rawimg.CrossFadeAlpha(0.0f, 0.0f, true);
        obj.SetActive(true);
        rawimg.CrossFadeAlpha(rawimg_alpha, 0.5f, false);
        img.CrossFadeAlpha(img_alpha, 0.5f, false);
        txt.CrossFadeAlpha(txt_alpha, 0.5f, false);
    }

    void DesactivateSlowly(GameObject obj, Text txt, RawImage rawimg)
    {
        Image img = obj.GetComponent<Image>();
        rawimg.CrossFadeAlpha(0.0f, 0.5f, false);
        img.CrossFadeAlpha(0.0f, 0.5f, false);
        txt.CrossFadeAlpha(0.0f, 0.5f, false);
    }

    void SetCountText (){
        if(player.transform.position.x > 1)
        {
            score++;
            countText.text = "Score: " + score.ToString();
            countTextFinal.text = "Final Score: " + score.ToString();
        }
        

        //Difficult increase
        //Music Change
        if(score > 300){
            if (wichMusic == 0){
                wichMusic = 1;
                audioSource.clip = musicMiddle;
                audioSource.Play();
                audioSource.loop = true;
                periodObstacle = 6f;
            }
        }
        else if(score > 600){
            if (wichMusic == 1){
                wichMusic = 2;
                audioSource.clip = musicFast;
                audioSource.Play();
                audioSource.loop = true;
                periodObstacle = 3f;
            }
        }
    }

    public void SpawnBackground(){
        var instanceBackground = Instantiate(backgroundPrefab);
        var instanceBackgroundMiddle = Instantiate(backgroundPrefabMiddle);
        var instanceBackgroundFront = Instantiate(backgroundPrefabFront);
        instanceBackground.transform.position = new Vector3(xBackground, backgroundPrefab.transform.position.y, backgroundPrefab.transform.position.z);
        instanceBackgroundMiddle.transform.position = new Vector3(xBackground, backgroundPrefabMiddle.transform.position.y, backgroundPrefabMiddle.transform.position.z);
        instanceBackgroundFront.transform.position = new Vector3(xBackground, backgroundPrefabFront.transform.position.y, backgroundPrefabFront.transform.position.z);

        xBackground += 700f;
    }

    private void SpawnRandomObstacle(){
        int spawnRandom = Random.Range(0,25);
        int spawnRandomZ = Random.Range(0,2);
        AudioClip soundEffect = perigoFrente;
        GameObject instanceObstacle = bus;
        xObstacle = player.transform.position.x + 50f;
        zObstacle = 0f;
        yObstacle = 1.2f;

        if (player.transform.transform.position.z > 0){
            if (spawnRandomZ == 1){
                soundEffect = perigoFrente;
            }else{
                soundEffect = perigoLado;
            }
        }else{
            if (spawnRandomZ == 0){
                soundEffect = perigoFrente;
            }else{
                soundEffect = perigoLado;
            }
        }

        switch (spawnRandom){
            case 0:
            case 1:
            case 2:
                instanceObstacle = bus;
                yObstacle = -1.6f;
                zObstacle = 0f;
                break;
            case 3:
            case 4:
            case 5:
                instanceObstacle = car;
                zObstacle = 0f;
                break;
            case 6:
            case 7:
            case 8:
            case 9:
                instanceObstacle = cavalete;
                xObstacle = player.transform.position.x + 35f;
                instanceObstacle.GetComponent<AudioSource>().clip = soundEffect;
                yObstacle = 1f;
                if (spawnRandomZ == 0){zObstacle = -3f;}
                else{zObstacle = 3f;}
                break;
            case 10:
            case 11:
            case 12:
            case 13:
                instanceObstacle = trash;
                xObstacle = player.transform.position.x + 35f;
                instanceObstacle.GetComponent<AudioSource>().clip = soundEffect;
                yObstacle = 1f;
                if (spawnRandomZ == 0){zObstacle = -3f;}
                else{zObstacle = 3f;}
                break;
            case 14:
            case 15:
            case 16:
                instanceObstacle = glasses;
                yObstacle = -2.6f;
                if (spawnRandomZ == 0){zObstacle = -3.5f;}
                else{zObstacle = 2f;}
                break;
            case 17:
            case 18:
            case 19:
            case 20:
                instanceObstacle = vase;
                xObstacle = player.transform.position.x + 35f;
                instanceObstacle.GetComponent<AudioSource>().clip = soundEffect;
                yObstacle = 0.5f;
                if (spawnRandomZ == 0){zObstacle = -3f;}
                else{zObstacle = 3f;}
                break;
            case 21:
            case 22:
            case 23:
            case 24:
                instanceObstacle = trashBig;
                xObstacle = player.transform.position.x + 35f;
                instanceObstacle.GetComponent<AudioSource>().clip = soundEffect;
                yObstacle = 0.69f;
                if (spawnRandomZ == 0){zObstacle = -3f;}
                else{zObstacle = 3f;}
                break;                
        }

        var instance = Instantiate(instanceObstacle);
        instance.transform.position = new Vector3(xObstacle, yObstacle, zObstacle);
    }

    public void SpawnFloors(int count){
        i = Random.Range(0,10);
        GameObject instanceObject = floorsPrefab;

        for (int c = 0; c < count; c++){
            x += 2f;

            if (i < 2 && !faixaPut){
                instanceObject = floorsFaixaPrefab;
                faixaPut = true;
            }else{
                instanceObject = floorsPrefab;
            }

            if (faixaPut){
                countWaitFaixa++;

                if (countWaitFaixa > 125){
                    faixaPut = false;
                }
            }

            var instance = Instantiate(instanceObject);
            instance.transform.position = new Vector3(x, 0f, 0f);
        }
    }

    public void PauseGame() {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameIsPaused && !gameOver && !tutorialOn)
        {
            gameIsPaused = true;
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
            audioSource.Pause();
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && gameIsPaused)
        {
            UnpauseGame();
        }
        if(gameIsPaused && Input.GetKeyDown(KeyCode.Q))
        {
            ExitGame();
        }
    }

    public void UnpauseGame()
    {
        gameIsPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        audioSource.UnPause();
    }
}

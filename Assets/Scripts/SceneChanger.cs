using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Video;

public class SceneChanger : MonoBehaviour
{

	private int minutes = 1;
	private float interval = 1;
	public bool hasUser;
	public float timeLimit = 15;
    public float sessionTimoutLimit;
	private VideoPlayer vid;
	public int sceneCycle;
	public GameObject sceneController;
	public bool activeScene;
	private bool sessionOpen;
    public bool endScene;
	private Logger logger;
    public AudioMixerSnapshot noSound;
    public AudioMixerSnapshot soundUp;


    // Use this for initialization
    void Start()
	{        
		logger = GameObject.Find("Logger").GetComponent<Logger>();
		sessionOpen = false;
		TimeLapse();
		vid = this.GetComponent<VideoPlayer>();
        endScene = false;
        

        if (SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 4)
        {
            sceneCycle = SceneManager.GetActiveScene().buildIndex + 1;
            activeScene = false;
            vid.Play();
        }
        else
        {
            activeScene = true;
        }
    }

	// Update is called once per frame
	void Update()
	{
		if (activeScene)
		{
			hasUser = sceneController.GetComponent<CycleMultiImages>().hasUser;
            if (minutes > sessionTimoutLimit && !hasUser)
            {
                if (sessionOpen)
                {
                    sessionOpen = false;
                    logger.AppendLog("Session closed: " + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                }
            }
            else if (hasUser)
            {
                minutes = 0;
                if (!sessionOpen)
                {
                    sessionOpen = true;
                    logger.AppendLog("Session opened: " + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                }
            }
            if (endScene)
            {
                noSound.TransitionTo(2f);
                StartCoroutine(ChangeScene());
            }
            if (!endScene && minutes > timeLimit && !hasUser)
            {
                endScene = true;
            }
            else if (!endScene && Input.GetKey(KeyCode.Space))
			{
                endScene = true;               
            }
            else if (!endScene && !hasUser)
            {
                soundUp.TransitionTo(2f);
            } 
		}
		else
		{            
            if (!vid.isPlaying)
			{
				StartCoroutine(ChangeScene());
			}
        }
	}

    //fade to black over time and set the next scene to be viewed
    //unless it's at the last scene, then reset to zero
    IEnumerator ChangeScene()
	{
        float fadeTime = GetComponentInChildren<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
		sceneCycle = SceneManager.GetActiveScene().buildIndex + 1;
		if (sceneCycle >= SceneManager.sceneCountInBuildSettings)
		{
			SceneManager.LoadScene(0);
		}
		else
		{
			SceneManager.LoadScene(sceneCycle);
		}
	}

    //Some simultaneous loading stuff that never worked, and isn't being used

	//IEnumerator loadNext()
	//{
	//	activeScene = true;
	//	Application.backgroundLoadingPriority = ThreadPriority.BelowNormal; 
	//	AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneCycle);
	//	asyncLoad.allowSceneActivation = false;
	//	while (!asyncLoad.isDone)
	//	{
	//		float fadeTime = GetComponentInChildren<Fading>().BeginFade(1);
	//		yield return new WaitForSeconds(fadeTime);
	//		//Output the current progress
	//		Debug.Log("Loading progress: " + (asyncLoad.progress * 100) + "%");

	//		// Check if the load has finished
	//		if (asyncLoad.progress >= 0.9f)
	//		{
	//			asyncLoad.allowSceneActivation = true;
	//		}
	//	}
	//}
	
	public void TimeLapse()
	{
		minutes = minutes + 1;
		Invoke("TimeLapse", interval);
		//Debug.Log(minutes);
	}
}

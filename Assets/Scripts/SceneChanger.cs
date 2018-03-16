using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Video;

public class SceneChanger : MonoBehaviour
{

    private int minutes = 1;
    private float interval = 1;
    public bool hasUser;
    public float timeLimit = 15;
    private VideoPlayer vid;
    public int sceneCycle;
    public GameObject sceneController;
    private bool activeScene;

    // Use this for initialization
    void Start()
    {
        TimeLapse();
        vid = this.GetComponent<VideoPlayer>();
        if (SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 4)
        {
            sceneCycle = SceneManager.GetActiveScene().buildIndex + 1;
            activeScene = false;
            vid.Play();
            StartCoroutine(loadNext());
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
            if (minutes > timeLimit && !hasUser)
            {
                StartCoroutine(ChangeScene());
            }
            else if (Input.GetKeyDown("Space"))
            {
                StartCoroutine(ChangeScene());
            }
            else if (hasUser)
            {
                minutes = 0;
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

    IEnumerator loadNext()
    {
        {
            yield return new WaitForSeconds(4f);
            SceneManager.LoadSceneAsync(sceneCycle);            
        }
}

public void TimeLapse()
    {
        minutes = minutes + 1;
        Invoke("TimeLapse", interval);
        Debug.Log(minutes);
    }
}

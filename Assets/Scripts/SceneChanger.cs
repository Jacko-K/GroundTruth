using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{

    private int minutes = 1;
    private float interval = 1;
    public bool hasUser =  true;
    public float timeLimit = 5;
    public int nextSceneNumber = 0;
    
    // Use this for initialization
    void Start()
    {
        TimeLapse();       
    }

    // Update is called once per frame
    void Update()
    {
        if (minutes > timeLimit)
        {
          StartCoroutine(ChangeScene());
        }
      
    }
    //fade to black over time and set the next scene to be viewed
    IEnumerator ChangeScene()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            float fadeTime = GetComponentInChildren<Fading>().BeginFade(1);
            yield return new WaitForSeconds(fadeTime);
            nextSceneNumber = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(0);
        }
    }
    
    public void TimeLapse()
    {
        minutes = minutes + 1;
        Invoke("TimeLapse", interval);
        Debug.Log(minutes);
    }
}

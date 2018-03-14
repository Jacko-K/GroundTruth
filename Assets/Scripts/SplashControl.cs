using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class SplashControl : MonoBehaviour {

    private VideoPlayer vid;
    public int sceneCycle;

	// Use this for initialization
	void Start () {
        vid = this.GetComponent<VideoPlayer>();
        sceneCycle = this.GetComponent<SceneChanger>().nextSceneNumber;
        if (sceneCycle > 3)
        {
            sceneCycle = 1;
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (!vid.isPlaying)
            {
                SceneManager.LoadScene(sceneCycle);
                Debug.Log("load next scene");
            }
        }
		
	}
}

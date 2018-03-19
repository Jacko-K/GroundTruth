using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class Fading : MonoBehaviour {

    public Texture2D fadeOutTexture;
    public float fadeSpeed;

    private int drawdepth = -1000;
    private float fadeAlpha = 1.0f;
    private int fadeDir = -1;

    void OnGUI()
    {
        fadeAlpha += fadeDir * fadeSpeed * Time.deltaTime;
        fadeAlpha = Mathf.Clamp01(fadeAlpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, fadeAlpha);
        GUI.depth = drawdepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
    }

    public float BeginFade(int direction)
    {
        fadeDir = direction;
        return(fadeSpeed);
    }
		
}


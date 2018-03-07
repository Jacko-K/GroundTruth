using UnityEngine;
using System.Collections;

public class CycleImages : MonoBehaviour {

    public Texture[] images;
    public int currentTexture;
    public float blendAmount;
    public float GlobalSpeed;
    
    private Renderer r;
    private bool blending;
    private float timer;

    // Use this for initialization
    void Start () {

        System.Array.Sort(images, (a, b) => a.name.CompareTo(b.name));

        blendAmount = 0;
        currentTexture = 0;
        r = GetComponent<Renderer>();
        r.material.SetTexture("_MainTex", images[currentTexture]);
        r.material.SetTexture("_NextTex", images[currentTexture+1]);
        currentTexture++;
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
        string text = "Year: " + images[currentTexture].name.Substring(0,4);
        GUI.Label(rect, text, style);
    }

    // Update is called once per frame
    void Update() {

        if (Input.GetKey(KeyCode.UpArrow))
        {
            Blend(1.0f);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            Blend(-1.0f);
        }

        if (blendAmount > 1.0f)
        {
            if (currentTexture < images.Length)
            {
                r.material.SetTexture("_MainTex", images[currentTexture]);
                r.material.SetTexture("_NextTex", images[currentTexture + 1]);
                currentTexture++;
                blendAmount = 0.0f;
            }
        }
        else if (blendAmount < 0.0f)
        {
            if (currentTexture > 1)
            {
                currentTexture--;
                r.material.SetTexture("_MainTex", images[currentTexture - 1]);
                r.material.SetTexture("_NextTex", images[currentTexture]);
                blendAmount = 1.0f;
            }
        }

        blendAmount = Mathf.Clamp(blendAmount, 0.0f, 1.0f);

        if (blending)
            r.material.SetFloat("_Percentage", blendAmount);
        
    }

    public void Blend(float speed = 1.0f)
    {
        blending = true;
        blendAmount += Time.deltaTime * (GlobalSpeed * speed);
    }
}

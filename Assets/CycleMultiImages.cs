using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CycleMultiImages : MonoBehaviour
{

    public Texture[] images_Main;
    public Texture[] images_Alt;

    public int currentTexture;
    public float blendAmountX;
    public float blendAmountY;
    public float GlobalSpeed = 1.0f;

    private Renderer r;
    private bool blending;
    private float timer;
    public Text yearText;
    public bool hasUser;
    public bool hasInput;

    // Use this for initialization
    void Start()
    {

        System.Array.Sort(images_Main, (a, b) => a.name.CompareTo(b.name));
        System.Array.Sort(images_Alt, (a, b) => a.name.CompareTo(b.name));
        Init();
    }

    void Init()
    {
        blendAmountX = 0;
        blendAmountY = 0;
        currentTexture = 0;
        r = GetComponent<Renderer>();
        ImageSwap(currentTexture, currentTexture + 1);
        currentTexture++;
        hasInput = false;
    }

    void OnGUI()
    {
        //int w = Screen.width, h = Screen.height;

        //GUIStyle style = new GUIStyle();

        //Rect rect = new Rect(0, 0, w, h * 2 / 100);
        //style.alignment = TextAnchor.UpperLeft;
        //style.fontSize = h * 2 / 100;
        //style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);

        string text = "";
        if (currentTexture < 0)
        {
            text = "";
        }
        else
        {
            text = "" + images_Main[currentTexture].name.Substring(0, 4);
        }
        yearText.text = text;
    }

    // Update is called once per frame
    void Update()
    {

        GetInput();

        if (hasUser || hasInput)
        {
            GlobalSpeed = 1.0f;
            BlendImages();
        }
        else if (!hasUser && !hasInput)
        {
            BlendY(1.0f);
            GlobalSpeed = 0.5f;
            BlendImages();
        }

    }

    public void GetInput()
    {
        
        if (Input.GetKey(KeyCode.UpArrow))
        {
            hasInput = true;
            BlendY(1.0f);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            hasInput = true;
            BlendY(-1.0f);
        }
        else
        {
            hasInput = false;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            hasInput = true;
            BlendX(-1.0f);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            hasInput = true;
            BlendX(1.0f);
        }
    }

    public void BlendImages()
    {
        if (blendAmountY > 1.0f)
        {
            if (currentTexture < 0)
            {
                currentTexture = 1;
                ImageSwap(0, currentTexture);
                blendAmountY = 0.0f;
            }
            else if (currentTexture < images_Main.Length - 1)
            {
                ImageSwap(currentTexture, currentTexture + 1);
                currentTexture++;
                blendAmountY = 0.0f;
            }
            else
            {
                ImageSwap(currentTexture, 0);
                currentTexture = -1;
                blendAmountY = 0.0f;
            }
        }
        else if (blendAmountY < 0.0f)
        {
            if (currentTexture > 1)
            {
                currentTexture--;
                ImageSwap(currentTexture - 1, currentTexture);
                blendAmountY = 1.0f;
            }
        }

        blendAmountX = Mathf.Clamp(blendAmountX, 0.0f, 1.0f);
        blendAmountY = Mathf.Clamp(blendAmountY, 0.0f, 1.0f);

        if (blending)
        {
            r.material.SetFloat("_YPercentage", blendAmountY);
            r.material.SetFloat("_XPercentage", blendAmountX);
            blending = false;
        }
    }

    void ImageSwap(int first, int second)
    {
        r.material.SetTexture("_MainTex", images_Main[first]);
        r.material.SetTexture("_NextTex", images_Main[second]);
        r.material.SetTexture("_MainTex1", images_Alt[first]);
        r.material.SetTexture("_NextTex1", images_Alt[second]);
    }

    public void BlendY(float speed = 1.0f)
    {
        blending = true;
        blendAmountY += Time.deltaTime * (GlobalSpeed * speed);
    }

    public void BlendX(float speed = 1.0f)
    {
        blending = true;
        blendAmountX += Time.deltaTime * (GlobalSpeed * speed);
    }
}

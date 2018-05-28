using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Logger : MonoBehaviour
{
	private static Logger instanceRef;
	private StreamWriter writer;
	public string filename;

	void Awake()
	{
		if (instanceRef == null)
		{
			instanceRef = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			DestroyImmediate(gameObject);
        }
	}

	void Start()
	{
		writer = new StreamWriter(Application.dataPath + "/" + filename, true);
        AppendLog("");
        AppendLog("!!! Application started !!!");

	}

	private void OnDisable()
	{
		if (writer != null)
            writer.Close();
	}

	public void AppendLog(string line)
	{
		writer.WriteLine(line);
        writer.Flush();
        //Debug.Log(line);
	}

}

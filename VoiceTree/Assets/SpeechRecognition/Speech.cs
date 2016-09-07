using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine.UI;

public class Speech : MonoBehaviour {
    public int AudioDeviceNum;
    public Text out_text;

    [DllImport("SpeechRecognition_dll")]
    private static extern void Init(int m_num);
    [DllImport("SpeechRecognition_dll")]
    private static extern void Stop();
    [DllImport("SpeechRecognition_dll", CharSet = CharSet.Unicode)]
    private static extern int GetRecognitionLabel(StringBuilder label);

    // Use this for initialization
    void Start () {
        Init(AudioDeviceNum);
	}
	
	// Update is called once per frame
	void Update () {
        StringBuilder label = new StringBuilder(1024);
        if (GetRecognitionLabel(label)!=0)
        {
            out_text.text = label.ToString();
            Debug.Log(label);
        }
    }

    void OnApplicationQuit()
    {
        Stop();
    }
}

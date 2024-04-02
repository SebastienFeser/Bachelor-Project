using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
public class ArduinoDataReader : MonoBehaviour
{
    Thread thread;
    SerialPort data_stream = new SerialPort("COM3", 9600);
    JsonDataGatherer jsonDataGatherer;

    string receivedData;
    float receivedDataFloat;
    float readTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        data_stream.Open();
        thread = new Thread(ThreadMethod);
        thread.Start();
        
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
    void ThreadMethod ()
    {
        receivedData = data_stream.ReadLine();
        receivedDataFloat = float.Parse(receivedData);
        Debug.Log(receivedDataFloat);
        ThreadMethod();
    }

    void OnApplicationQuit()
    {
        thread.Abort();
    }
    
    public float GetPulse()
    {
        return receivedDataFloat;
    }

    public void StopThread()
    {
        thread.Abort();
    }

}


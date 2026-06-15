using UnityEngine;
using UnityEngine.Events;
using System.IO.Ports;

public class ArduinoCheck : MonoBehaviour
{
    public string portName = "COM3";
    public bool isClosed = false;
    SerialPort serial;
    public UnityEvent OnClosed;
    public UnityEvent OnOpened;
    public UnityEvent OnShake;

    void Start()
    {
        
        serial = new SerialPort(portName, 9600);
        serial.ReadTimeout = 50;
        serial.Open();
    }

    void Update()
    {
        if (serial.IsOpen)
        {
            try
            {
                string data = serial.ReadLine().Trim();

                if (data == "CLOSED" && !isClosed)
                {
                    isClosed = true;
                    Debug.Log("がま口：閉じた");
                    OnClosed?.Invoke(); 
                }
                else if (data == "OPEN" && isClosed)
                {
                    isClosed = false;
                    Debug.Log("がま口：開いた");
                    OnOpened?.Invoke(); 
                }
                if(data == "SHAKE")
                {
                Debug.Log("Shake!");
                OnShake?.Invoke();
                }
            }
            catch (System.TimeoutException) { }
        }
    }

    void OnApplicationQuit()
    {
        if (serial != null && serial.IsOpen)
        {
            serial.Close();
        }
    }
}
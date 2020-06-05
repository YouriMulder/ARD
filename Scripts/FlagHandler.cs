using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagHandler : MonoBehaviour
{
    float speed_current = 10f;
    float frequency_current = 1f;
    float amplitude_current = 0.2f;

    // Update is called once per frame
    protected void UpdateValues() {
        GetComponent<Renderer>().material.SetFloat("_Speed", speed_current);
        GetComponent<Renderer>().material.SetFloat("_Frequency", frequency_current);
        GetComponent<Renderer>().material.SetFloat("_Amplitude", amplitude_current);
    }

    protected void Start() {
        UpdateValues();
    }

    void OnGUI() {
        GUI.Label(new Rect(500, 500, 200, 40), "speed: " + GetComponent<Renderer>().material.GetFloat("_Speed").ToString());
        GUI.Label(new Rect(500, 600, 200, 40), "freq: " + GetComponent<Renderer>().material.GetFloat("_Frequency").ToString());
        GUI.Label(new Rect(500, 700, 200, 40), "amp: " + GetComponent<Renderer>().material.GetFloat("_Amplitude").ToString());
    }
}

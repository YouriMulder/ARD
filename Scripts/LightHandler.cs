using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class GetLux {

    private static AndroidJavaObject activityContext = null;
    private static AndroidJavaObject jo = null;
    private static AndroidJavaClass activityClass = null;


    public static void Start() {
    #if UNITY_ANDROID
        activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");

        jo = new AndroidJavaObject("com.etiennefrank.lightsensorlib.LightSensorLib");
        jo.Call("init", activityContext);
    #endif
    }

    public static float GetFloat() {
    #if UNITY_ANDROID
        return jo.Call<float>("getLux");
    #endif
    }
}


public class LightHandler : MonoBehaviour {
    public Camera main_camera;
    public Light light;
    
    private const float lux_max = 100;
    private const float light_intensity_max = 1;

    // Start is called before the first frame update
    void Start() {
        GetLux.Start();
        main_camera = Camera.main;
        light = GetComponent<Light>();
    }

    float ClipLux(float lux) {
        lux = lux > lux_max ? lux_max : lux;
        return lux;
    }

    float CalculateNewLightIntensity(float lux) {
        lux = ClipLux(lux);
        const float lux_light_intensity_factor =  light_intensity_max / lux_max;
        return lux * lux_light_intensity_factor;
    }

    void Update() {
        transform.position = main_camera.transform.position;

        float lux = GetLux.GetFloat();
        light.intensity = CalculateNewLightIntensity(lux);
    }

    void OnGUI() {
        GUI.Label(new Rect(500, 300, 200, 40), "Lux: " + GetLux.GetFloat().ToString());
        GUI.Label(new Rect(500, 400, 200, 40), "Lux: " + light.intensity.ToString());
    }
}
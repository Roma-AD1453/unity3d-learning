using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    private ParticleSystem ps;
    ParticleSystem.MainModule mainModule;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        mainModule = ps.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 50), "Red"))
        {
            mainModule.startColor = Color.red;
        }
        if (GUI.Button(new Rect(100, 0, 100, 50), "Yellow"))
        {
            mainModule.startColor = Color.yellow;
        }
        if (GUI.Button(new Rect(200, 0, 100, 50), "Blue"))
        {
            mainModule.startColor = Color.blue;
        }
    }

}

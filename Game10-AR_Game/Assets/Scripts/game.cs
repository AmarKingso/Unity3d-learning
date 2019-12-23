using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class game : MonoBehaviour, IVirtualButtonEventHandler
{
    private GameObject sphere;
    public float step = 1f;
    private Color[] color = { Color.red, Color.blue, Color.yellow, Color.black };
    private System.Random rd = new System.Random();

    void IVirtualButtonEventHandler.OnButtonPressed(VirtualButtonBehaviour vbb) {
        int index = (int)rd.Next(4);
        sphere.GetComponent<MeshRenderer>().material.color = color[index];
        Debug.Log(color[index]);
    }

    void IVirtualButtonEventHandler.OnButtonReleased(VirtualButtonBehaviour vbb) {
        Debug.Log("released");
    }

    // Start is called before the first frame update
    void Start()
    {
        VirtualButtonBehaviour[] vbbs = GetComponentsInChildren<VirtualButtonBehaviour>();
        foreach(var vbb in vbbs) {
            vbb.RegisterEventHandler(this);
        }

        sphere = transform.Find("Sphere").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

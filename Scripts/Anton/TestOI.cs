using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using XboxCtrlrInput;

public class TestOI : MonoBehaviour {
    public int TestPlayerID = 0;
    public string PlayerString = "Horizontal";
    public bool enable = true;
    private string cur;
    private int id;

    [SerializeField]
    private XboxController controller;
    // Use this for initialization
    void Start () {
        Debug.developerConsoleVisible = true;
        XCI.DEBUG_LogControllerNames();
	}
	
	// Update is called once per frame
	void Update () {
        if (!enable)
            return;
        if (XCI.GetButton(XboxButton.A, XboxController.Any))
        {
            
            if (XCI.GetButton(XboxButton.A, XboxController.Second))
                Debug.Log("Second");
            if (XCI.GetButton(XboxButton.A, XboxController.Third))
                Debug.Log("Third");
            if (XCI.GetButton(XboxButton.A, XboxController.First))
                Debug.Log("First");
        }
	}
}

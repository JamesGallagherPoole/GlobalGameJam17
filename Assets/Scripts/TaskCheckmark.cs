using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskCheckmark : MonoBehaviour
{
    private bool isChecked = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckOffTask()
    {
        isChecked = true;
        Debug.Log("Checked Off!");
    }
}

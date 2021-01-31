using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskCheckmark : MonoBehaviour
{
    private bool isChecked = false;
    private int currentGameState;

    GameStateManager gameStateManager;

    // Start is called before the first frame update
    void Start()
    {
        gameStateManager = GameObject.Find("GameStateManager").GetComponent<GameStateManager>();        
        gameStateManager.gameStateChangeEvent.AddListener(UpdateState);
    }

    void OnDestroy()
    {
        gameStateManager.gameStateChangeEvent.RemoveListener(UpdateState);
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

    public void UpdateState(int newGameState)
    {
        currentGameState = newGameState;
        Debug.Log("Current State: " + currentGameState);
    }
}

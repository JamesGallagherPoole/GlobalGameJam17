using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GameStateChangeEvent : UnityEvent<int> {}

public class GameStateManager : MonoBehaviour
{
    // Game State 0 == Worst
    // Game State 2 == Best
    private int gameState = 1;

    public GameStateChangeEvent gameStateChangeEvent;

    void Start()
    {
        gameStateChangeEvent.Invoke(gameState);
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void GameStateIncrease()
    {
        gameState += 1;
        gameStateChangeEvent.Invoke(gameState);
    }

    public void GameStateDecrease()
    {
        gameState -= 1;
        gameStateChangeEvent.Invoke(gameState);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public GameStateManager gameStateManager;
    public AudioSource audioHigh;
    public AudioSource audioLow;

    private int currentGameState;

    // Start is called before the first frame update
    void Start()
    {
        gameStateManager.gameStateChangeEvent.AddListener(UpdateState);
    }

    void OnDestroy()
    {
        gameStateManager.gameStateChangeEvent.RemoveListener(UpdateState);       
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Music State at: " + currentGameState);
        if (currentGameState == 0) {
            audioHigh.volume = 0;
            audioLow.volume = 1;
        } else if (currentGameState > 0) {
            audioHigh.volume = 1;
            audioLow.volume = 0;
        }
    }

    private void UpdateState(int newGameState)
    {
        currentGameState = newGameState;
    }
}

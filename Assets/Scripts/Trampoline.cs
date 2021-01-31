using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    // Start is called before the first frame update
    private int currentGameState;

    GameStateManager gameStateManager;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        gameStateManager = GameObject.Find("GameStateManager").GetComponent<GameStateManager>();        
        gameStateManager.gameStateChangeEvent.AddListener(UpdateState);

        animator = GetComponent<Animator>();
    }

    void OnDestroy()
    {
        gameStateManager.gameStateChangeEvent.RemoveListener(UpdateState);
    }   

    // Update is called once per frame
    void Update()
    {
        if (currentGameState == 0)
            animator.Play("TrampolineLow");
        else if (currentGameState == 1)
            animator.Play("TrampolineMedium");
        else if (currentGameState == 2)
            animator.Play("TrampolineHigh");
    }

    private void UpdateState(int newGameState)
    {
        currentGameState = newGameState;
    }
}

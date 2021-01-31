using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskCheckmark : MonoBehaviour
{
    private bool isChecked = false;
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
        if (isChecked) {
            if (currentGameState == 0)
                animator.Play("LowChecked");
            else if (currentGameState == 1)
                animator.Play("MediumChecked");
            else if (currentGameState == 2)
                animator.Play("HighChecked");
        } else if (!isChecked) {
            if (currentGameState == 0)
                animator.Play("LowUnchecked");
            else if (currentGameState == 1)
                animator.Play("MediumUnchecked");
            else if (currentGameState == 2)
                animator.Play("HighUnchecked");
        }
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSwitcher : MonoBehaviour
{
    Tilemap tilemap;

    public TileBase[] codeTiles;
    public TileBase[] artTiles;
    public TileBase[] soundTiles;
    public TileBase[] productionTiles;
    public TileBase[] levelDesignTiles;
    public TileBase[] seamlessTiles;

    int currentState = 1;
    public GameStateManager gameStateManager;

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        gameStateManager.gameStateChangeEvent.AddListener(UpdateState);
    }

    public void SwitchTilesUp()
    {
        if (currentState < 2)
        {
            tilemap.SwapTile(codeTiles[currentState], codeTiles[currentState + 1]);
            tilemap.SwapTile(artTiles[currentState], artTiles[currentState + 1]);
            tilemap.SwapTile(soundTiles[currentState], soundTiles[currentState + 1]);
            tilemap.SwapTile(productionTiles[currentState], productionTiles[currentState + 1]);
            tilemap.SwapTile(levelDesignTiles[currentState], levelDesignTiles[currentState + 1]);
            tilemap.SwapTile(seamlessTiles[currentState], seamlessTiles[currentState + 1]);
        }
        else
        {
            return;
        }
    }

    public void SwitchTilesDown()
    {
        if(currentState > 0)
        {
            tilemap.SwapTile(codeTiles[currentState], codeTiles[currentState - 1]);
            tilemap.SwapTile(artTiles[currentState], artTiles[currentState - 1]);
            tilemap.SwapTile(soundTiles[currentState], soundTiles[currentState - 1]);
            tilemap.SwapTile(productionTiles[currentState], productionTiles[currentState - 1]);
            tilemap.SwapTile(levelDesignTiles[currentState], levelDesignTiles[currentState - 1]);
            tilemap.SwapTile(seamlessTiles[currentState], seamlessTiles[currentState - 1]);
        }
        else
        {
            return;
        }
    }

    public void UpdateState(int newGameState)
    {
        currentState = newGameState;
        Debug.Log("Current State: " + currentState);
    }
    void OnDestroy()
    {
        gameStateManager.gameStateChangeEvent.RemoveListener(UpdateState);
    }

}

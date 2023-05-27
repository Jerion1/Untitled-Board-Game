using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {turnStart, turnRollDice, turnMove, turnFieldAction, combatStart, combatAttack, combatDefend, combatEnd};

public class GameLogic : MonoBehaviour
{
    public GameState gameState;
    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.turnStart;
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case GameState.turnStart:
                break;
            case GameState.turnRollDice:
                break;
            case GameState.turnMove:
                break;
            case GameState.turnFieldAction:
                break;
            case GameState.combatStart:
                break;
            case GameState.combatEnd:
                break;
            case GameState.combatDefend:
                break;
            case GameState.combatAttack:
                break;
        }
    }
}

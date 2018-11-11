using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// La classe mère de l'ensemble des stratégies
public abstract class Strategie : MonoBehaviour {

    int nbVictoires = 0;
    int nbMatches = 0;

    abstract public MyInput GetInput(GameState gs);
    abstract public void Load();
    abstract public void Unload();
    abstract public void Reward(GameState S, MyInput A, GameState newS, float r);

    public virtual void RegisterVictory() {
        nbVictoires++;
        nbMatches++;
    }
    public virtual void RegisterDefeat() {
        nbMatches++;
    }
    public float GetWinrate() {
        return (float)nbVictoires / (float)nbMatches;
    }
}

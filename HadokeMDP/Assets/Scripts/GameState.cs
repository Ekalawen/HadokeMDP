using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GameState {

    public enum PlayerNumber { PLAYER_1, PLAYER_2 };
    static int nbPlayers = 2;

    PlayerState playerState1;
    PlayerState playerState2;
    PlayerNumber playerNumber;
    int id;

    public GameState(PlayerState playerState1, PlayerState playerState2, PlayerNumber playerNumber)
    {
        this.playerState1 = playerState1;
        this.playerState2 = playerState2;
        this.playerNumber = playerNumber;
        SetId();
    }
    void SetId() {
        id = (PlayerState.nbPlayerStates * PlayerState.nbPlayerStates) * (int)playerNumber + (PlayerState.nbPlayerStates) * playerState1.GetId() + playerState2.GetId();
    }

    public int GetId() {
        return id;
    }

    public static List<GameState> AllStates()
    {
        List<GameState> states = new List<GameState>();
        for(int i = 0; i < nbPlayers; i++) {
            foreach(PlayerState ps1 in PlayerState.AllStates()) {
                foreach(PlayerState ps2 in PlayerState.AllStates()) {
                    states.Add(new GameState(ps1, ps2, (PlayerNumber)i));
                }
            }
        }
        Debug.Log("Nombre d'états possible du jeu = " + states.Count);
        return states;
    }

    public override string ToString() {
        return "(P1= " + playerState1 + ", P2= " + playerState2 + ", " + playerNumber + ")";
    }

    //public override bool Equals(object obj) {
    //    if (obj == null || this == null) return false;
    //    GameState autre = obj as GameState;
    //    return playerState1.Equals(autre.playerState1) && playerState2.Equals(autre.playerState2) && playerNumber == autre.playerNumber;
    //}

    //public override int GetHashCode()
    //{
    //    var hashCode = 1822735354;
    //    hashCode = hashCode * -1521134295 + EqualityComparer<PlayerState>.Default.GetHashCode(playerState1);
    //    hashCode = hashCode * -1521134295 + EqualityComparer<PlayerState>.Default.GetHashCode(playerState2);
    //    hashCode = hashCode * -1521134295 + playerNumber.GetHashCode();
    //    hashCode = hashCode * -1521134295 + id.GetHashCode();
    //    return hashCode;
    //}
}

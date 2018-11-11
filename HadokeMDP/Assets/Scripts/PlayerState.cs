using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerState {
    // public int pv; // Je ne pense pas qu'il faille modifier le comportement en fonction des points de vie des joueurs, et puis ça accélèrera les calculs :)
    // Bon pour le moment je le laisse, on sait jamais, je ne suis pas une intelligence omnisciente après tout ^^'
    // Non ça prend vraiment vraiment trop trop de place ! x)
    PlayerScript.Etat etat;
    int position;
    int id;

    static int nbEtats = Enum.GetValues(typeof(PlayerScript.Etat)).Length;
    public static int nbPlayerStates = nbEtats * PlayerScript.nbPositions;

    public PlayerState(int position, PlayerScript.Etat etat)
    {
        this.position = position;
        this.etat = etat;
        SetId();
    }
    void SetId() {
        id = (PlayerScript.nbPositions) * (int)etat + position;
    }

    public int GetId() {
        return id;
    }

    public static List<PlayerState> AllStates()
    {
        List<PlayerState> states = new List<PlayerState>();
        for(int i = 0; i < nbEtats; i ++) {
            for(int k = 0; k < PlayerScript.nbPositions; k++) {
                states.Add(new PlayerState(k, (PlayerScript.Etat)i));
            }
        }
        return states;
    }

    public override string ToString() {
        return "(position= " + position + ", " + Enum.GetName(etat.GetType(), etat) + ")";
    }

    //public override bool Equals(object obj) {
    //    if (obj == null || this == null) return false;
    //    PlayerState autre = obj as PlayerState;
    //    return etat == autre.etat && position == autre.position;
    //}

    //public override int GetHashCode()
    //{
    //    var hashCode = -170254029;
    //    hashCode = hashCode * -1521134295 + etat.GetHashCode();
    //    hashCode = hashCode * -1521134295 + position.GetHashCode();
    //    hashCode = hashCode * -1521134295 + id.GetHashCode();
    //    return hashCode;
    //}
}

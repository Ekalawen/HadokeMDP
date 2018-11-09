using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Classe contenant l'ensemble des inputs d'un player pour une frame ! =)
public class MyInput {
    // Alors un player peu appuyer sur l'une des 4 touches d'action à chaque tour
    public enum Coup
    {
        NOTHING,
        PUNCH,
        KICK,
        UPPERCUT,
        PROTECT
    }

    // Et il peut se déplacer dans l'une des 2 directions
    public enum Deplacement
    {
        NOTHING,
        LEFT,
        RIGHT
    }

    public Coup coup;
    public Deplacement deplacement;
    public int etat;

    public MyInput(Coup coup, Deplacement deplacement)
    {
        this.coup = coup;
        this.deplacement = deplacement;
        this.etat = 10 * (int)coup + (int)deplacement;
    }

    public void SetCoup(Coup coup) {
        this.coup = coup;
        this.etat = 10 * (int)coup + (int)deplacement;
    }
    public void SetDeplacement(Deplacement deplacement) {
        this.deplacement = deplacement;
        this.etat = 10 * (int)coup + (int)deplacement;
    }
    public void Reset()
    {
        this.coup = Coup.NOTHING;
        this.deplacement = Deplacement.NOTHING;
        this.etat = 10 * (int)coup + (int)deplacement;
    }

    public override string ToString() {
        return "(" + Enum.GetName(coup.GetType(), coup) + ", " + Enum.GetName(deplacement.GetType(), deplacement) + ")";
    }
}

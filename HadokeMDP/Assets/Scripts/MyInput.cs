using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Classe contenant l'ensemble des inputs d'un player pour une frame ! =)
[Serializable]
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
    static int nbCoup = Enum.GetValues(typeof(Coup)).Length;

    // Et il peut se déplacer dans l'une des 2 directions
    public enum Deplacement
    {
        NOTHING,
        LEFT,
        RIGHT
    }
    static int nbDeplacement = Enum.GetValues(typeof(Deplacement)).Length;
    public static int nbInputs = nbCoup * nbDeplacement;

    Coup coup;
    Deplacement deplacement;
    int id;

    public Coup GetCoup() {
        return coup;
    }
    public Deplacement GetDeplacement() {
        return deplacement;
    }
    public int GetId() {
        return id;
    }


    public MyInput(Coup coup, Deplacement deplacement)
    {
        this.coup = coup;
        this.deplacement = deplacement;
        this.id = 0;
        SetId();
    }
    void SetId() {
        this.id = nbDeplacement * (int)coup + (int)deplacement;
    }

    public void SetCoup(Coup coup) {
        this.coup = coup;
        SetId();
    }
    public void SetDeplacement(Deplacement deplacement) {
        this.deplacement = deplacement;
        SetId();
    }
    public void Reset()
    {
        this.coup = Coup.NOTHING;
        this.deplacement = Deplacement.NOTHING;
        SetId();
    }

    public override string ToString() {
        return "(" + Enum.GetName(coup.GetType(), coup) + ", " + Enum.GetName(deplacement.GetType(), deplacement) + ")";
    }

    public static MyInput Random()
    {
        int valCoup = UnityEngine.Random.Range(0, nbCoup);
        int valDeplacement = UnityEngine.Random.Range(0, nbDeplacement);
        return new MyInput((Coup)valCoup, (Deplacement)valDeplacement);
    }

    public static List<MyInput> AllInputs()
    {
        List<MyInput> inputs = new List<MyInput>();
        for(int i = 0; i < nbCoup; ++i) { 
            for(int j = 0; j < nbDeplacement; ++j) {
                inputs.Add(new MyInput((Coup)i, (Deplacement)j));
            }
        }
        return inputs;
    }
}

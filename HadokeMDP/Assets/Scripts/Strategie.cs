using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// La classe mère de l'ensemble des stratégies
public abstract class Strategie : MonoBehaviour {

    abstract public MyInput GetInput();
    abstract public void Load();
    abstract public void Unload();

}

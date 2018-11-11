using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrategieInactive : Strategie {

    public override MyInput GetInput(GameState gs) {
        return new MyInput(MyInput.Coup.NOTHING, MyInput.Deplacement.NOTHING);
    }

    public override void Load() {
    }

    public override void Reward(GameState S, MyInput A, GameState newS, float r) {
    }

    public override void Unload() {
    }
}

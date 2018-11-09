using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrategieInactive : Strategie {

    public override MyInput GetInput() {
        return new MyInput(MyInput.Coup.NOTHING, MyInput.Deplacement.NOTHING);
    }

    public override void Load() {
    }

    public override void Unload() {
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrategiePlayer : Strategie {

    MyInput input;

    public override MyInput GetInput() {
        MyInput tmpInput = new MyInput(input.coup, input.deplacement);
        input.Reset();
        return tmpInput;
    }

    public override void Load() {
    }

    public override void Unload() {
    }

    void Start() {
        input = new MyInput(MyInput.Coup.NOTHING, MyInput.Deplacement.NOTHING);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            input.SetCoup(MyInput.Coup.PUNCH);
        } else if (Input.GetKeyDown(KeyCode.Z)) {
            input.SetCoup(MyInput.Coup.KICK);
        } else if (Input.GetKeyDown(KeyCode.E)) {
            input.SetCoup(MyInput.Coup.UPPERCUT);
        } else if (Input.GetKeyDown(KeyCode.LeftShift)) {
            input.SetCoup(MyInput.Coup.PROTECT);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            input.SetDeplacement(MyInput.Deplacement.LEFT);
        } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            input.SetDeplacement(MyInput.Deplacement.RIGHT);
        }
    }
}

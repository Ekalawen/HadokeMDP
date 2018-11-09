using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrategiePlayer : Strategie {

    public KeyCode punchButton;
    public KeyCode kickButton;
    public KeyCode uppercutButton;
    public KeyCode protectButton;

    public KeyCode leftButton;
    public KeyCode rightButton;

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
        if (Input.GetKeyDown(punchButton)) {
            input.SetCoup(MyInput.Coup.PUNCH);
        } else if (Input.GetKeyDown(kickButton)) {
            input.SetCoup(MyInput.Coup.KICK);
        } else if (Input.GetKeyDown(uppercutButton)) {
            input.SetCoup(MyInput.Coup.UPPERCUT);
        } else if (Input.GetKeyDown(protectButton)) {
            input.SetCoup(MyInput.Coup.PROTECT);
        }

        if (Input.GetKeyDown(leftButton)) {
            input.SetDeplacement(MyInput.Deplacement.LEFT);
        } else if (Input.GetKeyDown(rightButton)) {
            input.SetDeplacement(MyInput.Deplacement.RIGHT);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerScript : MonoBehaviour {

    public static int leftSideTerrain = 0;
    public static int rightSideTerrain = 10;

    public enum Etat {
        IDLE,
        PUNCH_START,
        PUNCH_END,
        KICK_START,
        KICK_MIDDL,
        KICK_END,
        UPPERCUT_START,
        UPPERCUT_MIDDLE,
        UPPERCUT_END,
        PROTECT,
        DAMMAGED
    };

    [HideInInspector]
    public Etat etat = Etat.IDLE;
    public int pv;
    public int position;
    public bool isLeftplayer;

    public Transform displayPosition;
    public SpriteRenderer sprite;

    public List<Sprite> sprites;

    public void Display() {
        // On change le sprite
        sprite.sprite = sprites[(int)etat];

        // On update la position
        Vector3 oldPosition = displayPosition.position;
        float x = (float)position / (float)rightSideTerrain * 20f - 10f;
        displayPosition.position = new Vector3(x, oldPosition.y, oldPosition.z);

        // On tourne le sprite si c'est le joueur de droite
        if (!isLeftplayer) {
            Vector3 oldScale = displayPosition.localScale;
            displayPosition.localScale = new Vector3(Mathf.Min(oldScale.x, -oldScale.x), oldScale.y, oldScale.z);
        }
    }

    public override string ToString() {
        return "(pv= " + pv + ", position= " + position + ", " + Enum.GetName(etat.GetType(), etat) + ")";
    }

    public void setInitialPosition(int position)
    {
        this.position = position;
    }

    public bool isDead()
    {
        return pv <= 0;
    }

    public void ApplyInput(MyInput input, int positionAutrePlayer) {
        switch(etat)
        {
            case Etat.IDLE:
                ApplyInputCoup(input);
                ApplyInputDeplacement(input, positionAutrePlayer);
                break;
            case Etat.PUNCH_START:
                etat = Etat.PUNCH_END;
                ApplyInputDeplacement(input, positionAutrePlayer);
                break;
            case Etat.PUNCH_END:
                etat = Etat.IDLE;
                break;
            case Etat.KICK_START:
                etat = Etat.KICK_MIDDL;
                ApplyInputDeplacement(input, positionAutrePlayer);
                break;
            case Etat.KICK_MIDDL:
                etat = Etat.KICK_END;
                break;
            case Etat.KICK_END:
                etat = Etat.IDLE;
                break;
            case Etat.UPPERCUT_START:
                etat = Etat.UPPERCUT_MIDDLE;
                ApplyInputDeplacement(input, positionAutrePlayer);
                break;
            case Etat.UPPERCUT_MIDDLE:
                etat = Etat.UPPERCUT_END;
                break;
            case Etat.UPPERCUT_END:
                etat = Etat.IDLE;
                break;
            case Etat.PROTECT:
                etat = Etat.IDLE;
                ApplyInputDeplacement(input, positionAutrePlayer);
                break;
            case Etat.DAMMAGED:
                ApplyInputCoup(input);
                ApplyInputDeplacement(input, positionAutrePlayer);
                break;
        }
    }

    void ApplyInputCoup(MyInput input)
    {
        switch(input.coup)
        {
            case MyInput.Coup.NOTHING:
                etat = Etat.IDLE;
                break;
            case MyInput.Coup.PUNCH:
                etat = Etat.PUNCH_START;
                break;
            case MyInput.Coup.KICK:
                etat = Etat.KICK_START;
                break;
            case MyInput.Coup.UPPERCUT:
                etat = Etat.UPPERCUT_START;
                break;
            case MyInput.Coup.PROTECT:
                etat = Etat.PROTECT;
                break;
        }
    }
    
    void ApplyInputDeplacement(MyInput input, int positionAutrePlayer)
    {
        int positionTmp = position;
        switch(input.deplacement)
        {
            case MyInput.Deplacement.LEFT:
                positionTmp = Mathf.Max(leftSideTerrain, position - 1);
                break;
            case MyInput.Deplacement.RIGHT:
                positionTmp = Mathf.Min(rightSideTerrain, position + 1);
                break;
        }
        if (positionTmp != positionAutrePlayer)
        {
            position = positionTmp;
        }
    }

    public bool ApplyDammage(PlayerScript autrePlayer)
    {
        switch(autrePlayer.etat)
        {
            case Etat.PUNCH_END:
                if(isInDistance(autrePlayer, 1))
                {
                    if (etat != Etat.PROTECT)
                    {
                        pv--;
                        return true;
                    }
                }
                break;
            case Etat.KICK_END:
                if(isInDistance(autrePlayer, 2))
                {
                    if (etat != Etat.PROTECT)
                    {
                        pv--;
                        return true;
                    }
                }
                break;
            case Etat.UPPERCUT_END:
                if(isInDistance(autrePlayer, 1))
                {
                    pv--;
                    return true;
                }
                break;
        }
        return false;
    }

    bool isInDistance(PlayerScript autrePlayer, int distance)
    {
        return Mathf.Abs(autrePlayer.position - position) <= distance;
    }
}

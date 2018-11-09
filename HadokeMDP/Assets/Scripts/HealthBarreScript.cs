using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarreScript : MonoBehaviour {

    public float tailleMax;
    public RectTransform vie;

    public void SetLife(float pourcentage)
    {
        Debug.Log("pourcentage = " + pourcentage);
        Vector3 oldPosition = vie.localPosition;
        Debug.Log("oldPosition = " + oldPosition);
        vie.localPosition = new Vector3(- tailleMax * (1 - pourcentage), oldPosition.y, oldPosition.z);
        Debug.Log("X vie = " + ((float)tailleMax * ((float)1 - pourcentage)));
    }

}

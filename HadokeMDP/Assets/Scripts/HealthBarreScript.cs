using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarreScript : MonoBehaviour {

    public float tailleMax;
    public RectTransform vie;

    public void SetLife(float pourcentage)
    {
        Vector3 oldPosition = vie.localPosition;
        vie.localPosition = new Vector3(- tailleMax * (1 - pourcentage), oldPosition.y, oldPosition.z);
    }

}

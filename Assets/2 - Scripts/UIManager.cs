using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public RectTransform panelObject1;

    void FixedUpdate()

    {

        if (Input.GetMouseButtonDown(0))
        {

            panelObject1.gameObject.SetActive(false);


        }

    }

}
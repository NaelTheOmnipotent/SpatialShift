using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanvasScript : MonoBehaviour
{
    [SerializeField] private CanvasGroup shiftingCanvasGroup;

    private PlayerScript playerScript;

    private void Start()
    {
        playerScript = GetComponentInParent<PlayerScript>();
        shiftingCanvasGroup.HideCanvasGroup();
    }

    private void Update()
    {
        if (playerScript.hasTeleported)
        {
            shiftingCanvasGroup.ShowCanvasGroup();
        }
        else
        {
            shiftingCanvasGroup.HideCanvasGroup();
        }
    }
}

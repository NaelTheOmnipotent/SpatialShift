using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteDeathAnim : MonoBehaviour
{
    [SerializeField] private float destroyTime;
    private void OnEnable()
    {
        Destroy(gameObject, destroyTime);
    }
}

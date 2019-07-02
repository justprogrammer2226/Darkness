using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverInteraction : MonoBehaviour
{
    public Lever currentLever;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentLever != null)
        {
            currentLever.isActive = !currentLever.isActive;
        }
    }
}

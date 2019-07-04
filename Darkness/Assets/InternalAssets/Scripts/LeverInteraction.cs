using UnityEngine;

public class LeverInteraction : MonoBehaviour
{
    public Lever currentLever;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentLever != null)
        {
            if(currentLever.IsActive) currentLever.Deactivate();
            else currentLever.Activate();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Lever : MonoBehaviour
{
    public GameObject stick;
    public bool isActive;
    public float speed;

    private void Update()
    {
        if(isActive)
        {
            stick.transform.rotation = Quaternion.RotateTowards(stick.transform.rotation, Quaternion.Euler(0, 0, -40), speed * Time.deltaTime);
        }
        else
        {
            stick.transform.rotation = Quaternion.RotateTowards(stick.transform.rotation, Quaternion.Euler(0, 0, 40), speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            Debug.Log("ОТРАБОТАЛО");
            LeverInteraction leverInteraction = col.gameObject.GetComponent<LeverInteraction>();
            leverInteraction.currentLever = this;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            LeverInteraction leverInteraction = col.gameObject.GetComponent<LeverInteraction>();
            leverInteraction.currentLever = null;
        }
    }
}

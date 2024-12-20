using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCart : MonoBehaviour
{
    public float speed = 3;
    public float distance = 5;

    private Vector3 startPos;
    private Vector3 lastPos;
    private CharacterController playerOnPlatform;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        lastPos = startPos;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = transform.position;
        newPos.z = startPos.z + (Mathf.Sin(Time.time * speed) * distance);

        Vector3 deltaMovement = newPos - lastPos;

        transform.position = newPos;
        lastPos = newPos; 

        if (playerOnPlatform != null)
        {
            playerOnPlatform.Move(deltaMovement);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController playerController = other.GetComponent<CharacterController>();
            if (playerController != null)
            {
                playerOnPlatform = playerController;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerOnPlatform != null && other.GetComponent<CharacterController>() == playerOnPlatform)
            {
                playerOnPlatform = null;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class LiftDoor : MonoBehaviour
{
    [SerializeField] private float liftHeight = 3f;      // how high the door lifts
    [SerializeField] private float liftSpeed = 5f;       // how fast it moves
    [SerializeField] private float delayBeforeDrop = 2f; // time before dropping back

    private Vector3 originalPosition;
    private Vector3 targetLiftPosition;
    private bool isLifting = false;
    private bool isDropping = false;

    void Start()
    {
        originalPosition = transform.position;
        targetLiftPosition = originalPosition + Vector3.up * liftHeight;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isLifting && !isDropping)
        {
            StartCoroutine(LiftThenDrop());
        }
    }

    private System.Collections.IEnumerator LiftThenDrop()
    {
        isLifting = true;

        // Lifting
        while (Vector3.Distance(transform.position, targetLiftPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetLiftPosition, liftSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetLiftPosition;
        yield return new WaitForSeconds(delayBeforeDrop);

        // Dropping
        isDropping = true;
        while (Vector3.Distance(transform.position, originalPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, liftSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = originalPosition;
        isLifting = false;
        isDropping = false;
    }
}


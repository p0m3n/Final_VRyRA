using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterGameDoor : MonoBehaviour
{
    private int runesActivated;
    [SerializeField] private int requiredRunes;
    [SerializeField] private GameObject pointB;
    [SerializeField] private float speed = 10f;

    private Vector3 targetPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        runesActivated = 0;
        targetPosition = pointB.transform.position;
    }

    public void CounterOfRunesActivated()
    {
        runesActivated++;
        if (runesActivated >= requiredRunes)
            StartCoroutine(MoveDoor());
    }

    IEnumerator MoveDoor()
    {
        while ((targetPosition - gameObject.transform.position).sqrMagnitude > 0.01f)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
    }
}

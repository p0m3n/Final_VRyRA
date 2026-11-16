using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnterGameDoor : MonoBehaviour
{
    public UnityEvent pEvent;
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
        {
            pEvent.Invoke();
            StartCoroutine(MoveDoor());
        }
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

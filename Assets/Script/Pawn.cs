using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class Pawn : MonoBehaviour, ISubject
{
    public PawnData data;
    public Transform[] cells;

    public List<IObserver> observers = new List<IObserver>();

    public int backwardSteps;
    public int imprisonTurns;
    public bool imprisonUsed;

    void Awake()
    {
        data.currentPosition = 0;
        backwardSteps = data.backwardSteps;

    }

    private void Start()
    {
    }

    private void StartPawnMovement(int steps,UnityAction callBack)
    {
        StartCoroutine(MovePawnByStep(steps, callBack));
    }

    private IEnumerator MovePawnByStep(int steps,UnityAction callBack)
    {
        for (int i = 1; i <= steps; i++)
        {
            Move(1);
            yield return new WaitForSeconds(.5f);
        }
        if (imprisonTurns == 0)
        {
            EndTurn();
            callBack?.Invoke();
        }
        else
        {
            imprisonTurns--;
        }
    }
    public void RollDice(int steps,UnityAction callBack)
    {
        StartPawnMovement(steps, callBack);
    }

    public void Move(int steps)
    {
        Debug.Log("move"+ data.currentPosition+steps);
        data.currentPosition += steps;
        data.currentPosition = Mathf.Clamp(data.currentPosition, 0, cells.Length - 1);
        transform.position = cells[data.currentPosition].position;
    }

    public void UseBackwardsPower()
    {

        if (backwardSteps > 0)
        {
            StartCoroutine(MovePawnByStep());
        }
        EndTurn();
    }

    public void UseImprisonPower()
    {
        if (!imprisonUsed)
        {
            imprisonTurns = data.imprisonTurns;
            imprisonUsed = true;
        }
        EndTurn();
    }
    private IEnumerator MovePawnByStep()
    {
        for (int i = backwardSteps; i > 0; i--)
        {
            Debug.Log(i);
            Move(-1);
            yield return new WaitForSeconds(.5f);
        }
        backwardSteps = 0;
    }

    private void EndTurn()
    {
        Notify();
    }
    public void Attach(IObserver observer)
    {
        observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void Notify()
    {
        foreach (IObserver observer in observers)
        {
            observer.UpdateObserver();
        }
    }
}
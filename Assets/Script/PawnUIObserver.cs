using System;
using UnityEngine;
using UnityEngine.UI;

public class PawnUIObserver : MonoBehaviour, IObserver
{
    public Text currentPositionText;
    public Text backwardStepsText;
    public Text imprisonTurnsText;

    public Pawn pawn;

    private void Start()
    {
        pawn.Attach(this);
        UpdateObserver();
    }

    public void UpdateObserver()
    {
        currentPositionText.text = "Position: " + pawn.data.currentPosition;
        backwardStepsText.text = "Backward Steps: " + pawn.backwardSteps;
        imprisonTurnsText.text = "Imprison Turns: " + (pawn.imprisonUsed ? 0:1).ToString() ;
    }
}
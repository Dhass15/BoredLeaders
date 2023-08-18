using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "PawnData", menuName = "Pawn/Pawn Data", order = 1)]
public class PawnData : ScriptableObject
{
    public int currentPosition = 0;
    public int backwardSteps = 0;
    public int imprisonTurns = 0;
}

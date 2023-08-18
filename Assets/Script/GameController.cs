using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public interface ISubject
{
    void Attach(IObserver observer);
    void Detach(IObserver observer);
    void Notify();
}

public interface IObserver
{
    void UpdateObserver();
}

public interface ICommand
{
    void Execute();
}

public class RollDiceCommand : ICommand
{
    private Pawn pawn;
    private int step;
    UnityAction action;

    public RollDiceCommand(Pawn pawn,int steps,UnityAction callback)
    {
        this.pawn = pawn;
        this.step = steps;
        action = callback;
    }

    public void Execute()
    {
        pawn.RollDice(step,action);
    }
}

public class UseBackwardsPowerCommand : ICommand
{
    private Pawn pawn;

    public UseBackwardsPowerCommand(Pawn pawn)
    {
        this.pawn = pawn;
    }

    public void Execute()
    {
        pawn.UseBackwardsPower();
    }
}

public class UseImprisonPowerCommand : ICommand
{
    private Pawn pawn;

    public UseImprisonPowerCommand(Pawn pawn)
    {
        this.pawn = pawn;
    }

    public void Execute()
    {
        pawn.UseImprisonPower();
    }
}
public class GameController : MonoBehaviour
{
    public Transform[] cells;
    public Button rollDiceButton;
    public Button backwardPowerButton;
    public Button imprisonPowerButton;
    public float speed;

    public Pawn pawnA;
    public Pawn pawnB;

    private int currentPlayer = 1; 
    private int diceRoll;

    private void Start()
    {
        rollDiceButton.onClick.AddListener(RollDice);
        backwardPowerButton.onClick.AddListener(UseBackwardPower);
        imprisonPowerButton.onClick.AddListener(UseImprisonPower);

        AttachUIObservers();
    }

    private void AttachUIObservers()
    {
        pawnA.Attach(pawnAUIObserver);
        pawnB.Attach(pawnBUIObserver);
    }

    private void RollDice()
    {

        diceRoll = UnityEngine.Random.Range(1, 7);
        rollDiceButton.GetComponentInChildren<Text>().text = diceRoll.ToString();
        ICommand useRollDiceCommand;

        var currentPawn = currentPlayer == 1 ? pawnA : pawnB;
        useRollDiceCommand = new RollDiceCommand(currentPawn,diceRoll,EndTurn);

        useRollDiceCommand.Execute();
    }

    private void UseBackwardPower()
    {
        ICommand useBackwardsPowerCommand;

        var currentPawn = currentPlayer == 1 ? pawnB : pawnA;
        useBackwardsPowerCommand = new UseBackwardsPowerCommand(currentPawn);

        useBackwardsPowerCommand.Execute();
        if(currentPawn.backwardSteps>0)
        EndTurn();
    }

    private void UseImprisonPower()
    {
        ICommand useImprisonPowerCommand;

        if (currentPlayer == 1)
            useImprisonPowerCommand = new UseImprisonPowerCommand(pawnA);
        else
            useImprisonPowerCommand = new UseImprisonPowerCommand(pawnB);

        useImprisonPowerCommand.Execute();
    }


    private void EndTurn()
    {
        if (currentPlayer == 1)
        {
            Color c = pawnA.gameObject.GetComponent<MeshRenderer>().material.color;
            c.a = 0;
            pawnA.gameObject.GetComponent<MeshRenderer>().material.color = c;
            Color c1 = pawnB.gameObject.GetComponent<MeshRenderer>().material.color;
            c1.a = 255;
            pawnB.gameObject.GetComponent<MeshRenderer>().material.color = c1;
        }
        else
        {

            Color c = pawnA.gameObject.GetComponent<MeshRenderer>().material.color;
            c.a = 255;
            pawnA.gameObject.GetComponent<MeshRenderer>().material.color = c;
            Color c1 = pawnB.gameObject.GetComponent<MeshRenderer>().material.color;
            c1.a = 0;
            pawnB.gameObject.GetComponent<MeshRenderer>().material.color = c1;
        }
        currentPlayer *= -1;
    }

    public PawnUIObserver pawnAUIObserver;
    public PawnUIObserver pawnBUIObserver;
}

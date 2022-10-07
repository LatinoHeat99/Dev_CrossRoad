using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    BaseState currentState;

    // Start is called before the first frame update
    public void Start()
    {
        currentState = GetInitState();
        if (currentState != null)
            currentState.Enter();
    }
    void Update()
    {
        if (currentState != null)
            currentState.UpdateLogic();
    }
    private void LateUpdate()
    {
        if (currentState != null)
            currentState.UpdatePhysics();
    }
    public void ChangeState(BaseState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        _newState.Enter();
    }
    protected virtual BaseState GetInitState()
    {
        return null;
    }
}

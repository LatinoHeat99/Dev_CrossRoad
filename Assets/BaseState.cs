using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState
{
    public string name = null;
    protected StateMachine stateMachine = null;

    public BaseState(string _name, StateMachine _sm)
    {
        this.name = _name;
        this.stateMachine = _sm;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void UpdateLogic() { }
    public virtual void UpdatePhysics() { }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
    public State<T> CurrentState { get;private set; }

    T _owner;

    public StateMachine(T owner)
    {
        _owner = owner;
    }

    public void ChangeState(State<T> newState)
    {
        CurrentState?.Exit();//?.是可选链操作符，如果当前对象或者属性为空或者是未定义就不会执行点后面的调用
        CurrentState = newState;
        CurrentState.Enter(_owner);
    }

    public void Execute()
    {
        CurrentState?.Execute();
    }
}

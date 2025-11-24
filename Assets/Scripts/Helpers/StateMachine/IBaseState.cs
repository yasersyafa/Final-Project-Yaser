using UnityEngine;

namespace Helpers.StateMachine
{
    public interface IBaseState<T> where T : Component
    {
        void Enter(T owner);
        void Execute(T owner);
        void Exit(T owner);
    }
}

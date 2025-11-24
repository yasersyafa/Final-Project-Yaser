using UnityEngine;
using Helpers.BehaviorTree;

public class CheckEnemyDistance : Node
{
    private Transform _transform;
    private Transform _target;
    private float _detectionRange;
    private Animator _animator;

    public CheckEnemyDistance(Transform transform, Transform target, float range, Animator animator = null)
    {
        _transform = transform;
        _target = target;
        _detectionRange = range;
        _animator = animator;
    }

    public override NodeState Evaluate()
    {
        if (_target == null) return NodeState.Failure;

        float dist = Vector3.Distance(_transform.position, _target.position);
        _animator?.SetTrigger("idle");
        // Jika jarak dekat, return Success (artinya BAHAYA!)
        return dist < _detectionRange ? NodeState.Success : NodeState.Failure;
    }
}
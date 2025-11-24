using Helpers.BehaviorTree;
using UnityEngine;

public class TaskPatrol : Node
{
    private Transform _transform;
    private Transform[] _waypoints;
    private float _speed;
    private int _wpIndex = 0;
    private float _reachThreshold = 1f;
    private Animator _animator;

    public TaskPatrol(Transform transform, Transform[] waypoints, float speed, Animator animator = null)
    {
        _transform = transform;
        _waypoints = waypoints;
        _speed = speed;
        _animator = animator;
    }

    public override NodeState Evaluate()
    {
        if (_waypoints.Length == 0) return NodeState.Failure;

        _animator?.SetTrigger("run");
        Transform targetWP = _waypoints[_wpIndex];
        
        // 1. Gerak ke Waypoint
        Vector3 dir = (targetWP.position - _transform.position).normalized;
        _transform.position += dir * _speed * Time.deltaTime;

        // 2. Rotasi badan
        if (dir != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(dir);
            _transform.rotation = Quaternion.Slerp(_transform.rotation, lookRot, 5f * Time.deltaTime);
        }

        // 3. Cek jika sampai
        if (Vector3.Distance(_transform.position, targetWP.position) < _reachThreshold)
        {
            _wpIndex = (_wpIndex + 1) % _waypoints.Length; // Loop index
        }

        return NodeState.Running;
    }
}
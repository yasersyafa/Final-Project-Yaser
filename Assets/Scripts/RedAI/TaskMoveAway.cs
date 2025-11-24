using Helpers.BehaviorTree;
using UnityEngine;

public class TaskMoveAway : Node
{
    private Transform _transform;
    private Transform _threat;
    private float _speed;
    private Animator _animator;

    public TaskMoveAway(Transform transform, Transform threat, float speed, Animator animator = null)
    {
        _transform = transform;
        _threat = threat;
        _speed = speed;
        _animator = animator;
    }

    public override NodeState Evaluate()
    {
        // Hitung arah menjauh: (Posisi Kita - Posisi Musuh)
        Vector3 dir = (_transform.position - _threat.position).normalized;
        
        // Gerakan mundur (Native Translate)
        Vector3 moveVector = dir * _speed * Time.deltaTime;
        _transform.position += moveVector;

        // Putar badan menghadap arah lari
        if(dir != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(dir);
            _transform.rotation = Quaternion.Slerp(_transform.rotation, lookRot, 5f * Time.deltaTime);
        }

        _animator?.SetTrigger("run");

        return NodeState.Running;
    }
}
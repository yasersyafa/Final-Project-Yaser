using Helpers.StateMachine;
using UnityEngine;

public class GreenAI : MonoBehaviour
{
    [Header("Settings")]
    public Transform waypoint; 
    public float moveSpeed = 3f;
    public float emoteDuration = 3f; 

    [Header("References")]
    public Animator animator;

    // Internal Data
    public Vector3 StartPosition { get; private set; }
    public bool IsReturning { get; set; } = false; 
    public IBaseState<GreenAI> CurrentState { get; set; }

    void Start()
    {
        StartPosition = transform.position;
        if (animator == null) animator = GetComponent<Animator>();
        
        ChangeState(new WalkState());
    }

    void Update()
    {
        CurrentState?.Execute(this);
    }

    public void ChangeState(IBaseState<GreenAI> newState)
    {
        CurrentState?.Exit(this);
        CurrentState = newState;
        CurrentState?.Enter(this);
    }

    public void MoveTowards(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        Vector3 direction = (target - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10f * Time.deltaTime);
        }
    }
}
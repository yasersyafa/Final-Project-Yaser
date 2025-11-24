using System.Collections.Generic;
using UnityEngine;
using Helpers.BehaviorTree;

public class SmartPedestrian : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 3f;
    public float detectionRange = 5f;
    public Animator anim;
    
    [Header("Freeze Settings")]
    public float freezeTime = 3f; // Diam selama 3 detik
    public float ignoreTime = 5f; // Setelah diam, cuek selama 5 detik

    [Header("References")]
    public Transform carTarget; 
    public Transform waypointParent; 

    private Node _topNode;
    private Transform[] _waypoints;

    void Start()
    {
        List<Transform> wpList = new List<Transform>();
        foreach(Transform t in waypointParent) if(t != waypointParent) wpList.Add(t);
        _waypoints = wpList.ToArray();

        ConstructBehaviorTree();
    }

    void ConstructBehaviorTree()
    {
        TaskFreeze freezeTask = new(freezeTime, ignoreTime, anim);

        // 2. Node: Cek Jarak
        CheckEnemyDistance checkDanger = new(transform, carTarget, detectionRange);

        Sequence reactionSequence = new(new List<Node> { checkDanger, freezeTask });

        TaskPatrol patrolTask = new(transform, _waypoints, moveSpeed, anim);

        _topNode = new Selector(new List<Node> { reactionSequence, patrolTask });
    }

    void Update()
    {
        _topNode?.Evaluate();
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
using Helpers.BehaviorTree;
using UnityEngine;

public class TaskFreeze : Node
{
    private float _freezeDuration;
    private float _cooldownDuration; // Waktu 'kebal' setelah diam agar tidak diam terus menerus
    
    private float _timer;
    private float _cooldownTimer;
    private bool _isFreezing = false;
    private bool _isInCooldown = false;
    private Animator _animator;

    public TaskFreeze(float duration, float cooldown, Animator animator = null)
    {
        _freezeDuration = duration;
        _cooldownDuration = cooldown;
        _animator = animator;
    }

    public override NodeState Evaluate()
    {
        // 1. Cek apakah sedang Cooldown (Masa bodoh dengan mobil)
        if (_isInCooldown)
        {
            _cooldownTimer += Time.deltaTime;
            if (_cooldownTimer >= _cooldownDuration)
            {
                _isInCooldown = false;
                _cooldownTimer = 0;
            }
            // Return Failure agar Tree lanjut ke node Patroli
            return NodeState.Failure;
        }

        // 2. Mulai Freeze (Diam)
        if (!_isFreezing)
        {
            _isFreezing = true;
            _timer = 0;
        }

        // 3. Proses Menunggu
        if (_isFreezing)
        {
            _timer += Time.deltaTime;
            
            if (_timer >= _freezeDuration)
            {
                // Waktu habis, reset dan masuk cooldown
                _isFreezing = false;
                _isInCooldown = true; 
                
                // Return Failure agar frame ini langsung lanjut cek Node Patroli
                return NodeState.Failure; 
            }
            _animator?.SetTrigger("idle");
            // Selama masih menunggu, return Running (Tree berhenti di sini, Patroli tidak dijalankan)
            return NodeState.Running;
        }

        return NodeState.Failure;
    }
}
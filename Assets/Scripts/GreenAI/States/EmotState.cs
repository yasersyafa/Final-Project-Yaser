using Helpers.StateMachine;
using UnityEngine;

public class EmotState : IBaseState<GreenAI>
{
    private float timer;
    
    public void Enter(GreenAI owner)
    {
        timer = 0f;

        // 1. Tentukan Emot (0 atau 1) untuk Blend Tree "blend feel"
        // Jika < 0.5 jadi 0, Jika > 0.5 jadi 1
        float randomValue = Random.value > 0.5f ? 1f : 0f; 
        
        // Set nilai blend tree DULUAN sebelum trigger
        owner.animator.SetFloat("blend feeling", randomValue);

        // 2. Panggil Trigger "feel" untuk memulai animasi
        owner.animator.SetTrigger("feel");

        Debug.Log($"Emot Started. Blend Value: {randomValue}");
    }

    public void Execute(GreenAI owner)
    {
        timer += Time.deltaTime;

        // Tunggu durasi emot selesai, lalu suruh pulang
        if (timer >= owner.emoteDuration)
        {
            owner.IsReturning = true;
            owner.ChangeState(new WalkState());
        }
    }

    public void Exit(GreenAI owner)
    {
        // Karena pakai Trigger, kita tidak perlu men-set false apa-apa.
        // Animasi akan transisi otomatis oleh Animator Controller atau diganti state jalan.
    }
}
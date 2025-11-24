using Helpers.StateMachine;
using UnityEngine;

public class WalkState : IBaseState<GreenAI>
{
    private Vector3 targetPosition;
    private float stopDistance = 0.1f;

    public void Enter(GreenAI owner)
    {
        // Tentukan target: Pulang atau Pergi
        if (owner.IsReturning)
        {
            targetPosition = owner.StartPosition;
        }
        else
        {
            targetPosition = owner.waypoint.position;
        }
        owner.animator.SetTrigger("run");
    }

    public void Execute(GreenAI owner)
    {
        // Gerakkan AI
        owner.MoveTowards(targetPosition);

        // Cek jarak
        if (Vector3.Distance(owner.transform.position, targetPosition) < stopDistance)
        {
            if (!owner.IsReturning)
            {
                // Sampai Waypoint -> Jalankan Emot
                owner.ChangeState(new EmotState());
            }
            else
            {
                // Sampai Rumah -> Selesai
                // (Opsional: Panggil trigger idle jika ada, misal "getup" atau biarkan saja)
                owner.ChangeState(null); 
            }
        }
    }

    public void Exit(GreenAI owner)
    {
        // Tidak ada cleanup khusus karena tidak pakai bool parameter
    }
}
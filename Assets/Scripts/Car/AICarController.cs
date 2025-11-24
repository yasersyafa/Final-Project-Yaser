using UnityEngine;

public class AICarController : MonoBehaviour
{
    [Header("References")]
    public WaypointCircuit circuit;
    public WheelCollider wheelFL, wheelFR, wheelRL, wheelRR;
    public Transform meshFL, meshFR, meshRL, meshRR;

    [Header("Center of Mass (PENTING)")]
    public Transform centerOfMassObj; // Buat object kosong di bawah mobil

    [Header("Car Settings")]
    public float maxMotorTorque = 800f; // Tenaga mesin
    public float maxBrakeTorque = 2000f; // Kekuatan rem
    public float maxSteeringAngle = 35f; // Sudut belok maksimal
    public float maxSpeed = 100f; // Kecepatan maksimal (km/h)
    public float turnSpeedLimit = 30f; // Batas kecepatan saat tikungan tajam

    [Header("AI Sensitivity")]
    public float reachDist = 5f; // Jarak dianggap sampai waypoint
    public float steerSensitivity = 1.0f; 

    private int currentWP = 0;
    private Rigidbody rb;
    private float currentSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // STABILITAS: Menggeser titik berat ke bawah agar tidak gampang terbalik
        if (centerOfMassObj != null)
            rb.centerOfMass = centerOfMassObj.localPosition;
    }

    void FixedUpdate()
    {
        if (circuit == null || circuit.waypoints.Count == 0) return;

        Drive();
        CheckWaypoint();
        AnimateWheels();
    }

    void Drive()
    {
        // 1. Hitung Kecepatan (KM/H)
        currentSpeed = rb.linearVelocity.magnitude * 3.6f;

        // 2. Tentukan Target Posisi (Local Space)
        // Kita ubah posisi dunia waypoint menjadi posisi relatif terhadap moncong mobil
        // Jika x positif = target di kanan. Jika x negatif = target di kiri.
        Vector3 targetPos = circuit.waypoints[currentWP].position;
        Vector3 relativeVector = transform.InverseTransformPoint(targetPos);

        // 3. Rumus Steering (Best Practice)
        // Membagi x dengan magnitude memberikan rasio seberapa tajam harus berbelok (-1 s/d 1)
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteeringAngle * steerSensitivity;
        wheelFL.steerAngle = newSteer;
        wheelFR.steerAngle = newSteer;

        // 4. Smart Throttle & Braking
        // Cek apakah kita sedang belok tajam?
        bool isCornering = Mathf.Abs(newSteer) > 10f;
        
        float targetSpeed = isCornering ? turnSpeedLimit : maxSpeed;
        float accel = 0f;
        float brake = 0f;

        if (currentSpeed < targetSpeed)
        {
            accel = maxMotorTorque;
            brake = 0f;
        }
        else
        {
            accel = 0f;
            brake = maxBrakeTorque * 0.5f; // Rem pelan agar speed turun
        }

        // Apply ke Roda (FWD - Front Wheel Drive, ubah jika ingin RWD/AWD)
        wheelFL.motorTorque = accel;
        wheelFR.motorTorque = accel;

        wheelFL.brakeTorque = brake;
        wheelFR.brakeTorque = brake;
        wheelRL.brakeTorque = brake;
        wheelRR.brakeTorque = brake;
    }

    void CheckWaypoint()
    {
        // Cek jarak 2D (abaikan tinggi/Y) agar waypoint di tanjakan tetap terdeteksi
        Vector3 myPos = transform.position;
        Vector3 targetPos = circuit.waypoints[currentWP].position;
        myPos.y = 0; 
        targetPos.y = 0;

        if (Vector3.Distance(myPos, targetPos) < reachDist)
        {
            currentWP++;
            if (currentWP >= circuit.waypoints.Count) currentWP = 0;
        }
    }

    void AnimateWheels()
    {
        UpdateWheel(wheelFL, meshFL);
        UpdateWheel(wheelFR, meshFR);
        UpdateWheel(wheelRL, meshRL);
        UpdateWheel(wheelRR, meshRR);
    }

    void UpdateWheel(WheelCollider col, Transform mesh)
    {
        Vector3 pos;
        Quaternion rot;
        col.GetWorldPose(out pos, out rot);
        mesh.position = pos;
        mesh.rotation = rot;
    }
}
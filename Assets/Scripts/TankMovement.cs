using UnityEngine;


public class TankMovement : MonoBehaviour
{
    
    [SerializeField] private float m_Speed = 12f;                 // How fast the tank moves forward and back.
    [SerializeField] private float m_TurnSpeed = 140f;            // How fast the tank turns in degrees per second.
    [SerializeField] private AudioSource m_MovementAudio;         // Reference to the audio source used to play engine sounds. NB: different to the shooting audio source.
    [SerializeField] private AudioClip m_EngineIdling;            // Audio to play when the tank isn't moving.
    [SerializeField] private AudioClip m_EngineDriving;           // Audio to play when the tank is moving.
    [SerializeField] private float m_PitchRange = 0.2f;           // The amount by which the pitch of the engine noises can vary.

    private Rigidbody m_Rigidbody;              // Reference used to move the tank.
    private float m_MovementInputValue;         // The current value of the movement input.
    private float m_TurnInputValue;             // The current value of the turn input.
    private float m_OriginalPitch;              // The pitch of the audio source at the start of the scene.


    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }


    private void OnEnable()
    {
        // Разрешаем танку двигаться
        m_Rigidbody.isKinematic = false;

        // Сбрасываем значения
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }


    private void OnDisable()
    {
        // Запрещаем танку двигаться
        m_Rigidbody.isKinematic = true;
    }


    private void Start()
    {
        // Получаем оригинальный тон клипа
        m_OriginalPitch = m_MovementAudio.pitch;
    }


    private void Update()
    {
        EngineAudio();
    }

    public void SetAxis(float movementAxis, float turnAxis)
    {
        m_MovementInputValue = movementAxis;
        m_TurnInputValue = turnAxis;
    }

    private void EngineAudio()
    {
        // Если танк стоит на месте
        if (Mathf.Abs(m_MovementInputValue) < 0.1f && Mathf.Abs(m_TurnInputValue) < 0.1f)
        {
            // И если всё-ещё звучит клип движения
            if (m_MovementAudio.clip == m_EngineDriving)
            {
                // переключаем на клип холостого хода
                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
        else
        {
            // Если танк движется и всё ещё играет клип холостого хода...
            if (m_MovementAudio.clip == m_EngineIdling)
            {
                // переключаем на клип движения 
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
    }


    public void Move()
    {
        Vector3 vel = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;
        vel.y = m_Rigidbody.velocity.y;
        m_Rigidbody.velocity = vel;
    }


    public void Turn()
    {
        m_Rigidbody.angularVelocity = new Vector3(0f, m_TurnInputValue * m_TurnSpeed * Time.deltaTime, 0f);
    }
}

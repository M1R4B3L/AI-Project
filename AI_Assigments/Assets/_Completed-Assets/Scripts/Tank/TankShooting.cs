using System;
using UnityEngine;
using UnityEngine.UI;

namespace Complete
{
    public class TankShooting : MonoBehaviour
    {
        public int m_PlayerNumber = 1;              // Used to identify the different players.
        public Rigidbody m_Shell;                   // Prefab of the shell.
        public Transform m_FireTransform;           // A child of the tank where the shells are spawned.
        public AudioSource m_ShootingAudio;         // Reference to the audio source used to play the shooting audio. NB: different to the movement audio source.
        public AudioClip m_FireClip;                // Audio that plays when each shot is fired.

        // Student added
        public GameObject target;
        public Transform m_tank_transform;
        public float m_LaunchForce = 20.0f;              // The force given to the shell. Constant
        public float m_Max_Angle = 60.0f;                // Max angle of the trajectory.
        public float m_Min_Angle = 0.0f;                 // Min angleof the trajectory.
        public float m_Shooting_Cooldown = 3.0f;         // Minimum time between each shot.
        public float m_Shooting_Cooldown_Current = 0.0f; // Starting cooldown.
        public float m_Shooting_Angle = 0.0f;            // Starting shooting angle.
        public float m_Shooting_Radius = 0.0f;           // Radius range of the shells.


        public bool m_Shooting_Angle_Found = false;   // Angle needed to hit target.
        public bool m_AI_Controlled = true;           // For testing porpouses we can switch between AI and player controll.
        //

        private string m_fireButton;                // The input axis that is used for launching shells.
        private bool m_fired;                       // Whether or not the shell has been launched with this button press.

        private void OnEnable()
        {
            m_Shooting_Radius = Get_Max_Shooting_Range();
        }


        private void Start ()
        {
            // The fire axis is based on the player number.
            m_fireButton = "Fire" + m_PlayerNumber;

        }

        // Student added
        private void Update()
        {

            if (m_fired)
            {
                m_Shooting_Cooldown_Current += Time.deltaTime;

                if (m_Shooting_Cooldown_Current >= m_Shooting_Cooldown)          
                {
                    m_fired = false;
                    m_Shooting_Cooldown_Current = 0.0f;
                    m_Shooting_Cooldown = 3.0f;

                }
            }

            if (m_AI_Controlled)
            {
                if (!m_fired)
                {
                    FindSuitableAngle();                                                                          

                    if (m_Shooting_Angle_Found)                                                                   
                    {
                        AI_Fire();
                    }
                }
            }
            else
            {
                if (Input.GetButtonUp(m_fireButton) && !m_fired)
                {
                    Fire();
                }
            }
        }

        // Get maximum radius range
        private float Get_Max_Shooting_Range()
        {
            float g = Physics.gravity.y;                                      
            float p = m_LaunchForce;                                          
            float m = 45.0f;                                                  

            float Max_Shooting_Range = ((p * p) * Mathf.Sin(2 * m)) / g;      

            Max_Shooting_Range = Mathf.Abs(Max_Shooting_Range);               

            Debug.Log(Max_Shooting_Range);

            return Max_Shooting_Range;
        }

        // AI controlled fire
        private void AI_Fire()
        {
            m_Shooting_Angle_Found = false;
            m_fired = true;

            m_FireTransform.Rotate(-m_Shooting_Angle, 0.0f, 0.0f);

            Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
            shellInstance.velocity = m_LaunchForce * m_FireTransform.forward;

            m_ShootingAudio.clip = m_FireClip;
            m_ShootingAudio.Play();
        }

        // Angle Management
        private void FindSuitableAngle()
        {
            float Distance_To_Tank = Vector3.Distance(m_FireTransform.position, m_tank_transform.position);

            if (Distance_To_Tank < m_Shooting_Radius)                                      
            {
                // tan(a) = (v^2 +- sqrt(v^4 - g(gx^2 + 2yv^2))) / gx;
                float g = Physics.gravity.y;                                               
                float p = m_LaunchForce;                                                   
                float x = Distance_To_Tank;                                                
                float y = m_tank_transform.position.y;                                  

                if (y < 0.0f)                                                           
                {
                    y = 0.0f;
                }

                // Dividing operations for easier equation.
                float x2 = x * x;
                float p2 = p * p;                                                                                  
                float p4 = p * p * p * p;                                                                           

                float tan = (p2 - Mathf.Sqrt(p4 - g * (g * x2 + 2 * y * p2))) / (g * x);   
                float rad_angle = Mathf.Atan(tan);

                m_Shooting_Angle = Test_Suitable_Angle(rad_angle);                              

                if (m_Shooting_Angle > 0.0f)
                {
                    m_Shooting_Angle_Found = true;
                }
            }
            
        }

        private float Test_Suitable_Angle(double angle)                           
        {
            float ret = 0.0f;

            float posible_angle = Math.Abs((float)angle * Mathf.Rad2Deg);

            if ((posible_angle > m_Min_Angle && posible_angle < m_Max_Angle))     
            {
                ret = posible_angle;
            }
            else
            {
                Debug.LogError("[ERROR]");
            }

            return ret;
        }
        // Student adeed end.
       

        // Kept for testing porpouses.
        private void Fire ()
        {
            // Set the fired flag so only Fire is only called once.
            m_fired = true;

            // Create an instance of the shell and store a reference to it's rigidbody.
            Rigidbody shellInstance =
                Instantiate (m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

            // Set the shell's velocity to the launch force in the fire position's forward direction.

            // Change the clip to the firing clip and play it.
            m_ShootingAudio.clip = m_FireClip;
            m_ShootingAudio.Play ();

            // Reset the launch force.  This is a precaution in case of missing button events.
        }
    }
}
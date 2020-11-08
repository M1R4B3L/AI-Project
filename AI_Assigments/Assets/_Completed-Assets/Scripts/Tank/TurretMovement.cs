using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretMovement : MonoBehaviour
{
    public GameObject m_Tank;
    public GameObject m_Turret;

    // Update is called once per frame
    void Update()
    {
        if (m_Tank != null)
        {
            if (m_Tank.tag == "Red")
            {
                m_Turret.transform.LookAt(GameObject.FindGameObjectWithTag("Blue").transform);
            }
            else if (m_Tank.tag == "Blue")
            {
                m_Turret.transform.LookAt(GameObject.FindGameObjectWithTag("Red").transform);
            }
        }
    }
}

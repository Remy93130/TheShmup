using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class ShieldCollision : MonoBehaviour
{

    [SerializeField] string[] _collisionTag;
    [SerializeField] int m_shieldLife;
    Material mat;

    public int ShieldLife
    {
        get => m_shieldLife;
    }

    void Start()
    {
        if (GetComponent<Renderer>())
        {
            mat = GetComponent<Renderer>().sharedMaterial;
        }
    }

    public void ManageCollision(Collision collision)
    {
        for (int i = 0; i < _collisionTag.Length; i++)
        {
            if (collision.transform.CompareTag(_collisionTag[i]))
            {
                if (--m_shieldLife <= 0)
                {
                    Destroy(gameObject);
                }
                Debug.Log(Time.frameCount + "-Hit by " + collision.transform.tag + " life left:" + m_shieldLife);
            }
        }
    }
}


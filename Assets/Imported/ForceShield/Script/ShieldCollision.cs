using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class ShieldCollision : MonoBehaviour
{

    [SerializeField] string[] _collisionTag;
    [SerializeField] int m_shieldLife;
    [SerializeField] GameObject m_explosionPrefab = null;
    private GameObject _animation;
    Material mat;

    public int ShieldLife
    {
        get => m_shieldLife;
    }

    private void FixedUpdate()
    {
        /*if (_animation)
            _animation.transform.position = transform.parent.position;*/
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
                    if (m_explosionPrefab)
                    {
                        _animation = Instantiate(m_explosionPrefab, gameObject.GetComponent<Collider>().transform.position, Quaternion.identity);
                    }
                    Destroy(gameObject);
                }
                Debug.Log(Time.frameCount + "-Hit by " + collision.transform.tag + " life left:" + m_shieldLife);
            }
        }
    }
}


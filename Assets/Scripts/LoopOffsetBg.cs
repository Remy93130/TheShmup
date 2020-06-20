using SDD.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LoopOffsetBg : SimpleGameStateObserver
{
    [Header("Backgrounds")]
    [SerializeField] private Material[] m_materials;
    private int _index = 0;

    [Header("Transition")]
    [SerializeField] private GameObject m_explosionPrefab;
    [SerializeField] private Transform[] m_explosionsSpawnPoint;
    

    MeshRenderer m_meshRenderer;

    #region EventManager section
    public override void SubscribeEvents()
    {
        base.SubscribeEvents();
        EventManager.Instance.AddListener<ChangeBackgroundEvent>(ChangeBackgroundEvent);
    }

    public override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();
        EventManager.Instance.RemoveListener<ChangeBackgroundEvent>(ChangeBackgroundEvent);
    }
    #endregion

    void Start()
    {
        m_meshRenderer = GetComponent<MeshRenderer>();
        m_meshRenderer.material = m_materials[0];
    }

    void Update()
    {
        Material mat = m_meshRenderer.material;
        Vector2 offset = mat.mainTextureOffset;
        offset.y = GameObject.FindGameObjectWithTag("Player").transform.position.y / 100;
        offset.x += Time.deltaTime / 10f;
        mat.mainTextureOffset = offset;
    }

    protected override void ChangeBackgroundEvent(ChangeBackgroundEvent e)
    {
        if (e.DefaultBackground)
        {
            m_meshRenderer.material = m_materials[0];
            _index = 0;
        } else
        {
            ShuffleArray();
            StartCoroutine(TriggerExplosion());
            _index = (++_index % 2 == 0) ? 0 : 1;
            StartCoroutine(BackgroundTransition());
        }
    }

    private void ShuffleArray()
    {
        System.Random rand = new System.Random();
        for (int i = 0; i < m_explosionsSpawnPoint.Length - 1; i++)
        {
            int j = rand.Next(i, m_explosionsSpawnPoint.Length);
            Transform tmp = m_explosionsSpawnPoint[i];
            m_explosionsSpawnPoint[i] = m_explosionsSpawnPoint[j];
            m_explosionsSpawnPoint[j] = tmp;
        }
    }

    IEnumerator TriggerExplosion()
    {
        for (int i = 0; i < m_explosionsSpawnPoint.Length; i++)
        {
            yield return new WaitForSeconds(.125f);
            Vector3 spawnPosition = new Vector3(
                m_explosionsSpawnPoint[i].position.x,
                m_explosionsSpawnPoint[i].position.y,
                UnityEngine.Random.Range(-0.3f, 0.25f)
            );
            Instantiate(m_explosionPrefab, spawnPosition, Quaternion.identity);
        }
    }

    IEnumerator BackgroundTransition()
    {
        yield return new WaitForSeconds(.75f);
        m_meshRenderer.material = m_materials[_index];
    }

    protected override void GameReset(GameResetEvent e) => StopAllCoroutines();
}

using SDD.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopOffsetBg : SimpleGameStateObserver
{
    [SerializeField]
    private Material[] m_materials;
    private int _index = 0;

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
        m_meshRenderer.material = m_materials[_index];
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
        m_meshRenderer.material = m_materials[++_index % m_materials.Length];
    }
}

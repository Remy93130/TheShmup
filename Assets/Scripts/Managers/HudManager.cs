using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HudManager : Manager<HudManager>
{
    #region Labels & Values
    [Header("HudManager")]
    [Header("Texts")]
    [SerializeField]
    private Text m_TxtBestScore;
    [SerializeField] private Text m_TxtScore;
    [SerializeField] private Text m_TxtNLives;
    [SerializeField] private Text m_TxtNEnemiesLeftBeforeVictory;

    [Header("Health bar")]
    [SerializeField] private Slider m_Slider;
    [SerializeField] private Slider m_SliderBoss;
    [SerializeField] public GameObject m_Border;

    [Header("Texts Prefabs")]
    [SerializeField] public GameObject m_PrefScore;
    [SerializeField] public GameObject m_PrefGameOverScore;
    [SerializeField] public GameObject m_PrefNewBestScore;

    [Header("GameOverTexts")]
    [SerializeField] private Text m_TxtGameOverScore;
    [SerializeField] private Text m_TxtGameOverBestScore;





    private float _fadeMode;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        _fadeMode = 0.0f;
        m_Border.GetComponent<Image>().CrossFadeAlpha(_fadeMode, 0, true);
        SetImageState(m_Border.transform, true);
    }

    #region Manager implementation
    public void SetBorderBoss(bool state, bool init = false)
    {
        _fadeMode = (state) ? 1.0f : 0.0f;
        m_Border.GetComponent<Image>().CrossFadeAlpha(_fadeMode, 3.25f, false);
        SetImageState(m_Border.transform, init);
    }

    private void SetImageState(Transform transform, bool init)
    {
        float duration = (init) ? 0.0f : 3.5f;
        Image image = transform.gameObject.GetComponent<Image>();
        if (image) image.CrossFadeAlpha(_fadeMode, duration, init);
        for (int i = 0; i < transform.childCount; i++)
            SetImageState(transform.GetChild(i), init);

    }
    #endregion

    #region Callbacks to GameManager events
    protected override void GameStatisticsChanged(GameStatisticsChangedEvent e)
    {
        m_TxtBestScore.text = e.eBestScore.ToString();
        m_TxtGameOverBestScore.text = e.eBestScore.ToString();
        m_TxtScore.text = e.eScore.ToString();
        m_TxtGameOverScore.text = e.eScore.ToString();
        m_TxtNLives.text = e.eNLives.ToString();
        m_Slider.value = e.eNLives;
    }

    protected override void GameBossShoted(GameBossShotedEvent e) => m_SliderBoss.value = e.eNLives;

    protected override void NewBoss(NewBossEvent e) => m_SliderBoss.maxValue = e.eNLives;


    protected override IEnumerator InitCoroutine()
    {
        yield break;
    }

    #endregion
}

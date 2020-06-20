using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HudManager : Manager<HudManager>
{
    [Header("HudManager")]
    #region Labels & Values
    [Header("Texts")]
    [SerializeField]
    private Text m_TxtBestScore;
    [SerializeField] private Text m_TxtScore;
    [SerializeField] private Text m_TxtNLives;
    [SerializeField] private Text m_TxtNEnemiesLeftBeforeVictory;
    [SerializeField] private Slider slider;

    [SerializeField] private Slider sliderBoss;

    [SerializeField] public GameObject m_Border;

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
        m_TxtScore.text = e.eScore.ToString();
        m_TxtNLives.text = e.eNLives.ToString();
        slider.value = e.eNLives;
        // m_TxtNEnemiesLeftBeforeVictory.text = e.eNEnemiesLeftBeforeVictory.ToString();
    }

    protected override void GameBossShoted(GameBossShotedEvent e)
    {
        sliderBoss.value = e.eNLives;
    }
    protected override void NewBoss(NewBossEvent e)
    {
        Debug.Log("Max value : " + e.eNLives);
        sliderBoss.maxValue = e.eNLives;
    }


    protected override IEnumerator InitCoroutine()
    {
        yield break;
    }

    #endregion
}

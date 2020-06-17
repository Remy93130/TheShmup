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
	#endregion

	#region Manager implementation
	public void SetBorderBoss(bool state)
    {

        m_Border.SetActive(state);
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

   
    protected override IEnumerator InitCoroutine()
    {
        yield break;
    }
 
    #endregion
}

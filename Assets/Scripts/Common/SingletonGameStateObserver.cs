using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using System;

public abstract class SingletonGameStateObserver<T> :  Singleton<T>,IEventHandler where T:Component
{
	public virtual void SubscribeEvents()
	{
		EventManager.Instance.AddListener<GameMenuEvent>(GameMenu);
		EventManager.Instance.AddListener<GameBeginnerLevelEvent>(GameBeginnerLevelPlay);
		EventManager.Instance.AddListener<GameIntermediateLevelEvent>(GameIntermediateLevelPlay);
		EventManager.Instance.AddListener<GameDifficultLevelEvent>(GameDifficultLevelPlay);
		EventManager.Instance.AddListener<GamePauseEvent>(GamePause);
		EventManager.Instance.AddListener<GameResumeEvent>(GameResume);
		EventManager.Instance.AddListener<GameOverEvent>(GameOver);
		EventManager.Instance.AddListener<GameVictoryEvent>(GameVictory);
		EventManager.Instance.AddListener<GameStatisticsChangedEvent>(GameStatisticsChanged);
        EventManager.Instance.AddListener<GameSettingsEvent>(GameSettings);
        EventManager.Instance.AddListener<GameControlsEvent>(GameControls);
		EventManager.Instance.AddListener<GameChooseLevelEvent>(GameChooseLevel);
	}

	public virtual void UnsubscribeEvents()
	{
		EventManager.Instance.RemoveListener<GameMenuEvent>(GameMenu);
		EventManager.Instance.RemoveListener<GameBeginnerLevelEvent>(GameBeginnerLevelPlay);
		EventManager.Instance.RemoveListener<GameIntermediateLevelEvent>(GameIntermediateLevelPlay);
		EventManager.Instance.RemoveListener<GameDifficultLevelEvent>(GameDifficultLevelPlay);
		EventManager.Instance.RemoveListener<GamePauseEvent>(GamePause);
		EventManager.Instance.RemoveListener<GameResumeEvent>(GameResume);
		EventManager.Instance.RemoveListener<GameOverEvent>(GameOver);
		EventManager.Instance.RemoveListener<GameVictoryEvent>(GameVictory);
		EventManager.Instance.RemoveListener<GameStatisticsChangedEvent>(GameStatisticsChanged);
        EventManager.Instance.RemoveListener<GameSettingsEvent>(GameSettings);
        EventManager.Instance.RemoveListener<GameControlsEvent>(GameControls);
		EventManager.Instance.RemoveListener<GameChooseLevelEvent>(GameChooseLevel);
	}

	protected override void Awake()
	{
		base.Awake();
		SubscribeEvents();
	}

	protected virtual void OnDestroy()
	{
		UnsubscribeEvents();
	}

	protected virtual void GameMenu(GameMenuEvent e)
	{
	}

	protected virtual void GameBeginnerLevelPlay(GameBeginnerLevelEvent e)
	{
	}

	protected virtual void GameIntermediateLevelPlay(GameIntermediateLevelEvent e)
	{
	}

	protected virtual void GameDifficultLevelPlay(GameDifficultLevelEvent e)
	{
	}

	protected virtual void GamePause(GamePauseEvent e)
	{
	}

	protected virtual void GameResume(GameResumeEvent e)
	{
	}

	protected virtual void GameOver(GameOverEvent e)
	{
	}

	protected virtual void GameVictory(GameVictoryEvent e)
	{
	}

    protected virtual void GameSettings(GameSettingsEvent e)
    {
    }

    protected virtual void GameControls(GameControlsEvent e)
    {
    }

	protected virtual void GameChooseLevel(GameChooseLevelEvent e)
	{
	}

	protected virtual void GameStatisticsChanged(GameStatisticsChangedEvent e)
	{
	}

	protected virtual void ChangeBackgroundEvent(ChangeBackgroundEvent e)
	{
	}
}

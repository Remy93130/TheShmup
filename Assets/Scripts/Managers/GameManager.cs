using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public enum GameState { gameMenu,gamePlay,gamePause,gameOver,gameVictory}

public class GameManager : Manager<GameManager> {

	//Game State
	private GameState m_GameState;
	public bool IsPlaying { get { return m_GameState == GameState.gamePlay; } }

	// TIME SCALE
	private float m_TimeScale;
	public float TimeScale { get { return m_TimeScale; } }
	void SetTimeScale(float newTimeScale)
	{
		m_TimeScale = newTimeScale;
		Time.timeScale = m_TimeScale;
	}

	//SCORE
	private int m_Score;
	public int Score {
		get { return m_Score; }
		set
		{
			m_Score = value;
			BestScore = Mathf.Max(BestScore, value);
		}
	}

	public int BestScore
	{
		get
		{
			return PlayerPrefs.GetInt("BEST_SCORE", 0);
		}
		set
		{
			PlayerPrefs.SetInt("BEST_SCORE", value);
		}
	}

	void IncrementScore(int increment)
	{
		SetScore(m_Score + increment);
	}

	void SetScore(int score)
	{
		Score = score;
		//EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eBestScore = BestScore, eScore = m_Score, eNLives = m_NLives, eNEnemiesLeftBeforeVictory = m_NEnemiesLeftBeforeVictory });
		EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eBestScore = BestScore, eScore = m_Score, eNLives = m_NLives});
	}

	//LIVES
	[Header("GameManager")]
	[SerializeField] private int m_NStartLives;

	private int m_NLives;
	public int NLives { get { return m_NLives; } }
	void DecrementNLives(int decrement)
	{
		SetNLives(m_NLives-decrement);
	}

	void SetNLives(int nLives)
	{
		m_NLives =nLives;
		//EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eBestScore = BestScore, eScore = m_Score, eNLives = m_NLives, eNEnemiesLeftBeforeVictory = m_NEnemiesLeftBeforeVictory });
		EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eBestScore = BestScore, eScore = m_Score, eNLives = m_NLives});
	}

	// Victory Condition
	/*[SerializeField] private int m_NEnemiesToDestroyForVictory;
	private int m_NEnemiesLeftBeforeVictory;
	void DecrementNEnemiesLeftBeforeVictory(int decrement)
	{
		SetNEnemiesLeftBeforeVictory(m_NEnemiesLeftBeforeVictory - decrement);
	}
	void SetNEnemiesLeftBeforeVictory(int nEnemies)
	{
		m_NEnemiesLeftBeforeVictory = Mathf.Max(nEnemies,0);
		EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eBestScore = BestScore, eScore = m_Score, eNLives = m_NLives, eNEnemiesLeftBeforeVictory = m_NEnemiesLeftBeforeVictory });
	}*/

	//Players
	[SerializeField]
	List<PlayerController> m_Players = new List<PlayerController>();

	#region Events' subscription
	public override void SubscribeEvents()
	{
		base.SubscribeEvents();

		//PlayerController
		EventManager.Instance.AddListener<PlayerHasBeenHitEvent>(PlayerHasBeenHit);

		//MainMenuManager
		EventManager.Instance.AddListener<MainMenuButtonClickedEvent>(MainMenuButtonClicked);
        EventManager.Instance.AddListener<MainMenuButtonFromSettingsClickedEvent>(MainMenuButtonFromSettingsClicked);
        EventManager.Instance.AddListener<PlayButtonClickedEvent>(PlayButtonClicked);
		EventManager.Instance.AddListener<NextLevelButtonClickedEvent>(NextLevelButtonClicked);
		EventManager.Instance.AddListener<ResumeButtonClickedEvent>(ResumeButtonClicked);
		EventManager.Instance.AddListener<EscapeButtonClickedEvent>(EscapeButtonClicked);
        EventManager.Instance.AddListener<SettingsButtonClickedEvent>(SettingsButtonClicked);
        EventManager.Instance.AddListener<ControlsButtonClickedEvent>(ControlsButtonClicked);
		EventManager.Instance.AddListener<BeginnerButtonClickedEvent>(BeginnerButtonClicked);
		EventManager.Instance.AddListener<IntermediateButtonClickedEvent>(IntermediateButtonClicked);
		EventManager.Instance.AddListener<DifficultButtonClickedEvent>(DifficultButtonClicked);


		//Enemy
		//EventManager.Instance.AddListener<EnemyHasBeenDestroyedEvent>(EnemyHasBeenDestroyed);

		//Enemy Manager
		EventManager.Instance.AddListener<LevelHasEnded>(Victory);

		//Score Item
		EventManager.Instance.AddListener<ScoreItemEvent>(ScoreHasBeenGained);

		//Pattern
		EventManager.Instance.AddListener<PatternHasBeenInstantiatedEvent>(PatternHasBeenInstantiated);
		EventManager.Instance.AddListener<AllEnemiesOfPatternHaveBeenDestroyedEvent>(AllEnemiesOfPatternHaveBeenDestroyed);
	}

	public override void UnsubscribeEvents()
	{
		base.UnsubscribeEvents();

		//PlayerController
		EventManager.Instance.RemoveListener<PlayerHasBeenHitEvent>(PlayerHasBeenHit);

		//MainMenuManager
		EventManager.Instance.RemoveListener<MainMenuButtonClickedEvent>(MainMenuButtonClicked);
        EventManager.Instance.RemoveListener<MainMenuButtonFromSettingsClickedEvent>(MainMenuButtonFromSettingsClicked);
		EventManager.Instance.RemoveListener<PlayButtonClickedEvent>(PlayButtonClicked);
		EventManager.Instance.RemoveListener<NextLevelButtonClickedEvent>(NextLevelButtonClicked);
		EventManager.Instance.RemoveListener<ResumeButtonClickedEvent>(ResumeButtonClicked);
		EventManager.Instance.RemoveListener<EscapeButtonClickedEvent>(EscapeButtonClicked);
        EventManager.Instance.RemoveListener<SettingsButtonClickedEvent>(SettingsButtonClicked);
        EventManager.Instance.RemoveListener<ControlsButtonClickedEvent>(ControlsButtonClicked);
		EventManager.Instance.RemoveListener<BeginnerButtonClickedEvent>(BeginnerButtonClicked);
		EventManager.Instance.RemoveListener<IntermediateButtonClickedEvent>(IntermediateButtonClicked);
		EventManager.Instance.RemoveListener<DifficultButtonClickedEvent>(DifficultButtonClicked);

		//Enemy
		//EventManager.Instance.RemoveListener<EnemyHasBeenDestroyedEvent>(EnemyHasBeenDestroyed);

		//Enemy Manager
		EventManager.Instance.RemoveListener<LevelHasEnded>(Victory);

		//Score Item
		EventManager.Instance.RemoveListener<ScoreItemEvent>(ScoreHasBeenGained);

		//Pattern
		EventManager.Instance.RemoveListener<PatternHasBeenInstantiatedEvent>(PatternHasBeenInstantiated);
		EventManager.Instance.RemoveListener<AllEnemiesOfPatternHaveBeenDestroyedEvent>(AllEnemiesOfPatternHaveBeenDestroyed);
	}
	#endregion

	#region Manager implementation
	protected override IEnumerator InitCoroutine()
	{
		Menu();
		//EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eBestScore = BestScore, eScore = 0, eNLives = 0, eNEnemiesLeftBeforeVictory = 0 });
		EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eBestScore = BestScore, eScore = 0, eNLives = 0});
		yield break;
	}
	#endregion

	private void InitNewGame()
	{
		SetScore(0);
		SetNLives(m_NStartLives);
		//SetNEnemiesLeftBeforeVictory(m_NEnemiesToDestroyForVictory);
	}

	#region Callbacks to events issued by Enemy
	/*private void EnemyHasBeenDestroyed(EnemyHasBeenDestroyedEvent e)
	{
		if (e.eDestroyedByPlayer)
		{
			DecrementNEnemiesLeftBeforeVictory(1);

			if (m_NEnemiesLeftBeforeVictory == 0)
			{
				Victory();
			}
		}
	}*/
	#endregion

	#region Callbacks to events issued by Score items
	private void PlayerHasBeenHit(PlayerHasBeenHitEvent e)
	{
		DecrementNLives(1);

		if (m_NLives == 0)
		{

			Over();
		}
	}
	#endregion

	#region Callbacks to events issued by Score items
	private void ScoreHasBeenGained(ScoreItemEvent e)
	{
		IncrementScore(e.eScore.Score);
	}
	#endregion

	#region Callbacks to events issued by Pattern
	private void PatternHasBeenInstantiated(PatternHasBeenInstantiatedEvent e)
	{ }
	private void AllEnemiesOfPatternHaveBeenDestroyed(AllEnemiesOfPatternHaveBeenDestroyedEvent e)
	{ }
	#endregion



	#region Callbacks to Events issued by MenuManager
	private void MainMenuButtonClicked(MainMenuButtonClickedEvent e)
	{
		Menu();
	}

    private void MainMenuButtonFromSettingsClicked(MainMenuButtonFromSettingsClickedEvent e)
    {
        MenuFromSettings();
    }

	private void PlayButtonClicked(PlayButtonClickedEvent e)
	{
		ChooseLevel();
	}

	private void BeginnerButtonClicked(BeginnerButtonClickedEvent e)
	{
		BeginnerLevel();
	}

	private void IntermediateButtonClicked(IntermediateButtonClickedEvent e)
	{
		IntermediateLevel();
	}

	private void DifficultButtonClicked(DifficultButtonClickedEvent e)
	{
		DifficultLevel();
	}

	private void NextLevelButtonClicked(NextLevelButtonClickedEvent e)
	{
		EventManager.Instance.Raise(new GoToNextLevelEvent());
	}

	private void ResumeButtonClicked(ResumeButtonClickedEvent e)
	{
		Resume();
	}

	private void EscapeButtonClicked(EscapeButtonClickedEvent e)
	{
		if (IsPlaying)
			Pause();
	}
    private void SettingsButtonClicked(SettingsButtonClickedEvent e)
    {
        Settings();
    }
    private void ControlsButtonClicked(ControlsButtonClickedEvent e)
    {
        Controls();
    }
	#endregion



	//EVENTS
	private void Menu()
	{
		SetTimeScale(0);
		m_GameState = GameState.gameMenu;
		MusicLoopsManager.Instance.PlayMusic(Constants.MENU_MUSIC);
		EventManager.Instance.Raise(new GameMenuEvent());
	}

    public void MenuFromSettings()
    {
        SetTimeScale(0);
        m_GameState = GameState.gameMenu;
        EventManager.Instance.Raise(new GameMenuEvent());
    }

	private void ChooseLevel()
	{
		SetTimeScale(0);
		m_GameState = GameState.gameMenu;
		EventManager.Instance.Raise(new GameChooseLevelEvent());
	}
	private void Play()
	{
		InitNewGame();
		SetTimeScale(1);
		m_GameState = GameState.gamePlay;
		
	}

	private void BeginnerLevel()
	{
		Play();
		EventManager.Instance.Raise(new GameBeginnerLevelEvent());
        MusicLoopsManager.Instance.PlayMusic(Constants.GAMEPLAY_MUSIC);
    }

	private void IntermediateLevel()
	{
		Play();
		EventManager.Instance.Raise(new GameIntermediateLevelEvent());
        MusicLoopsManager.Instance.PlayMusic(Constants.GAMEPLAY_MUSIC);
    }

	private void DifficultLevel()
	{
		Play();
		EventManager.Instance.Raise(new GameDifficultLevelEvent());
        MusicLoopsManager.Instance.PlayMusic(Constants.GAMEPLAY_MUSIC);
    }

	private void Pause()
	{
		SetTimeScale(0);
		m_GameState = GameState.gamePause;
		EventManager.Instance.Raise(new GamePauseEvent());
	}

	private void Resume()
	{
		SetTimeScale(1);
		m_GameState = GameState.gamePlay;
		EventManager.Instance.Raise(new GameResumeEvent());
	}

	private void Over()
	{
		SetTimeScale(0);
		m_GameState = GameState.gameOver;
		SfxManager.Instance.PlaySfx(Constants.GAMEOVER_SFX);
		EventManager.Instance.Raise(new GameOverEvent());
	}

	private void Victory(LevelHasEnded e)
	{
		SetTimeScale(0);
		m_GameState = GameState.gameVictory;
		SfxManager.Instance.PlaySfx(Constants.VICTORY_SFX);
		EventManager.Instance.Raise(new GameVictoryEvent());
	}

    private void Settings()
    {
        SetTimeScale(0);
        m_GameState = GameState.gameMenu;
        
        EventManager.Instance.Raise(new GameSettingsEvent());
    }
    private void Controls()
    {
        SetTimeScale(0);
        m_GameState = GameState.gameMenu;
        
        EventManager.Instance.Raise(new GameControlsEvent());
    }
}

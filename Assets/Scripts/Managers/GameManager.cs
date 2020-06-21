using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using System.Dynamic;

public enum GameState { gameMenu, gamePlay, gamePause, gameOver, gameVictory}

public class GameManager : Manager<GameManager> {

	private bool _isArcadeMode = false;

	private GameState m_GameState;
	public bool IsPlaying { get { return m_GameState == GameState.gamePlay; } }

	private float m_TimeScale;
	public float TimeScale { get => m_TimeScale; }
	void SetTimeScale(float newTimeScale)
	{
		m_TimeScale = newTimeScale;
		Time.timeScale = m_TimeScale;
	}

	public int Score { get; set; }

	public int BestScore
	{
		get => PlayerPrefs.GetInt("BEST_SCORE", 0);
		set => PlayerPrefs.SetInt("BEST_SCORE", value);
	}

	void IncrementScore(int increment) => SetScore(Score + increment);

	void SetScore(int score)
	{
		Score = score;
		EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eBestScore = BestScore, eScore = Score, eNLives = NLives});
	}

	[Header("GameManager")]
	[SerializeField] private int m_NStartLives;

	public int NLives { get; private set; }

	void DecrementNLives(int decrement) => SetNLives(NLives-decrement);

    void SetNLives(int nLives)
	{
		NLives = nLives;
		EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eBestScore = BestScore, eScore = Score, eNLives = NLives});
	}

    [SerializeField]
	List<PlayerController> m_Players = new List<PlayerController>();

	public PlayerController GetPlayer { get { return m_Players[UnityEngine.Random.Range(0,m_Players.Count)]; } }

	#region Events' subscription

	/// <summary>
	/// TODO: Voir si c est vraiment utile car lol un peu
	/// </summary>
	public override void SubscribeEvents()
	{
		base.SubscribeEvents();
		EventManager.Instance.AddListener<PlayerHasBeenHitEvent>(PlayerHasBeenHit);
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
		EventManager.Instance.AddListener<ArcadeButtonClickedEvent>(ArcadeButtonClicked);
        EventManager.Instance.AddListener<NormalButtonClickedEvent>(NormalButtonClicked);
        EventManager.Instance.AddListener<AboutUsButtonClickedEvent>(AboutUsButtonClicked);
        EventManager.Instance.AddListener<CrewButtonClickedEvent>(CrewButtonClicked);
        EventManager.Instance.AddListener<LevelHasEnded>(Victory);
		EventManager.Instance.AddListener<ScoreItemEvent>(ScoreHasBeenGained);
		EventManager.Instance.AddListener<PatternHasBeenInstantiatedEvent>(PatternHasBeenInstantiated);
		EventManager.Instance.AddListener<AllEnemiesOfPatternHaveBeenDestroyedEvent>(AllEnemiesOfPatternHaveBeenDestroyed);
	}

	/// <summary>
	/// TODO: Same
	/// </summary>
	public override void UnsubscribeEvents()
	{
		base.UnsubscribeEvents();
		EventManager.Instance.RemoveListener<PlayerHasBeenHitEvent>(PlayerHasBeenHit);
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
		EventManager.Instance.RemoveListener<ArcadeButtonClickedEvent>(ArcadeButtonClicked);
        EventManager.Instance.RemoveListener<NormalButtonClickedEvent>(NormalButtonClicked);
        EventManager.Instance.RemoveListener<AboutUsButtonClickedEvent>(AboutUsButtonClicked);
        EventManager.Instance.RemoveListener<CrewButtonClickedEvent>(CrewButtonClicked);
        EventManager.Instance.RemoveListener<LevelHasEnded>(Victory);
		EventManager.Instance.RemoveListener<ScoreItemEvent>(ScoreHasBeenGained);
		EventManager.Instance.RemoveListener<PatternHasBeenInstantiatedEvent>(PatternHasBeenInstantiated);
		EventManager.Instance.RemoveListener<AllEnemiesOfPatternHaveBeenDestroyedEvent>(AllEnemiesOfPatternHaveBeenDestroyed);
	}
	#endregion

	#region Manager implementation

	protected override IEnumerator InitCoroutine()
	{
		Menu();
		EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eBestScore = BestScore, eScore = 0, eNLives = 0});
        EventManager.Instance.Raise(new GameBossShotedEvent() { eNLives = 0 });
		yield break;
	}
	#endregion

	private void InitNewGame()
	{
		SetScore(0);
		SetNLives(m_NStartLives);
	}

	#region Callbacks to events issued by Score items

	private void PlayerHasBeenHit(PlayerHasBeenHitEvent e)
	{
		if (e.eOneShot)
			DecrementNLives(999);
		else
			DecrementNLives(1);
		if (NLives <= 0)
			Over();
	}

	#endregion

	#region Callbacks to events issued by Score items

	private void ScoreHasBeenGained(ScoreItemEvent e) => IncrementScore(e.eScore.Score);

	#endregion

	#region Callbacks to events issued by Pattern
	/// <summary>
	/// TODO: Voir si on peut delete (je pense que oui)
	/// </summary>
	/// <param name="e"></param>
	private void PatternHasBeenInstantiated(PatternHasBeenInstantiatedEvent e)
	{ }
	/// <summary>
	/// TODO: Same
	/// </summary>
	/// <param name="e"></param>
	private void AllEnemiesOfPatternHaveBeenDestroyed(AllEnemiesOfPatternHaveBeenDestroyedEvent e)
	{ }
	#endregion



	#region Callbacks to Events issued by MenuManager
	private void MainMenuButtonClicked(MainMenuButtonClickedEvent e) => Menu();

    private void MainMenuButtonFromSettingsClicked(MainMenuButtonFromSettingsClickedEvent e) => MenuFromSettings();

	private void PlayButtonClicked(PlayButtonClickedEvent e) => ChooseType();

	private void BeginnerButtonClicked(BeginnerButtonClickedEvent e) => BeginnerLevel();

	private void IntermediateButtonClicked(IntermediateButtonClickedEvent e) => IntermediateLevel();

	private void DifficultButtonClicked(DifficultButtonClickedEvent e) => DifficultLevel();

	private void ArcadeButtonClicked(ArcadeButtonClickedEvent e) => Arcade();

    private void NormalButtonClicked(NormalButtonClickedEvent e) => ChooseLevel();

    private void AboutUsButtonClicked(AboutUsButtonClickedEvent e) => AboutUs();

    private void CrewButtonClicked(CrewButtonClickedEvent e) => Crew();

	private void NextLevelButtonClicked(NextLevelButtonClickedEvent e) => EventManager.Instance.Raise(new GoToNextLevelEvent());

	private void ResumeButtonClicked(ResumeButtonClickedEvent e) => Resume();

	/// <summary>
	/// Perso j aimerai bien que quand on appuie sur esc en pause on relance le jeu :)
	/// </summary>
	/// <param name="e"></param>
	private void EscapeButtonClicked(EscapeButtonClickedEvent e)
	{
		if (IsPlaying) Pause();
	}
    private void SettingsButtonClicked(SettingsButtonClickedEvent e) => Settings();

    private void ControlsButtonClicked(ControlsButtonClickedEvent e) => Controls();

    #endregion



    #region Events

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

    private void ChooseType()
    {
        SetTimeScale(0);
        m_GameState = GameState.gameMenu;
        EventManager.Instance.Raise(new GameChooseTypeEvent());
    }
	private void Play()
	{
		InitNewGame();
		SetTimeScale(1);
		m_GameState = GameState.gamePlay;
	}

    private void AboutUs()
    {
        SetTimeScale(0);
        m_GameState = GameState.gameMenu;
        EventManager.Instance.Raise(new GameAboutUsEvent());
    }
    private void Crew()
    {
        SetTimeScale(0);
        m_GameState = GameState.gameMenu;
        EventManager.Instance.Raise(new GameCrewEvent());
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

	private void Arcade()
	{
		Play();
		EventManager.Instance.Raise(new GameArcadeEvent());
		MusicLoopsManager.Instance.PlayMusic(Constants.GAMEPLAY_MUSIC);
		_isArcadeMode = true;
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
		HudManager.Instance.m_PrefNewBestScore.SetActive(_isArcadeMode && Score > BestScore);
		HudManager.Instance.m_PrefGameOverScore.SetActive(_isArcadeMode);
		if (_isArcadeMode && Score > BestScore)
        {
            BestScore = Score;
            EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eBestScore = BestScore, eNLives = NLives, eScore = Score });
        }
        SfxManager.Instance.PlaySfx(Constants.GAMEOVER_SFX);
		EventManager.Instance.Raise(new GameOverEvent() { eIsArcadeMode = _isArcadeMode });
		_isArcadeMode = false;
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

    #endregion
}

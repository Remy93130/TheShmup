using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class MenuManager : Manager<MenuManager>
{

	[Header("MenuManager")]

	#region Panels
	[Header("Panels")]
	[SerializeField]
	GameObject m_PanelMainMenu;
	[SerializeField] GameObject m_PanelInGameMenu;
	[HideInInspector] [SerializeField] GameObject m_PanelNextLevel; // Pour ce template de Shmup pas de Levels différents, juste des patterns qui s'enchaînent, donc pas besoin d'écran de passage d'un level à l'autre
	[SerializeField] GameObject m_PanelVictory;
	[SerializeField] GameObject m_PanelGameOver;

    [SerializeField] GameObject m_PanelSettings;
    [SerializeField] GameObject m_PanelControls;
	[SerializeField] GameObject m_PanelChooseLevel;
    [SerializeField] GameObject m_PanelChooseType;
    [SerializeField] GameObject m_PanelAboutUs;
    [SerializeField] GameObject m_PanelCrew;


    List<GameObject> m_AllPanels;
	#endregion

	#region Events' subscription
	public override void SubscribeEvents()
	{
		base.SubscribeEvents();
		EventManager.Instance.AddListener<AskToGoToNextLevelEvent>(AskToGoToNextLevel);
		EventManager.Instance.AddListener<GoToNextLevelEvent>(GoToNextLevel);
	}

	public override void UnsubscribeEvents()
	{
		base.UnsubscribeEvents();
		EventManager.Instance.RemoveListener<AskToGoToNextLevelEvent>(AskToGoToNextLevel);
		EventManager.Instance.RemoveListener<GoToNextLevelEvent>(GoToNextLevel);
	}
	#endregion

	#region Manager implementation
	protected override IEnumerator InitCoroutine()
	{
		yield break;
	}
	#endregion

	#region Monobehaviour lifecycle
	protected override void Awake()
	{
		base.Awake();
		RegisterPanels();
	}

	private void Update()
	{
		if (Input.GetButtonDown("Cancel"))
		{
			EscapeButtonHasBeenClicked();
		}
	}
	#endregion

	#region Panel Methods
	void RegisterPanels()
	{
		m_AllPanels = new List<GameObject>();
		if(m_PanelMainMenu)m_AllPanels.Add(m_PanelMainMenu);
		if(m_PanelInGameMenu) m_AllPanels.Add(m_PanelInGameMenu);
		if (m_PanelNextLevel) m_AllPanels.Add(m_PanelNextLevel);
		if (m_PanelVictory) m_AllPanels.Add(m_PanelVictory);
		if (m_PanelGameOver) m_AllPanels.Add(m_PanelGameOver);
        if (m_PanelSettings) m_AllPanels.Add(m_PanelSettings);
        if (m_PanelControls) m_AllPanels.Add(m_PanelControls);
		if (m_PanelChooseLevel) m_AllPanels.Add(m_PanelChooseLevel);
        if (m_PanelChooseType) m_AllPanels.Add(m_PanelChooseType);
        if (m_PanelAboutUs) m_AllPanels.Add(m_PanelAboutUs);
        if (m_PanelCrew) m_AllPanels.Add(m_PanelCrew);

    }
    

	void OpenPanel(GameObject panel)
	{
		foreach (var item in m_AllPanels)
			if (item)
				item.SetActive(item == panel);
	}
	#endregion

	#region UI OnClick Events
	public void EscapeButtonHasBeenClicked() => EventManager.Instance.Raise(new EscapeButtonClickedEvent());

	public void PlayButtonHasBeenClicked()
	{
		EventManager.Instance.Raise(new PlayButtonClickedEvent());
		EventManager.Instance.Raise(new ChangeBackgroundEvent() { eDefaultBackground = true });
	}

	public void ResumeButtonHasBeenClicked() => EventManager.Instance.Raise(new ResumeButtonClickedEvent());

	public void MainMenuButtonHasBeenClicked() => EventManager.Instance.Raise(new MainMenuButtonClickedEvent());

	public void AboutUsButtonHasBeenClicked() => EventManager.Instance.Raise(new AboutUsButtonClickedEvent());

	public void CrewButtonHasBeenClicked() => EventManager.Instance.Raise(new CrewButtonClickedEvent());

	public void MainMenuButtonFromSettingsHasBeenClicked() => EventManager.Instance.Raise(new MainMenuButtonFromSettingsClickedEvent());

	public void NextLevelButtonHasBeenClicked() => EventManager.Instance.Raise(new NextLevelButtonClickedEvent());


	public void SettingsButtonHasBeenClicked() => EventManager.Instance.Raise(new SettingsButtonClickedEvent());

	public void ControlsButtonHasBeenClicked() => EventManager.Instance.Raise(new ControlsButtonClickedEvent());

	public void BeginnerButtonHasBeenClicked() => EventManager.Instance.Raise(new BeginnerButtonClickedEvent());

	public void IntermediateButtonHasBeenClicked() => EventManager.Instance.Raise(new IntermediateButtonClickedEvent());

	public void DifficultButtonHasBeenClicked() => EventManager.Instance.Raise(new DifficultButtonClickedEvent());
	public void ArcadeButtonHasBeenClicked() => EventManager.Instance.Raise(new ArcadeButtonClickedEvent());

	public void NormalButtonHasBeenClicked() => EventManager.Instance.Raise(new NormalButtonClickedEvent());

	#endregion

	#region Callbacks to GameManager events
	private void AskToGoToNextLevel(AskToGoToNextLevelEvent e) => OpenPanel(m_PanelNextLevel);

	private void GoToNextLevel(GoToNextLevelEvent e) => OpenPanel(null);

	protected override void GameMenu(GameMenuEvent e) => OpenPanel(m_PanelMainMenu);

	protected override void AboutUs(GameAboutUsEvent e) => OpenPanel(m_PanelAboutUs);
	protected override void Crew(GameCrewEvent e) => OpenPanel(m_PanelCrew);
	protected override void GameBeginnerLevelPlay(GameBeginnerLevelEvent e) => OpenPanel(null);
	protected override void GameIntermediateLevelPlay(GameIntermediateLevelEvent e) => OpenPanel(null);
	protected override void GameDifficultLevelPlay(GameDifficultLevelEvent e) => OpenPanel(null);
	protected override void GameArcadePlay(GameArcadeEvent e) => OpenPanel(null);
	protected override void GamePause(GamePauseEvent e) => OpenPanel(m_PanelInGameMenu);

	protected override void GameResume(GameResumeEvent e) => OpenPanel(null);

	protected override void GameOver(GameOverEvent e) => OpenPanel(m_PanelGameOver);

	protected override void GameVictory(GameVictoryEvent e) => OpenPanel(m_PanelVictory);

	protected override void GameSettings(GameSettingsEvent e) => OpenPanel(m_PanelSettings);

	protected override void GameControls(GameControlsEvent e) => OpenPanel(m_PanelControls);

	protected override void GameChooseLevel(GameChooseLevelEvent e) => OpenPanel(m_PanelChooseLevel);

	protected override void GameChooseType(GameChooseTypeEvent e) => OpenPanel(m_PanelChooseType);

	public void QuitGame()
    {
        #if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
        #else
			Application.Quit();
        #endif
        
    }

	/// <summary>
	/// TODO: On utilise ce truc ?
	/// </summary>
	/// <param name="obj"></param>
    public void setActiveRecursif(GameObject obj)
    {
		Debug.LogWarning("Haha we use it");
		bool active = (obj.activeSelf)? false : true;
		obj.SetActive(active);
    }

    #endregion
}

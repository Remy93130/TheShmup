using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

#region GameManager Events
public class GameMenuEvent : SDD.Events.Event
{
}
public class GameBeginnerLevelEvent : SDD.Events.Event
{
}
public class GameIntermediateLevelEvent : SDD.Events.Event
{
}
public class GameDifficultLevelEvent : SDD.Events.Event
{
}
public class GamePauseEvent : SDD.Events.Event
{
}
public class GameResumeEvent : SDD.Events.Event
{
}
public class GameOverEvent : SDD.Events.Event
{
}
public class GameVictoryEvent : SDD.Events.Event
{
}
public class GameSettingsEvent : SDD.Events.Event
{
}
public class GameControlsEvent : SDD.Events.Event
{
}
public class GameChooseLevelEvent : SDD.Events.Event
{
}
public class GameStatisticsChangedEvent : SDD.Events.Event
{
	public int eBestScore { get; set; }
	public int eScore { get; set; }
	public int eNLives { get; set; }
	//public int eNEnemiesLeftBeforeVictory { get; set; }
}
#endregion

#region MenuManager Events
public class EscapeButtonClickedEvent : SDD.Events.Event
{
}
public class PlayButtonClickedEvent : SDD.Events.Event
{
}
public class ResumeButtonClickedEvent : SDD.Events.Event
{
}
public class MainMenuButtonClickedEvent : SDD.Events.Event
{
}
public class MainMenuButtonFromSettingsClickedEvent : SDD.Events.Event
{
}
public class NextLevelButtonClickedEvent : SDD.Events.Event
{
}
public class SettingsButtonClickedEvent : SDD.Events.Event
{
}
public class ControlsButtonClickedEvent : SDD.Events.Event
{
}
public class BeginnerButtonClickedEvent : SDD.Events.Event
{
}
public class IntermediateButtonClickedEvent : SDD.Events.Event
{
}
public class DifficultButtonClickedEvent : SDD.Events.Event
{
}
#endregion

#region Enemy Event
public class EnemyHasBeenDestroyedEvent: SDD.Events.Event
{
	public Enemy eEnemy;
	public bool eDestroyedByPlayer;
}
#endregion

#region Score Event
public class ScoreItemEvent : SDD.Events.Event
{
	public IScore eScore;
}
#endregion

#region Game Manager Additional Event
public class AskToGoToNextLevelEvent : SDD.Events.Event
{
}
public class GoToNextLevelEvent : SDD.Events.Event
{
}
#endregion

#region Player Events
public class PlayerHasBeenHitEvent:SDD.Events.Event
{
	public PlayerController ePlayerController;
}
#endregion

#region Pattern Events
public class AllEnemiesOfPatternHaveBeenDestroyedEvent : SDD.Events.Event
{
}

public class PatternHasFinishedSpawningEvent:SDD.Events.Event
{

}
#endregion

#region EnemiesManager Events
public class PatternHasBeenInstantiatedEvent : SDD.Events.Event
{
	public IPattern ePattern;
}
public class GoToNextPatternEvent : SDD.Events.Event
{
}

public class LevelHasEnded : SDD.Events.Event
{
}
#endregion
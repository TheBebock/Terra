using System;
using System.Collections.Generic;
using Terra.Core.Generics;
using Terra.GameStates;
using Terra.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Terra.Managers
{
    public class GameManager : PersistentMonoSingleton<GameManager>, IWithSetUp
    {
    
        [FormerlySerializedAs("currentGameState")] [SerializeField] private GameState _currentGameState;
        [FormerlySerializedAs("previousGameState")] [SerializeField] private GameState _previousGameState;
    
    
        private readonly Dictionary<Type, GameState> _allGameStates = new();


        protected override void Awake()
        {
            base.Awake();
            if(!Instance == this) return;
        
            _allGameStates.Clear();
            _allGameStates.Add(typeof(GameplayState), new GameplayState());
            _allGameStates.Add(typeof(UpgradeGameState), new UpgradeGameState());
            _allGameStates.Add(typeof(LoadGameState), new LoadGameState());
            _allGameStates.Add(typeof(DefaultGameState), new DefaultGameState());
            _allGameStates.Add(typeof(EndOfFloorState), new EndOfFloorState());
            _allGameStates.Add(typeof(StartOfFloorState), new StartOfFloorState());
            _allGameStates.Add(typeof(PlayerDeadState), new PlayerDeadState());
        }

        public void SetUp()
        {
#if UNITY_EDITOR

            if (ScenesManager.Instance.CurrentSceneName == SceneNames.Gameplay)
            {
                SwitchToGameState<StartOfFloorState>();
                return;
            }
#endif
            SwitchToGameState<DefaultGameState>();
        }
    

        public void SwitchToGameState<T>() where T : GameState
        {
            // Check if the game state exists in the dictionary
            if (!_allGameStates.ContainsKey(typeof(T)))
            {
                Debug.LogError($"{this}: Couldn't switch to new game state, game state {typeof(T)} doesn't exist in the dictionary.");
            }
        
            GameState newGameState = _allGameStates[typeof(T)];
            SwitchToNewGameState(newGameState);
        }

        public void SwitchToPreviousGameState()
        {
            GameState newGameState = _previousGameState;
            if (newGameState == null)
                Debug.LogWarning($"[{this}] Couldn't switch to previous games state, game state is empty");
            else
                SwitchToNewGameState(newGameState);
        }
        
        /// <summary>
        /// Checks if the current game state is of desired type.
        /// </summary>
        public bool IsCurrentState<T>() where T : GameState => _currentGameState is T;

        private void SwitchToNewGameState(GameState newGameState)
        {
            _previousGameState = _currentGameState;
            _previousGameState?.OnExit();
            _currentGameState = newGameState;
            _currentGameState.OnEnter();
        }



        public void TearDown()
        {
            _allGameStates.Clear();
        }
    }
}

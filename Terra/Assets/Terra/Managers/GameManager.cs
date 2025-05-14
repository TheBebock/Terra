using System;
using System.Collections.Generic;
using Terra.Core.Generics;
using Terra.GameStates;
using Terra.Interfaces;
using UnityEngine;

namespace Terra.Managers
{
    public class GameManager : PersistentMonoSingleton<GameManager>, IWithSetUp
    {
    
        [SerializeField] private GameState currentGameState;
        [SerializeField] private GameState previousGameState;
    
    
        private readonly Dictionary<Type, GameState> allGameStates = new();


        protected override void Awake()
        {
            base.Awake();
            if(!Instance == this) return;
        
            allGameStates.Clear();
            allGameStates.Add(typeof(NewGameState), new NewGameState());
            allGameStates.Add(typeof(LoadGameState), new LoadGameState());
            allGameStates.Add(typeof(MainMenuState), new MainMenuState());
            allGameStates.Add(typeof(DefaultGameState), new DefaultGameState());
        }

        public void SetUp()
        {
                
#if UNITY_EDITOR
            if (ScenesManager.Instance.CurrentSceneName == SceneNames.Gameplay)
            {
                SwitchToNewGameState<DefaultGameState>();
                return;
            }
#endif
        
            SwitchToNewGameState<MainMenuState>();
        }
    

        public void SwitchToNewGameState<T>() where T : GameState
        {
            // Check if the game state exists in the dictionary
            if (!allGameStates.ContainsKey(typeof(T)))
            {
                Debug.LogWarning($"[{this}] Couldn't switch to new game state, game state {typeof(T)} doesn't exist in the dictionary.");
            }
        
            GameState newGameState = allGameStates[typeof(T)];
            SwitchToNewGameState(newGameState);
        }

        public void SwitchToPreviousGameState()
        {
            GameState newGameState = previousGameState;
            if (newGameState == null)
                Debug.LogWarning($"[{this}] Couldn't switch to previous games state, game state is empty");
            else
                SwitchToNewGameState(newGameState);
        }

        public void StartNewGame()
        {
            SwitchToNewGameState<NewGameState>();
        }

        public void LoadGame()
        {
            SwitchToNewGameState<LoadGameState>();

        }

        /// <summary>
        /// Checks if the current game state is of desired type.
        /// </summary>
        public bool IsCurrentState<T>() where T : GameState => currentGameState is T;

        private void SwitchToNewGameState(GameState newGameState)
        {
            previousGameState = currentGameState;
            previousGameState?.OnExit();
            currentGameState = newGameState;
            currentGameState.OnEnter();
        }



        public void TearDown()
        {
            allGameStates.Clear();
        }
    }
}

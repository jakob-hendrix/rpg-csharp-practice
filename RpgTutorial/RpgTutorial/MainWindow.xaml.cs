﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Engine.EventArgs;
using Engine.Models;
using Engine.ViewModels;

namespace RpgTutorial
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly GameSession _gameSession;
        private readonly Dictionary<Key, Action> _userInputActions = new Dictionary<Key, Action>();

        public MainWindow()
        {
            InitializeComponent();
            InitializeUserInputActions();
            _gameSession = new GameSession();
            _gameSession.OnMessageRaised += OnGameMessageRaised;
            DataContext = _gameSession;
        }

        private void OnClick_MoveNorth(object sender, RoutedEventArgs e) => _gameSession.MoveNorth();

        private void OnClick_MoveWest(object sender, RoutedEventArgs e) => _gameSession.MoveWest();

        private void OnClick_MoveEast(object sender, RoutedEventArgs e) => _gameSession.MoveEast();

        private void OnClick_MoveSouth(object sender, RoutedEventArgs e) => _gameSession.MoveSouth();

        private void OnGameMessageRaised(object sender, GameMessageEventArgs e)
        {
            GameMessages.Document.Blocks.Add(
                new Paragraph(
                    new Run(e.Message)
                )
            );
            GameMessages.ScrollToEnd();
        }

        private void OnClick_AttackMonster(object sender, RoutedEventArgs e) => _gameSession.AttackCurrentMonster();

        private void OnClick_DisplayTradeScreen(object sender, RoutedEventArgs e)
        {
            TradeScreen tradeScreen = new TradeScreen
            {
                Owner = this, 
                DataContext = _gameSession
            };
            tradeScreen.ShowDialog();
        }
        private void OnClick_UseCurrentConsumable(object sender, RoutedEventArgs e) => _gameSession.UseCurrentConsumable();

        private void OnClick_Craft(object sender, RoutedEventArgs e)
        {  
            Recipe recipe = ((FrameworkElement)sender).DataContext as Recipe;
            _gameSession.CraftItemUsing(recipe);
        }

        private void InitializeUserInputActions()
        {
            // movement
            _userInputActions.Add(Key.W, () => _gameSession.MoveNorth());
            _userInputActions.Add(Key.A, () => _gameSession.MoveWest());
            _userInputActions.Add(Key.D, () => _gameSession.MoveEast());
            _userInputActions.Add(Key.S, () => _gameSession.MoveSouth());

            // player actions
            _userInputActions.Add(Key.Z, () => _gameSession.AttackCurrentMonster());
            _userInputActions.Add(Key.C, () => _gameSession.UseCurrentConsumable());

            // player data tabs
            _userInputActions.Add(Key.I, () => SetTabFocusTo("InventoryTabItem"));
            _userInputActions.Add(Key.Q, () => SetTabFocusTo("QuestTabItem"));
            _userInputActions.Add(Key.R, () => SetTabFocusTo("RecipeTabItem"));
        }

        private void SetTabFocusTo(string tabName)
        {
            foreach (object item in PlayerDataTabControl.Items)
            {
                if (item is TabItem tabItem)
                {
                    if (tabItem.Name == tabName)
                    {
                        tabItem.IsSelected = true;
                        return;
                    }
                }
            }
        }

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (_userInputActions.ContainsKey(e.Key))
            {
                _userInputActions[e.Key].Invoke();
            }
        }
    }
}

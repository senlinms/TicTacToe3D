﻿using System;
using Zenject;

namespace TicTacToe3D
{
    public class AdvancedSettingsPresenter : IMenuPresenter, IInitializable, IDisposable
    {
        private AdvancedSettingsView View { get; set; }
        private NewGameMenuPresenter NewGameMenu { get; set; }
        private GameInformationPresenter GameInformationPanel { get; set; }
        private GameInfo Info { get; set; }
        private GameSettings GameSettings { get; set; }

        public AdvancedSettingsPresenter(NewGameMenuPresenter newGameMenu, GameInfo info, GameSettings gameSettings)
        {
            NewGameMenu = newGameMenu;
            Info = info;
            GameSettings = gameSettings;
        }

        [Inject]
        private void InjectPanels(GameInformationPresenter gameInformationPanel)
        {
            GameInformationPanel = gameInformationPanel;
        }

        public void SetView(AdvancedSettingsView view)
        {
            View = view;
        }

        public void Initialize()
        {
            View.DimensionSlider.maxValue = GameSettings.DimensionMax;
            View.BadgesToWinSlider.maxValue = View.DimensionSlider.value;
            View.StepSizeSlider.maxValue = GameSettings.StepSizeMax;

            View.GameInformationButton.onClick.AddListener(OnGameInformationButtonClicked);

            View.DimensionSlider.onValueChanged.AddListener(OnDimensionSliderChanged);
            View.BadgesToWinSlider.onValueChanged.AddListener(OnBadgesToWinSliderChanged);
            View.StepSizeSlider.onValueChanged.AddListener(OnStepSizeSliderChanged);

            Info.Dimension = (int) View.DimensionSlider.value;
            Info.BadgesToWin = (int) View.BadgesToWinSlider.value;
            Info.StepSize = (int) View.StepSizeSlider.value;

            View.DimensionAmountText.text = Info.Dimension.ToString();
            View.BadgesToWinAmountText.text = Info.BadgesToWin.ToString();
            View.StepSizeAmountText.text = Info.StepSize.ToString();
        }

        public void Dispose()
        {
            View.GameInformationButton.onClick.RemoveAllListeners();

            View.DimensionSlider.onValueChanged.RemoveAllListeners();
            View.BadgesToWinSlider.onValueChanged.RemoveAllListeners();
            View.StepSizeSlider.onValueChanged.RemoveAllListeners();
        }

        public void Open()
        {
            View.IsOpen = true;
        }

        public void Close()
        {
            View.IsOpen = false;
        }

        private void OnDimensionSliderChanged(float value)
        {
            View.BadgesToWinSlider.maxValue = value;
            Info.Dimension = (int) value;
            View.DimensionAmountText.text = Info.Dimension.ToString();
            View.BadgesToWinAmountText.text = Info.BadgesToWin.ToString();
        }

        private void OnBadgesToWinSliderChanged(float value)
        {
            Info.BadgesToWin = (int) value;
            View.BadgesToWinAmountText.text = Info.BadgesToWin.ToString();
        }

        private void OnStepSizeSliderChanged(float value)
        {
            Info.StepSize = (int) value;
            View.StepSizeAmountText.text = Info.StepSize.ToString();
        }

        private void OnGameInformationButtonClicked()
        {
            NewGameMenu.OpenPanel(GameInformationPanel);
        }
    }
}
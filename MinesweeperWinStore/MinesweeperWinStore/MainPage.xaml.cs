﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MinesweeperWinStore
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            fillSettingsComboBoxes();
            heightComboBox.SelectedValue = 8;
            widthComboBox.SelectedValue = 8;
            minesComboBox.SelectedValue = 10;
        }

        private void startGameBtn_Click(object sender, RoutedEventArgs e)
        {
            BoardConfiguration boardConfig;
            int h = (int)heightComboBox.SelectedValue;
            int w = (int)widthComboBox.SelectedValue;
            int m = (int)minesComboBox.SelectedValue;
            boardConfig = new BoardConfiguration(h, w, m);
            this.Frame.Navigate(typeof(GamePage), boardConfig);
        }

        private void beginnerRdoBtn_Click(object sender, RoutedEventArgs e)
        {
            disableSettingsComboBoxes();
            heightComboBox.SelectedValue = 8;
            widthComboBox.SelectedValue = 8;
            minesComboBox.SelectedValue = 10;
        }

        private void intermediateRdoBtn_Click(object sender, RoutedEventArgs e)
        {
            disableSettingsComboBoxes();
            heightComboBox.SelectedValue = 16;
            widthComboBox.SelectedValue = 16;
            minesComboBox.SelectedValue = 40;
        }

        private void ExpertRdoBtn_Click(object sender, RoutedEventArgs e)
        {
            disableSettingsComboBoxes();
            heightComboBox.SelectedValue = 16;
            widthComboBox.SelectedValue = 30;
            minesComboBox.SelectedValue = 99;
        }

        private void customRdoBtn_Click(object sender, RoutedEventArgs e)
        {
            enableSettingsComboBoxes();
            changeMineComboBoxRange();
        }

        private void disableSettingsComboBoxes()
        {
            heightComboBox.IsEnabled = false;
            widthComboBox.IsEnabled = false;
            minesComboBox.IsEnabled = false;
        }

        private void enableSettingsComboBoxes()
        {
            heightComboBox.IsEnabled = true;
            widthComboBox.IsEnabled = true;
            minesComboBox.IsEnabled = true;
        }

        private void fillSettingsComboBoxes()
        {
            for (int i = 8; i <= 30; i++)
            {
                heightComboBox.Items.Add(i);
                widthComboBox.Items.Add(i);
            }
            for (int m = 10; m <= 600; m++)
                minesComboBox.Items.Add(m);
        }

        private void heightComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeMineComboBoxRange();
        }

        private void widthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeMineComboBoxRange();
        }

        private void changeMineComboBoxRange()
        {
            if (heightComboBox.SelectedValue != null && widthComboBox.SelectedValue != null && minesComboBox.SelectedValue != null)
            {
                int height = (int)heightComboBox.SelectedValue;
                int width = (int)widthComboBox.SelectedValue;
                int maxMines = (height * width);

                int selectedNumMines = (int)minesComboBox.SelectedValue;

                minesComboBox.Items.Clear();
                for (int m = 10; m < maxMines; m++)
                    minesComboBox.Items.Add(m);

                if (selectedNumMines > maxMines - 1)
                    minesComboBox.SelectedIndex = minesComboBox.Items.Count - 1;
                else
                    minesComboBox.SelectedValue = selectedNumMines;
            }
        }
    }
}
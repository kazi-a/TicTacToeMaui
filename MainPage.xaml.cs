using System;
using Microsoft.Maui.Controls;
using Plugin.Maui.Audio;

namespace TicTacToeMaui
{
    public partial class MainPage : ContentPage
    {
        private string currentPlayer;
        private string[,] board;
        private bool gameEnded;

        public MainPage()
        {
            InitializeComponent();
            StartNewGame();
        }

        private void StartNewGame()
        {
            currentPlayer = "❌";
            board = new string[3, 3];
            gameEnded = false;
            statusLabel.Text = "Player ❌'s turn";
            ResetButtons();
            AnimateTitle();  // Start title animation on new game
        }

        private void ResetButtons()
        {
            btn00.Text = btn01.Text = btn02.Text = "";
            btn10.Text = btn11.Text = btn12.Text = "";
            btn20.Text = btn21.Text = btn22.Text = "";
            btn00.IsEnabled = btn01.IsEnabled = btn02.IsEnabled = true;
            btn10.IsEnabled = btn11.IsEnabled = btn12.IsEnabled = true;
            btn20.IsEnabled = btn21.IsEnabled = btn22.IsEnabled = true;
        }

        private void OnButtonClicked(object sender, EventArgs e)
        {
            if (gameEnded) return;

            var button = (Button)sender;
            int row = Grid.GetRow(button);
            int col = Grid.GetColumn(button);

            if (string.IsNullOrEmpty(board[row, col]))
            {
                board[row, col] = currentPlayer;
                button.Text = currentPlayer;

                if (CheckForWinner())
                {
                    statusLabel.Text = $"Player {currentPlayer} wins!";
                    AnimateStatusLabel();  // Trigger color animation on win
                    gameEnded = true;
                    DisableAllButtons();
                }
                else if (CheckForDraw())
                {
                    statusLabel.Text = "It's a draw!";
                    AnimateStatusLabel();  // Trigger color animation on draw
                    gameEnded = true;
                }
                else
                {
                    SwitchPlayer();
                }
            }
        }

        private void SwitchPlayer()
        {
            currentPlayer = currentPlayer == "❌" ? "⭕" : "❌";
            statusLabel.Text = $"Player {currentPlayer}'s turn";
        }

        private bool CheckForWinner()
        {
            // Check rows, columns, and diagonals for a win
            for (int i = 0; i < 3; i++)
            {
                if (!string.IsNullOrEmpty(board[i, 0]) && board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2])
                    return true;
                if (!string.IsNullOrEmpty(board[0, i]) && board[0, i] == board[1, i] && board[1, i] == board[2, i])
                    return true;
            }
            if (!string.IsNullOrEmpty(board[0, 0]) && board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
                return true;
            if (!string.IsNullOrEmpty(board[0, 2]) && board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
                return true;

            return false;
        }

        private bool CheckForDraw()
        {
            foreach (var cell in board)
            {
                if (string.IsNullOrEmpty(cell))
                    return false;
            }
            return true;
        }

        private void DisableAllButtons()
        {
            btn00.IsEnabled = btn01.IsEnabled = btn02.IsEnabled = false;
            btn10.IsEnabled = btn11.IsEnabled = btn12.IsEnabled = false;
            btn20.IsEnabled = btn21.IsEnabled = btn22.IsEnabled = false;
        }

        private void OnRestartClicked(object sender, EventArgs e)
        {
            StartNewGame();
        }

        private async void AnimateTitle()
        {
            // Ensure XMasTitle is properly referenced in XAML

            // Title animation code (fade, scale, rotate, translate)
            await XMasTitle.FadeTo(0, 1000);  // Fade out
            await XMasTitle.FadeTo(1, 1000);  // Fade in
            await XMasTitle.ScaleTo(0.5, 1000);  // Zoom out
            await XMasTitle.ScaleTo(1, 1000);    // Zoom back in
            await XMasTitle.RotateTo(360, 2000);  // Rotate 360 degrees
            XMasTitle.Rotation = 0;               // Reset rotation
            await XMasTitle.TranslateTo(100, 0, 1000);  // Move right
            await XMasTitle.TranslateTo(0, 0, 1000);    // Move back to original position
        }

        private void AnimateStatusLabel()
        {
            // Color transition animation code
            var blackToRed = new Animation(v => statusLabel.TextColor = Color.FromRgba(0 + v, 0, 0, 1), 0, 1);
            var redToGreen = new Animation(v => statusLabel.TextColor = Color.FromRgba(1 - v, v, 0, 1), 0, 1);
            var greenToBlack = new Animation(v => statusLabel.TextColor = Color.FromRgba(0, 1 - v, 0, 1), 0, 1);

            var parentAnimation = new Animation();
            parentAnimation.Add(0, 0.33, blackToRed);  // Transition from black to red
            parentAnimation.Add(0.33, 0.66, redToGreen); // Transition from red to green
            parentAnimation.Add(0.66, 1, greenToBlack);  // Transition from green back to black

            parentAnimation.Commit(this, "TextColorAnimation", length: 3000, repeat: () => false);  // Commit animation
        }
    }
}

using System;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;
using TicTacToeGameProj.Services;

namespace TicTacToeGameProj
{
    public partial class LanguagePopup : Popup
    {
        public LanguagePopup()
        {
            InitializeComponent();
            BuildLanguageButtons();
        }

        private void BuildLanguageButtons()
        {
            LanguagesStack.Children.Clear();

            foreach (var lang in LanguageManager.Languages)
            {
                var btn = new Button
                {
                    Text = lang.DisplayName,
                    FontSize = 14,
                    CornerRadius = 18,
                    BackgroundColor = lang == LanguageManager.CurrentLanguage
                        ? Color.FromArgb("#B3EBED")
                        : Color.FromArgb("#D9D9D9"),
                    BorderColor = Color.FromArgb("#020136"),
                    BorderWidth = 1,
                    TextColor = Color.FromArgb("#020136"),
                    Padding = new Thickness(16, 4)
                };

                btn.Clicked += (_, _) => Close(lang.Key);

                LanguagesStack.Children.Add(btn);
            }
        }

        private void OnCloseClicked(object sender, EventArgs e)
        {
            Close(null);
        }
    }
}

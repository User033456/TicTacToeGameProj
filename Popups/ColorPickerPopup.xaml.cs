using CommunityToolkit.Maui.Views;
using Maui.ColorPicker;

namespace TicTacToeGameProj;

public partial class ColorPickerPopup : Popup
{
    public Color SelectedColor { get; private set; }

    public ColorPickerPopup(Color initialColor)
    {
        InitializeComponent();

        SelectedColor = initialColor;
        PreviewBox.Color = initialColor;

        MainColorPicker.PickedColor = initialColor;
        MainColorPicker.PickedColorChanged += MainColorPicker_PickedColorChanged;
    }

    private void MainColorPicker_PickedColorChanged(object sender, PickedColorChangedEventArgs e)
    {
        SelectedColor = e.NewPickedColorValue;
        PreviewBox.Color = e.NewPickedColorValue;
    }

    private void OnApplyClicked(object sender, EventArgs e)
    {
        Close(SelectedColor);
    }

    private void OnCloseClicked(object sender, EventArgs e)
    {
        Close(null);
    }
}
namespace CompressorApp;

public partial class MainPage : ContentPage
{
    int count = 0;

    public string fileName { get; set; } = string.Empty;


    public MainPage()
    {
        InitializeComponent();
    }


    private async Task<FileResult?> PickAndShow(PickOptions options)
    {
        try
        {
            var result = await FilePicker.Default.PickAsync(options);

            if (result == null) return result;

            if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
            {
                await using var stream = await result.OpenReadAsync();
                var image = ImageSource.FromStream(() => stream);
            }

            return result;
        }
        catch (Exception ex)
        {
        }

        return null;
    }

    private void ButtonPressed(object? sender, EventArgs e)
    {
        FileResult? fileResult = PickAndShow(new()
        {
            PickerTitle = "Please select a file",
            FileTypes = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.iOS, new[] { ".png", ".zip" } }, // UTType values
                    { DevicePlatform.Android, new[] { ".png", ".zip" } }, // MIME type
                    { DevicePlatform.WinUI, new[] { ".png", ".zip" } }, // file extension
                    { DevicePlatform.Tizen, new[] { ".png", ".zip" } },
                    { DevicePlatform.macOS, new[] { ".png", ".zip" } }, // UTType values
                }),
        }).Result;
    }
}
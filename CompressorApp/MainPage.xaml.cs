using System.Text;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;

namespace CompressorApp;

public partial class MainPage : ContentPage
{
    public string FileName => FileResult?.FileName ?? "No file selected";
    public FileResult? FileResult { get; set; }
    
    public string zipCount = "0";
    
    private int _zipCount = 0;
    

    public MainPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    private async Task<FileResult?> PickAndShow(PickOptions options)
    {
        try
        {
            var result = await FilePicker.Default.PickAsync(options);

            if (result == null) return result;

            OnPropertyChanged(nameof(FileName));

            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"How did we get here? {ex.Message}");
        }

        return null;
    }

    private async Task SaveFile(string fileName, FileStream stream, CancellationToken cancellationToken)
    {
        var fileSaverResult = await FileSaver.Default.SaveAsync(fileName, stream, cancellationToken);
        if (fileSaverResult.IsSuccessful)
        {
            await Toast.Make($"The file was saved successfully to location: {fileSaverResult.FilePath}")
                .Show(cancellationToken);
        }
        else
        {
            await Toast.Make($"The file was not saved successfully with error: {fileSaverResult.Exception.Message}")
                .Show(cancellationToken);
        }
    }

    public async void SelectButtonPressed(object? sender, EventArgs e)
    {
        FileResult = await PickAndShow(new()
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
        });
    }


    public async void SaveButtonPressed(object? sender, EventArgs e)
    {
        var cancellationToken = new CancellationToken();
        
        if (FileResult == null)
        {
            
             await Toast.Make($"Please select a file first.")
                .Show(cancellationToken);
        }
        try
        {
            await SaveFile(FileResult.FileName, new FileStream(FileResult.FullPath, FileMode.Open),
                new CancellationToken());
        }
        catch (Exception ex)
        {
             await Toast.Make($"ex.Message")
                .Show(cancellationToken);
        }
    }

    private void ZipButtonPressed(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
};
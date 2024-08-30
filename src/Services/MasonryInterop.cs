using Microsoft.JSInterop;

namespace AllTheLists.Services;

public interface IMasonryInterop
{
    Task Init(string parentSelector, string itemSelector, string columnWidth, bool percentPosition = true, float transitionDurationSecs = .2F);
}

public class MasonryInterop : IMasonryInterop
{
    private readonly IJSRuntime _jsRuntime;

    public MasonryInterop(IJSRuntime jSRuntime)
    {
        _jsRuntime = jSRuntime;
    }

    /// <summary>
    /// options ref : https://masonry.desandro.com/options.html
    /// </summary>
    /// <param name="parentSelector"></param>
    /// <param name="itemSelector"></param>
    /// <param name="percentPosition"></param>
    /// <param name="transitionDurationSecs"></param>
    /// <returns></returns>
    public async Task Init(string parentSelector, string itemSelector, string columnWidth, bool percentPosition = true, float transitionDurationSecs = 0.2f)
    {
        var transitionDurationStr = $"{transitionDurationSecs}s";

        await _jsRuntime.InvokeVoidAsync("initMasonry", parentSelector, itemSelector, columnWidth, percentPosition, transitionDurationStr);
    }
}
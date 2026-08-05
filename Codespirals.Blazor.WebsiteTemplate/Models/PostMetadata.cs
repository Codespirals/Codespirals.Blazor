using Codespirals.Base.Extensions;

namespace Codespirals.Blazor.WebsiteTemplate;

public class PostMetadata
{
    public string Id => $"{Title.MakeUrlSafe('_').Trim('_').ToLowerInvariant()}";
    public string? Group { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string[] Tags { get; set; } = [];
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime? Edited { get; set; }
    public string? ContentUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public bool ShowInList { get; set; } = true;
    public string StickerEmoji { get; set; } = "";
    public int TechnicalDifficulty { get; set; }
    public int? EstimatedReadTimeInMinutes { get; set; }
}

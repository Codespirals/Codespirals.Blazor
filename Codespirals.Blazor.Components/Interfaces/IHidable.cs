namespace Codespirals.Blazor.Components.Interfaces;

public interface IHidable
{
    public bool Visible { get; set; }
    public bool Hideable { get; init; }
}

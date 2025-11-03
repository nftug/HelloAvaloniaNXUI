namespace NXUI;

public static class GridDefinitionExtensions
{
    public static T ColumnDefinitions<T>(this T grid, string definitions) where T : Grid
    {
        grid.ColumnDefinitions.Clear();
        foreach (var def in definitions.Split(',', StringSplitOptions.RemoveEmptyEntries))
        {
            grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Parse(def.Trim())));
        }
        return grid;
    }

    public static T RowDefinitions<T>(this T grid, string definitions) where T : Grid
    {
        grid.RowDefinitions.Clear();
        foreach (var def in definitions.Split(',', StringSplitOptions.RemoveEmptyEntries))
        {
            grid.RowDefinitions.Add(new RowDefinition(GridLength.Parse(def.Trim())));
        }
        return grid;
    }
}

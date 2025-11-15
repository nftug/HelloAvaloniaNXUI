using Material.Icons;
using Material.Icons.Avalonia;

namespace HelloAvaloniaNXUI.Utils;

public class CustomIcon : PathIcon
{
    private readonly GeometryDrawing _geometryDrawing = new();
    private MaterialIcon? _imageSource;

    public static readonly StyledProperty<MaterialIcon?> SourceProperty =
        AvaloniaProperty.Register<CustomIcon, MaterialIcon?>(nameof(Source));

    public MaterialIcon? Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public CustomIcon()
    {
        _geometryDrawing.Brush = Foreground;
        AffectsRender<CustomIcon>(ForegroundProperty, DataProperty, SourceProperty);
    }

    public CustomIcon(MaterialIconKind iconKind)
    {
        Source = new MaterialIcon { Kind = iconKind };
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.Property == Avalonia.Controls.PathIcon.ForegroundProperty)
        {
            _geometryDrawing.Brush = Foreground;
            if (_imageSource is not null)
                _imageSource.Foreground = Foreground;
        }
        else if (e.Property == DataProperty)
        {
            _geometryDrawing.Geometry = Data;
        }
        else if (e.Property == SourceProperty)
        {
            _imageSource = Source;
        }
        else if (e.Property == FontSizeProperty)
        {
            Width = FontSize;
            Height = FontSize;
        }
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        if (_imageSource is not null)
        {
            // Image の場合
            var rect = new Rect(Bounds.Size);
            context.DrawImage(_imageSource, rect);
        }
        else if (_geometryDrawing.Geometry is not null)
        {
            // Geometry の場合
            var bounds = _geometryDrawing.Geometry.Bounds;
            if (bounds.Width <= 0 || bounds.Height <= 0)
                return;

            var scaleX = Bounds.Width / bounds.Width;
            var scaleY = Bounds.Height / bounds.Height;

            var translateToOrigin = Matrix.CreateTranslation(-bounds.X, -bounds.Y);
            var scale = Matrix.CreateScale(scaleX, scaleY);
            var translateToCenter = Matrix.CreateTranslation(
                (Bounds.Width - bounds.Width * scaleX) / 2,
                (Bounds.Height - bounds.Height * scaleY) / 2);

            using (context.PushTransform(translateToOrigin * scale * translateToCenter))
            {
                _geometryDrawing.Draw(context);
            }
        }
    }
}

namespace Geometrie;

public readonly struct Point2D
{
    public double X { get; init; }
    public double Y { get; init; }

    public Point2D(double x, double y)
    {
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}

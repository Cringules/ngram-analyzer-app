namespace Cringules.NGram.App.Model;

public class Peak
{
    public double Angle { get; }
    public double Distance { get; }
    public double MaxIntensity { get; }
    public double IntegralIntensity { get; }

    public Peak(double angle, double distance, double maxIntensity, double integralIntensity)
    {
        Angle = angle;
        Distance = distance;
        MaxIntensity = maxIntensity;
        IntegralIntensity = integralIntensity;
    }
}

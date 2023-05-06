using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Cringules.NGram.App.View;

public partial class NumericSlider : UserControl
{
    public NumericSlider()
    {
        InitializeComponent();
    }

    public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(
        nameof(Label), typeof(string), typeof(NumericSlider), new PropertyMetadata(default(string)));

    public string Label
    {
        get { return (string) GetValue(LabelProperty); }
        set { SetValue(LabelProperty, value); }
    }

    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        nameof(Value), typeof(double), typeof(NumericSlider),
        new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public double Value
    {
        get { return (double) GetValue(ValueProperty); }
        set { SetValue(ValueProperty, value); }
    }

    public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
        nameof(Minimum), typeof(double), typeof(NumericSlider), new PropertyMetadata(default(double)));

    public double Minimum
    {
        get { return (double) GetValue(MinimumProperty); }
        set { SetValue(MinimumProperty, value); }
    }

    public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
        nameof(Maximum), typeof(double), typeof(NumericSlider), new PropertyMetadata(default(double)));

    public double Maximum
    {
        get { return (double) GetValue(MaximumProperty); }
        set { SetValue(MaximumProperty, value); }
    }
}

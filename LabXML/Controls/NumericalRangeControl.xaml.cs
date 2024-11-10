using System.Windows;
using System.Windows.Controls;

namespace LabXML.Controls;

public partial class NumericalRangeControl : UserControl
{
    public static readonly DependencyProperty MinProperty =
        DependencyProperty.Register("Min", typeof(double), typeof(NumericalRangeControl));

    public static readonly DependencyProperty MaxProperty =
        DependencyProperty.Register("Max", typeof(double), typeof(NumericalRangeControl));

    public double Min
    {
        get => (double) GetValue(MinProperty);
        set => SetValue(MinProperty, value);
    }

    public double Max
    {
        get => (double)GetValue(MaxProperty);
        set => SetValue(MaxProperty, value);
    }
    public NumericalRangeControl()
    {
        InitializeComponent();
    }
}
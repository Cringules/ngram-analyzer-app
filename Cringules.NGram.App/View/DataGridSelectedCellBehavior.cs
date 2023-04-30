using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace Cringules.NGram.App.View;

public class DataGridSelectedCellBehavior : Behavior<DataGrid>
{
    public static readonly DependencyProperty SelectedCellProperty = DependencyProperty.Register(
        nameof(SelectedCell), typeof(DataGridCellInfo?), typeof(DataGridSelectedCellBehavior),
        new PropertyMetadata(default(DataGridCellInfo?)));

    public DataGridCellInfo? SelectedCell
    {
        get { return (DataGridCellInfo?) GetValue(SelectedCellProperty); }
        set { SetValue(SelectedCellProperty, value); }
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.SelectedCellsChanged += OnSelectedCellsChanged;
    }

    private void OnSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
    {
        if (SelectedCell.HasValue && e.RemovedCells.Contains(SelectedCell.Value))
        {
            SelectedCell = null;
        }

        if (e.AddedCells.Count > 0)
        {
            SelectedCell = e.AddedCells[0];
        }
    }
}

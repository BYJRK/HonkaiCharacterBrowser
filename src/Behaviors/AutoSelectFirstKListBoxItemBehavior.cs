using Microsoft.Xaml.Behaviors;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace HonkaiCharacterBrowser.Behaviors;

public class AutoSelectFirstKListBoxItemBehavior : Behavior<Selector>
{
    protected override void OnAttached()
    {
        base.OnAttached();

        AssociatedObject.TargetUpdated += AutoSelectFirst;
    }

    void AutoSelectFirst(object sender, DataTransferEventArgs e)
    {
        if (AssociatedObject.HasItems)
            AssociatedObject.SelectedIndex = 0;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();

        AssociatedObject.TargetUpdated -= AutoSelectFirst;
    }
}

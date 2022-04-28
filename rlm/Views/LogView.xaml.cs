using rlm.Dialogs;
using rlm.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace rlm.Views
{
    /// <summary>
    /// Interaction logic for LogView.xaml
    /// </summary>
    public partial class LogView : UserControl
    {
        public LogView()
        {
            InitializeComponent();
        }

        private void RaidEncounterCompletedActivityLogEntry_DoubleClick(object sender, MouseButtonEventArgs e) =>
            new RaidEncounterDetails((RaidEncounterCompletedActivityLogEntry)((ContentControl)sender).DataContext) { Owner = Window.GetWindow(this) }.ShowDialog();
    }
}

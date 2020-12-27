using rlm.Dialogs;
using rlm.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

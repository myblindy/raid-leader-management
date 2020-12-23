using ReactiveUI;
using rlm.ViewModels;

namespace rlm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ReactiveWindow<MainViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = ViewModel = new();
            this.WhenActivated(disposables => { });
        }
    }
}

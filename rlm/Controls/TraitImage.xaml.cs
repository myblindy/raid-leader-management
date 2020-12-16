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
using ReactiveUI;
using rlm.Models;

namespace rlm.Controls
{
    /// <summary>
    /// Interaction logic for TraitImage.xaml
    /// </summary>
    public partial class TraitImage : UserControl
    {
        public Trait Trait
        {
            get => (Trait)GetValue(TraitProperty);
            set => SetValue(TraitProperty, value);
        }

        public static readonly DependencyProperty TraitProperty = DependencyProperty.Register(nameof(Trait), typeof(Trait), typeof(TraitImage));

        public TraitImage()
        {
            InitializeComponent();
        }
    }
}

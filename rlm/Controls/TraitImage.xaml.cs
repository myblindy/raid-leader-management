using System.Windows;
using System.Windows.Controls;
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

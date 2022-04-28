using System.Windows;
using System.Windows.Controls;
using rlm.Models;

namespace rlm.Controls
{
    /// <summary>
    /// Interaction logic for EntityImage.xaml
    /// </summary>
    public partial class EntityImage : UserControl
    {
        public object Entity
        {
            get => GetValue(EntityProperty);
            set => SetValue(EntityProperty, value);
        }

        public static readonly DependencyProperty EntityProperty = DependencyProperty.Register(nameof(Entity), typeof(object), typeof(EntityImage));

        public EntityImage()
        {
            InitializeComponent();
        }
    }
}

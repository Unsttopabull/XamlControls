using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Frost.XamlControls {
    public class ImageButton : ButtonBase {
        public static readonly DependencyProperty EnabledSourceProperty = DependencyProperty.Register("EnabledSource", typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(default(ImageSource)));
        public static readonly DependencyProperty DisabledSourceProperty = DependencyProperty.Register("DisabledSource", typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(default(ImageSource)));
        public static readonly DependencyProperty ImageMaxHeightProperty = DependencyProperty.Register("ImageMaxHeight", typeof(double), typeof(ImageButton), new PropertyMetadata(double.PositiveInfinity));

        static ImageButton() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
        }

        public ImageSource EnabledSource {
            get { return (ImageSource) GetValue(EnabledSourceProperty); }
            set { SetValue(EnabledSourceProperty, value); }
        }

        public ImageSource DisabledSource {
            get { return (ImageSource) GetValue(DisabledSourceProperty); }
            set { SetValue(DisabledSourceProperty, value); }
        }

        public double ImageMaxHeight {
            get { return (double) GetValue(ImageMaxHeightProperty); }
            set { SetValue(ImageMaxHeightProperty, value); }
        }
    }
}

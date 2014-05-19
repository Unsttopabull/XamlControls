using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Frost.XamlControls {
    public class ToggleImageButton : ToggleButton {
        public static readonly DependencyProperty EnabledSourceProperty = DependencyProperty.Register("EnabledSource", typeof(ImageSource), typeof(ToggleImageButton), new PropertyMetadata(default(ImageSource)));
        public static readonly DependencyProperty DisabledSourceProperty = DependencyProperty.Register("DisabledSource", typeof(ImageSource), typeof(ToggleImageButton), new PropertyMetadata(default(ImageSource)));
        public static readonly DependencyProperty CheckedSourceProperty = DependencyProperty.Register("CheckedSource", typeof(ImageSource), typeof(ToggleImageButton), new PropertyMetadata(default(ImageSource)));

        static ToggleImageButton() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToggleImageButton), new FrameworkPropertyMetadata(typeof(ToggleImageButton)));
        }

        public ImageSource EnabledSource {
            get { return (ImageSource) GetValue(EnabledSourceProperty); }
            set { SetValue(EnabledSourceProperty, value); }
        }

        public ImageSource DisabledSource {
            get { return (ImageSource) GetValue(DisabledSourceProperty); }
            set { SetValue(DisabledSourceProperty, value); }
        }

        public ImageSource CheckedSource {
            get { return (ImageSource) GetValue(CheckedSourceProperty); }
            set { SetValue(CheckedSourceProperty, value); }
        }
    }
}

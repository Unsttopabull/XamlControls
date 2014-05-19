using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Frost.XamlControls {
    public class BoxArt : Control {
        public static readonly DependencyProperty BoxImageProperty = DependencyProperty.Register("BoxImage", typeof(ImageSource), typeof(BoxArt), new FrameworkPropertyMetadata(default(ImageSource), FrameworkPropertyMetadataOptions.AffectsRender, BoxImageChanged));
        public static readonly DependencyProperty CoverImageProperty = DependencyProperty.Register("CoverImage", typeof(ImageSource), typeof(BoxArt), new FrameworkPropertyMetadata(default(ImageSource), FrameworkPropertyMetadataOptions.AffectsRender, CoverImageChanged));

        static BoxArt() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BoxArt), new FrameworkPropertyMetadata(typeof(BoxArt)));
        }

        private static void CoverImageChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs) {
            
        }

        private static void BoxImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        }

        public ImageSource BoxImage {
            get { return (ImageSource) GetValue(BoxImageProperty); }
            set { SetValue(BoxImageProperty, value); }
        }

        public ImageSource CoverImage {
            get { return (ImageSource) GetValue(CoverImageProperty); }
            set { SetValue(CoverImageProperty, value); }
        }
    }
}

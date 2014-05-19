using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

[assembly:XmlnsDefinition("http://www.frostmediamanager.com/xaml/controls", "Frost.XamlControls")]
[assembly:XmlnsPrefix("http://www.frostmediamanager.com/xaml/controls", "fmm")]
namespace Frost.XamlControls {

    public class AwardStack : Control {
        public static readonly DependencyProperty WonProperty = DependencyProperty.Register("Won", typeof(int), typeof(AwardStack), new FrameworkPropertyMetadata(default(int), FrameworkPropertyMetadataOptions.AffectsRender, RatingChanged));
        public static readonly DependencyProperty NominatedProperty = DependencyProperty.Register("Nominated", typeof(int), typeof(AwardStack), new FrameworkPropertyMetadata(default(int), FrameworkPropertyMetadataOptions.AffectsRender, RatingChanged));
        public static readonly DependencyProperty WonImageProperty = DependencyProperty.Register("WonImage", typeof(ImageSource), typeof(AwardStack), new FrameworkPropertyMetadata(default(ImageSource), FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty NominatedImageProperty = DependencyProperty.Register("NominatedImage", typeof(ImageSource), typeof(AwardStack), new FrameworkPropertyMetadata(default(ImageSource), FrameworkPropertyMetadataOptions.AffectsRender));
        private readonly StackPanel _stack;

        public AwardStack() {
            _stack = new StackPanel();

            _stack.BeginInit();
            _stack.Orientation = Orientation.Horizontal;
            _stack.Name = "Stack";
            _stack.EndInit();

            AddVisualChild(_stack);
        }

        public int Won {
            get { return (int) GetValue(WonProperty); }
            set { SetValue(WonProperty, value); }
        }

        public ImageSource WonImage {
            get { return (ImageSource) GetValue(WonImageProperty); }
            set { SetValue(WonImageProperty, value); }
        }

        public int Nominated {
            get { return (int) GetValue(NominatedProperty); }
            set { SetValue(NominatedProperty, value); }
        }

        public ImageSource NominatedImage {
            get { return (ImageSource) GetValue(NominatedImageProperty); }
            set { SetValue(NominatedImageProperty, value); }
        }

        private static void RatingChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            ((AwardStack) obj).ChangeRating();
        }

        private void ChangeRating() {
            int won = Won;
            int nominated = Nominated;

            _stack.Children.Clear();

            for (int i = 0; i < won; i++) {
                _stack.Children.Add(new Image{Source = WonImage, Margin = new Thickness(0, 0, 5, 0), MaxWidth = 70});
            }

            for (int i = 0; i < nominated; i++) {
                _stack.Children.Add(new Image{Source = NominatedImage, Margin = new Thickness(0, 0, 5, 0), MaxWidth = 70});
            }
        }
    }

}
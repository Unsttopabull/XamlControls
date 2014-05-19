using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Frost.XamlControls.Commands;

namespace Frost.XamlControls {
    public class StarRating : Control {
        public static readonly DependencyProperty RatingProperty = DependencyProperty.Register("Rating", typeof(double?), typeof(StarRating), new FrameworkPropertyMetadata(default(double?), FrameworkPropertyMetadataOptions.AffectsRender, RatingChanged));

        private const string PACK_URI_FORMAT = "pack://application:,,,/Frost.XamlControls;component/{0}";
        private static readonly BitmapImage StarEmpty;
        private static readonly BitmapImage StarHalf;
        private static readonly BitmapImage StarFull;
        private double _mouseOverRating;
        private bool _templateApplied;
        private Image _star1;
        private Image _star2;
        private Image _star3;
        private Image _star4;
        private Image _star5;

        static StarRating() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StarRating), new FrameworkPropertyMetadata(typeof(StarRating)));

            StarFull = new BitmapImage(new Uri(string.Format(PACK_URI_FORMAT, "Images/Stars/star.png")));
            StarHalf = new BitmapImage(new Uri(string.Format(PACK_URI_FORMAT, "Images/Stars/starhalf.png")));
            StarEmpty = new BitmapImage(new Uri(string.Format(PACK_URI_FORMAT, "Images/Stars/starempty.png")));
        }

        public StarRating() {
            MouseLeaveCommand = new RelayCommand(() => ChangeRating(Rating));
            MouseDownCommand = new RelayCommand(() => Rating = _mouseOverRating);
            MouseMoveCommand = new RelayCommand<MouseEventArgs>(ChangeMouseOverRating);
        }

        public ICommand MouseLeaveCommand { get; private set; }
        public ICommand MouseDownCommand { get; private set; }
        public ICommand MouseMoveCommand { get; private set; }

        /// <summary>When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate"/>.</summary>
        public override void OnApplyTemplate() {
            base.OnApplyTemplate();

            _star1 = (Image) Template.FindName("Star1", this);
            _star2 = (Image) Template.FindName("Star2", this);
            _star3 = (Image) Template.FindName("Star3", this);
            _star4 = (Image) Template.FindName("Star4", this);
            _star5 = (Image) Template.FindName("Star5", this);

            _templateApplied = true;

            ChangeRating(Rating);
        }

        public double? Rating {
            get { return (double?) GetValue(RatingProperty); }
            set { SetValue(RatingProperty, value); }
        }

        private static void RatingChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            StarRating sr = (StarRating) obj;

            if (!sr._templateApplied) {
                return;
            }

            sr.ChangeRating((double?) e.NewValue);
        }

        private void ChangeRating(double? value) {
            if (!value.HasValue) {
                value = 0;
            }
            else {
                value /= 2;
            }

            if (value >= 0 && value < 0.5) {
                _star1.Source = StarEmpty;
                _star2.Source = StarEmpty;
                _star3.Source = StarEmpty;
                _star4.Source = StarEmpty;
                _star5.Source = StarEmpty;
            }

            if (value >= 0.5 && value < 1) {
                _star1.Source = StarHalf;
                _star2.Source = StarEmpty;
                _star3.Source = StarEmpty;
                _star4.Source = StarEmpty;
                _star5.Source = StarEmpty;
            }

            if (value >= 1 && value < 1.5) {
                _star1.Source = StarFull;
                _star2.Source = StarEmpty;
                _star3.Source = StarEmpty;
                _star4.Source = StarEmpty;
                _star5.Source = StarEmpty;
            }

            if (value >= 1.5 && value < 2) {
                _star1.Source = StarFull;
                _star2.Source = StarHalf;
                _star3.Source = StarEmpty;
                _star4.Source = StarEmpty;
                _star5.Source = StarEmpty;
            }

            if (value >= 2 && value < 2.5) {
                _star1.Source = StarFull;
                _star2.Source = StarFull;
                _star3.Source = StarEmpty;
                _star4.Source = StarEmpty;
                _star5.Source = StarEmpty;
            }

            if (value >= 2.5 && value < 3) {
                _star1.Source = StarFull;
                _star2.Source = StarFull;
                _star3.Source = StarHalf;
                _star4.Source = StarEmpty;
                _star5.Source = StarEmpty;
            }

            if (value >= 3 && value < 3.5) {
                _star1.Source = StarFull;
                _star2.Source = StarFull;
                _star3.Source = StarFull;
                _star4.Source = StarEmpty;
                _star5.Source = StarEmpty;
            }

            if (value >= 3.5 && value < 4) {
                _star1.Source = StarFull;
                _star2.Source = StarFull;
                _star3.Source = StarFull;
                _star4.Source = StarHalf;
                _star5.Source = StarEmpty;
            }

            if (value >= 4 && value < 4.5) {
                _star1.Source = StarFull;
                _star2.Source = StarFull;
                _star3.Source = StarFull;
                _star4.Source = StarFull;
                _star5.Source = StarEmpty;
            }

            if (value >= 4.5 && value < 5) {
                _star1.Source = StarFull;
                _star2.Source = StarFull;
                _star3.Source = StarFull;
                _star4.Source = StarFull;
                _star5.Source = StarHalf;
            }

            if (value >= 5) {
                _star1.Source = StarFull;
                _star2.Source = StarFull;
                _star3.Source = StarFull;
                _star4.Source = StarFull;
                _star5.Source = StarFull;
            }
        }

        private void ChangeMouseOverRating(MouseEventArgs e) {
            Image img = e.Source as Image;
            if (img == null || !(img.Tag is int)) {
                return;
            }

            int tag = (int) img.Tag;

            _mouseOverRating = e.GetPosition(img).X > img.ActualWidth / 2.0 ? tag + 1 : tag;
            ChangeRating(_mouseOverRating);
        }

    }
}

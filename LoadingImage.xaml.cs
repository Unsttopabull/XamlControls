using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Frost.XamlControls.Annotations;

namespace Frost.XamlControls {

    /// <summary>Interaction logic for LoadingImage.xaml</summary>
    public partial class LoadingImage : UserControl, INotifyPropertyChanged {
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(string), typeof(LoadingImage), new PropertyMetadata("", OnSourceChanged));
        public static readonly DependencyProperty ImageWidthProperty = DependencyProperty.Register("ImageWidth", typeof(double), typeof(LoadingImage), new PropertyMetadata(double.PositiveInfinity));
        public static readonly DependencyProperty ImageHeightProperty = DependencyProperty.Register("ImageHeight", typeof(double), typeof(LoadingImage), new PropertyMetadata(double.PositiveInfinity));
        public static readonly DependencyProperty ImageMaxWidthProperty = DependencyProperty.Register("ImageMaxWidth", typeof(double), typeof(LoadingImage), new PropertyMetadata(double.PositiveInfinity));
        public static readonly DependencyProperty ImageMaxHeightProperty = DependencyProperty.Register("ImageMaxHeight", typeof(double), typeof(LoadingImage), new PropertyMetadata(double.PositiveInfinity));
        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register("Stretch", typeof(Stretch), typeof(LoadingImage), new PropertyMetadata(default(Stretch)));
        public static readonly DependencyProperty LoadingTextProperty = DependencyProperty.Register("LoadingText", typeof(string), typeof(LoadingImage), new PropertyMetadata("Loading..."));

        public event PropertyChangedEventHandler PropertyChanged;
        private readonly EventHandler _downloadCompleted;
        private readonly EventHandler<ExceptionEventArgs> _imageFailed;
        private BitmapSource _imageSource;
        private BitmapSource _fallbackImage;

        public LoadingImage() {
            InitializeComponent();
            
            _downloadCompleted = DownloadCompleted;
            _imageFailed = ImageFailed;
        }

        public string LoadingText {
            get { return (string) GetValue(LoadingTextProperty); }
            set { SetValue(LoadingTextProperty, value); }
        }

        public BitmapSource ImageSource {
            get { return _imageSource; }
            set {
                if (Equals(value, _imageSource)) {
                    return;
                }

                _imageSource = value;
                OnPropertyChanged();
            }
        }

        public BitmapSource FallbackImage {
            get { return _fallbackImage; }
            set {
                if (Equals(value, _fallbackImage)) {
                    return;
                }
                _fallbackImage = value;
                OnPropertyChanged();
            }
        }

        public string Source {
            get { return (string) GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public double ImageWidth {
            get { return (double) GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }

        public double ImageHeight {
            get { return (double) GetValue(ImageHeightProperty); }
            set { SetValue(ImageHeightProperty, value); }
        }

        public double ImageMaxWidth {
            get { return (double) GetValue(ImageMaxWidthProperty); }
            set { SetValue(ImageMaxWidthProperty, value); }
        }

        public double ImageMaxHeight {
            get { return (double) GetValue(ImageMaxHeightProperty); }
            set { SetValue(ImageMaxHeightProperty, value); }
        }

        public Stretch Stretch {
            get { return (Stretch) GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        private void DownloadCompleted(object sender, EventArgs e) {
            Grid.Children.Remove(Spinner);
        }

        private void ImageFailed(object sender, ExceptionEventArgs e) {
            ImageSource = FallbackImage;

            Grid.Children.Remove(Spinner);
        }

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            LoadingImage li = d as LoadingImage;
            if (li == null) {
                return;
            }

            if (li.Source != null) {
                try {
                    li.ImageSource.DownloadCompleted -= li._downloadCompleted;
                    li.ImageSource.DownloadFailed -= li._imageFailed;
                    li.ImageSource.DecodeFailed -= li._imageFailed;
                }
                catch(Exception) {
                }
            }

            string source = e.NewValue as string;
            if (string.IsNullOrEmpty(source)) {
                li.ImageFailed(null, null);
                return;
            }

            try {
                li.ImageSource = new BitmapImage(new Uri(source));
            }
            catch {
                li.ImageFailed(null, null);
                return;
            }

            li.ImageSource.DownloadCompleted += li._downloadCompleted;
            li.ImageSource.DownloadFailed += li._imageFailed;
            li.ImageSource.DecodeFailed += li._imageFailed;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

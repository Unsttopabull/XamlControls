using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Frost.XamlControls.Commands;

namespace Frost.XamlControls {

    public enum Channel {
        Unknown,
        FrontCenter,
        FrontRight,
        FrontLeft,
        SideRight,
        SideLeft,
        BackRight,
        BackLeft,
        LFE
    }

    public class ChannelPicker : Control {
        public static readonly DependencyProperty ChannelSetupProperty = DependencyProperty.Register("ChannelSetup", typeof(string), typeof(ChannelPicker), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty ChannelLayoutProperty = DependencyProperty.Register("ChannelLayout", typeof(string), typeof(ChannelPicker), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty ChannelPositionsProperty = DependencyProperty.Register("ChannelPositions", typeof(string), typeof(ChannelPicker), new PropertyMetadata(default(string), OnChannelPositionsChanged));
        public static readonly DependencyProperty NumberOfChannelsProperty = DependencyProperty.Register("NumberOfChannels", typeof(int?), typeof(ChannelPicker), new PropertyMetadata(default(int?), OnNumberOfChannelChanged));

        private readonly HashSet<Channel> _front;
        private readonly HashSet<Channel> _side;
        private readonly HashSet<Channel> _back;
        private bool _first = true;
        private bool _templateApplied;

        private ToggleImageButton _frontCenter;
        private ToggleImageButton _frontRight;
        private ToggleImageButton _frontLeft;

        private ToggleImageButton _sideRight;
        private ToggleImageButton _sideLeft;

        private ToggleImageButton _backRight;
        private ToggleImageButton _backLeft;
        private ToggleImageButton _lfe;

        static ChannelPicker() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChannelPicker), new FrameworkPropertyMetadata(typeof(ChannelPicker)));
        }

        public ChannelPicker() {
            _front = new HashSet<Channel>();
            _side = new HashSet<Channel>();
            _back = new HashSet<Channel>();

            ChannelClickedCommand = new RelayCommand<ToggleImageButton>(OnToggleButtonClicked);
        }

        /// <summary>When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate"/>.</summary>
        public override void OnApplyTemplate() {
            base.OnApplyTemplate();

            _frontCenter = (ToggleImageButton) Template.FindName("FrontCenter", this);
            _frontRight = (ToggleImageButton) Template.FindName("FrontRight", this);
            _frontLeft = (ToggleImageButton) Template.FindName("FrontLeft", this);

            _sideRight = (ToggleImageButton) Template.FindName("SideRight", this);
            _sideLeft = (ToggleImageButton) Template.FindName("SideLeft", this);

            _backRight = (ToggleImageButton) Template.FindName("BackRight", this);
            _backLeft = (ToggleImageButton) Template.FindName("BackLeft", this);

            _lfe = (ToggleImageButton) Template.FindName("Woofer", this);

            _templateApplied = true;

            if (string.IsNullOrEmpty(ChannelPositions)) {
                SetFromNumberOfChannels(NumberOfChannels);
            }
            SetFromChannelPositionsString(ChannelPositions);  
        }

        public string ChannelSetup {
            get { return (string) GetValue(ChannelSetupProperty); }
            set { SetValue(ChannelSetupProperty, value); }
        }

        public string ChannelLayout {
            get { return (string) GetValue(ChannelLayoutProperty); }
            set { SetValue(ChannelLayoutProperty, value); }
        }

        public string ChannelPositions {
            get { return (string) GetValue(ChannelPositionsProperty); }
            set { SetValue(ChannelPositionsProperty, value); }
        }

        public int? NumberOfChannels {
            get { return (int?) GetValue(NumberOfChannelsProperty); }
            set { SetValue(NumberOfChannelsProperty, value); }
        }

        public ICommand ChannelClickedCommand { get; private set; }

        private static void OnChannelPositionsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ChannelPicker channelPicker = (ChannelPicker) d;
            if (channelPicker._templateApplied) {
                channelPicker.SetFromChannelPositionsString((string) e.NewValue);
            }
        }

        private static void OnNumberOfChannelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ChannelPicker channelPicker = (ChannelPicker) d;
            if (channelPicker._templateApplied) {
                channelPicker.SetFromNumberOfChannels((int?) e.NewValue);
            }
        }

        private void SetFromNumberOfChannels(int? numberOfChannels) {
            if (!numberOfChannels.HasValue || !string.IsNullOrEmpty(ChannelPositions) || !IsInitialized) {
                return;
            }

            _first = true;

            switch (numberOfChannels) {
                case 2:
                    _front.Add(Channel.FrontLeft);
                    _front.Add(Channel.FrontRight);
                    _frontRight.IsChecked = true;
                    _frontLeft.IsChecked = true;
                    break;
                case 6:
                    _front.Add(Channel.FrontLeft);
                    _front.Add(Channel.FrontRight);
                    _front.Add(Channel.FrontCenter);
                    _frontCenter.IsChecked = true;
                    _frontRight.IsChecked = true;
                    _frontLeft.IsChecked = true;

                    _side.Add(Channel.SideLeft);
                    _side.Add(Channel.SideRight);
                    _sideRight.IsChecked = true;
                    _sideLeft.IsChecked = true;

                    _lfe.IsChecked = true;
                    break;
                case 8:
                    _front.Add(Channel.FrontLeft);
                    _front.Add(Channel.FrontRight);
                    _front.Add(Channel.FrontCenter);
                    _frontCenter.IsChecked = true;
                    _frontRight.IsChecked = true;
                    _frontLeft.IsChecked = true;

                    _side.Add(Channel.SideLeft);
                    _side.Add(Channel.SideRight);
                    _sideRight.IsChecked = true;
                    _sideLeft.IsChecked = true;

                    _back.Add(Channel.BackLeft);
                    _back.Add(Channel.BackRight);
                    _backRight.IsChecked = true;
                    _backLeft.IsChecked = true;

                    _lfe.IsChecked = true;
                    break;
            }

            GetChannelPositions();
            GetChannelSetup();

            ChannelLayout = _lfe.IsChecked == true
                                ? (numberOfChannels - 1) + ".1"
                                : numberOfChannels + ".0";
        }

        private void SetFromChannelPositionsString(string channelPositions) {
            if (string.IsNullOrEmpty(channelPositions) || !_first || !IsInitialized ) {
                return;
            }

            _first = true;

            _lfe.IsChecked = false;
            _frontCenter.IsChecked = false;
            _frontRight.IsChecked = false;
            _frontLeft.IsChecked = false;

            _sideRight.IsChecked = false;
            _sideLeft.IsChecked = false;

            _backRight.IsChecked = false;
            _backLeft.IsChecked = false;

            int numChannels = 0;
            string[] speakerPositions = channelPositions.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string position in speakerPositions) {
                int front = position.IndexOf("Front:", StringComparison.InvariantCultureIgnoreCase);
                if (front != -1) {
                    string[] speakers = position.Remove(front, 6).Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string speaker in speakers) {
                        switch (speaker) {
                            case "C":
                                numChannels++;
                                _front.Add(Channel.FrontCenter);
                                _frontCenter.IsChecked = true;
                                break;
                            case "L":
                                numChannels++;
                                _front.Add(Channel.FrontLeft);
                                _frontLeft.IsChecked = true;
                                break;
                            case "R":
                                numChannels++;
                                _front.Add(Channel.FrontRight);
                                _frontRight.IsChecked = true;
                                break;
                        }
                    }

                    continue;
                }

                int side = position.IndexOf("Side:", StringComparison.InvariantCultureIgnoreCase);
                if (side != -1) {
                    string[] speakers = position.Remove(side, 5).Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string speaker in speakers) {
                        switch (speaker) {
                            case "L":
                                numChannels++;
                                _side.Add(Channel.SideLeft);
                                _sideLeft.IsChecked = true;
                                break;
                            case "R":
                                numChannels++;
                                _side.Add(Channel.SideRight);
                                _sideRight.IsChecked = true;
                                break;
                        }
                    }
                    continue;
                }

                int back = position.IndexOf("Back:", StringComparison.InvariantCultureIgnoreCase);
                if (back != -1) {
                    string[] speakers = position.Remove(back, 5).Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string speaker in speakers) {
                        switch (speaker) {
                            case "L":
                                numChannels++;
                                _back.Add(Channel.BackLeft);
                                _backLeft.IsChecked = true;
                                break;
                            case "R":
                                numChannels++;
                                _back.Add(Channel.BackRight);
                                _backRight.IsChecked = true;
                                break;
                        }
                    }
                    continue;
                }

                if (position.Equals("LFE", StringComparison.InvariantCultureIgnoreCase)) {
                    _lfe.IsChecked = true;
                    numChannels++;
                }
            }

            NumberOfChannels = numChannels;
            GetChannelSetup();
            ChannelLayout = _lfe.IsChecked == true
                                ? (NumberOfChannels - 1) + ".1"
                                : NumberOfChannels + ".0";
        }

        private void OnToggleButtonClicked(ToggleImageButton source) {
            if (source == null) {
                throw new ArgumentNullException("source");
            }

            Channel channels = (Channel) source.Tag;
            if (channels == Channel.Unknown) {
                return;
            }

            if (source.IsChecked == true) {
                AddRemove(false, channels);
                NumberOfChannels++;
            }
            else {
                AddRemove(true, channels);
                NumberOfChannels--;
            }

            GetChannelPositions();
            GetChannelSetup();

            ChannelLayout = _lfe.IsChecked == true
                                ? (NumberOfChannels == 0 ? NumberOfChannels + ".1" : (NumberOfChannels - 1) + ".1")
                                : NumberOfChannels + ".0";
        }

        private void GetChannelSetup() {
            StringBuilder sb = new StringBuilder();
            if (_front.Count > 0) {
                sb.Append(_front.Count);
            }

            if (_side.Count > 0) {
                sb.Append("/" + _side.Count);
            }

            if (_back.Count > 0) {
                sb.Append("/" + _back.Count);
            }
            ChannelSetup = sb.ToString();
        }

        private void GetChannelPositions() {
            StringBuilder sb = new StringBuilder();
            if (_front.Count > 0) {
                sb.Append("Front:");

                foreach (Channel channel in _front) {
                    switch (channel) {
                        case Channel.FrontCenter:
                            sb.Append(" C");
                            break;
                        case Channel.FrontRight:
                            sb.Append(" R");
                            break;
                        case Channel.FrontLeft:
                            sb.Append(" L");
                            break;
                    }
                }
            }

            if (_side.Count > 0) {
                sb.Append(", Side:");

                foreach (Channel channel in _side) {
                    switch (channel) {
                        case Channel.SideRight:
                            sb.Append(" R");
                            break;
                        case Channel.SideLeft:
                            sb.Append(" L");
                            break;
                    }
                }
            }

            if (_back.Count > 0) {
                sb.Append(", Back:");

                foreach (Channel channel in _back) {
                    switch (channel) {
                        case Channel.BackRight:
                            sb.Append(" R");
                            break;
                        case Channel.BackLeft:
                            sb.Append(" L");
                            break;
                    }
                }
            }

            if (_lfe.IsChecked == true) {
                sb.Append(", LFE");
            }
            ChannelPositions = sb.ToString();
        }

        private bool AddRemove(bool remove, Channel channel) {
            if (channel != Channel.LFE) {
                HashSet<Channel> hs = null;
                switch (channel) {
                    case Channel.FrontLeft:
                    case Channel.FrontCenter:
                    case Channel.FrontRight:
                        hs = _front;
                        break;
                    case Channel.SideLeft:
                    case Channel.SideRight:
                        hs = _side;
                        break;
                    case Channel.BackLeft:
                    case Channel.BackRight:
                        hs = _back;
                        break;
                }

                if (hs == null) {
                    return false;
                }

                return remove ? hs.Remove(channel) : hs.Add(channel);
            }
            return true;
        }
    }

}
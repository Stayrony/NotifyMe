using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotifyMe.Controls
{
        public class NotifyMeButton : ContentView
        {
            private uint _animateDuration = 200;

            private Frame _mainFrame;
            private TapGestureRecognizer _tapNotifyMe;
            private Label _buttonName;
            private Frame _sendFrame;
            private Frame _sendFrameDisable;
            private ExtendedEntry _emailEntry;

            public NotifyMeButton()
            {
                this.BuildContent();
            }

            #region -- Public properties --

            public static readonly BindableProperty SendCommandProperty =
                BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(NotifyMeButton), default(ICommand));

            public ICommand SendCommand
            {
                get { return (ICommand)GetValue(SendCommandProperty); }
                set { SetValue(SendCommandProperty, value); }
            }

            public static readonly BindableProperty SendCommandParameterProperty =
                       BindableProperty.Create(nameof(SendCommandParameter), typeof(object), typeof(NotifyMeButton), default(object));

            public object SendCommandParameter
            {
                get { return (object)GetValue(SendCommandParameterProperty); }
                set { SetValue(SendCommandParameterProperty, value); }
            }

            public static readonly BindableProperty EmailProperty = BindableProperty.Create(nameof(Email), typeof(string), typeof(NotifyMeButton), default(string));
            public string Email
            {
                get { return (string)GetValue(EmailProperty); }
                set { SetValue(EmailProperty, value); }
            }

            private ICommand _notifyMeCommand;
            public ICommand NotifyMeCommand
            {
                get { return _notifyMeCommand ?? (_notifyMeCommand = new Command(async () => await OnNotifyMeCommandAsync())); }
            }

            private ICommand _internalSendCommand;
            public ICommand InternalSendCommand
            {
                get { return _internalSendCommand ?? (_internalSendCommand = new Command(async () => await OnInternalSendCommandCommandAsync())); }
            }

            #endregion

            #region -- Overrides properties --

            protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                base.OnPropertyChanged(propertyName);
                if (propertyName == nameof(Email))
                {
                    if (string.IsNullOrEmpty(Email) || !IsValidEmail(Email))
                    {
                        _sendFrameDisable.IsVisible = true;
                    }
                    else
                    {
                        _sendFrameDisable.IsVisible = false;
                    }
                }
            }

            #endregion

            #region -- Private helpers --

            private void BuildContent()
            {
                _buttonName = new Label
                {
                    Text = "Notify me",
                    FontAttributes = FontAttributes.Bold,
                    FontSize = (Double)Application.Current.Resources["tsize_i1"],
                    TextColor = (Color)Application.Current.Resources["tcolor_i1"],
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand
                };

                _mainFrame = new Frame
                {
                    HasShadow = false,
                    CornerRadius = 35,
                    BackgroundColor = Color.White,
                    Content = _buttonName,
                    HeightRequest = 30,
                    WidthRequest = 150,
                    Padding = 20,
                };

                _tapNotifyMe = new TapGestureRecognizer();
                _tapNotifyMe.BindingContext = this;
                _tapNotifyMe.SetBinding(TapGestureRecognizer.CommandProperty, nameof(NotifyMeCommand));

                _mainFrame.GestureRecognizers.Add(_tapNotifyMe);

                Content = _mainFrame;
            }

            private async Task OnNotifyMeCommandAsync()
            {
                var animate = new Animation(d => this.WidthRequest = d, this.Width, this.Width + 200, Easing.SpringIn);
                animate.Commit(this, "ButtonName", 16, _animateDuration);

                var task1 = _mainFrame.Content.ScaleTo(0, _animateDuration);
                var task2 = _mainFrame.Content.FadeTo(0, _animateDuration);
                await Task.WhenAll(task1, task2);

                var sendButtonName = new Label
                {
                    Text = "Send",
                    FontAttributes = FontAttributes.Bold,
                    FontSize = (Double)Application.Current.Resources["tsize_i1"],
                    TextColor = Color.White,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand
                };

                _sendFrameDisable = new Frame
                {
                    Content = sendButtonName,
                    HasShadow = false,
                    HeightRequest = 24,
                    WidthRequest = 70,
                    CornerRadius = 30,
                    BackgroundColor = (Color)Application.Current.Resources["cbg_i1"],
                    Opacity = 0,
                    Scale = 0,
                };

                _sendFrame = new Frame
                {
                    Content = sendButtonName,
                    HasShadow = false,
                    HeightRequest = 24,
                    WidthRequest = 70,
                    CornerRadius = 30,
                    BackgroundColor = (Color)Application.Current.Resources["cbg_i1"],
                    Padding = 10,
                    Scale = 0,
                    Opacity = 0,
                };

                var tap = new TapGestureRecognizer();
                tap.BindingContext = this;
                tap.SetBinding(TapGestureRecognizer.CommandProperty, nameof(InternalSendCommand));
                _sendFrame.GestureRecognizers.Add(tap);

                _emailEntry = new ExtendedEntry
                {
                    Placeholder = "E-mail",
                    FontAttributes = FontAttributes.Bold,
                    PlaceholderColor = (Color)Application.Current.Resources["tcolor_i3"],
                    TextColor = (Color)Application.Current.Resources["tcolor_i1"],
                    FontSize = (Double)Application.Current.Resources["tsize_i1"],
                    Keyboard = Keyboard.Email,
                    Scale = 0,
                    Opacity = 0,
                    BindingContext = this
                };
                _emailEntry.SetBinding(Entry.TextProperty, nameof(Email));

                var grid = new Grid()
                {
                    ColumnSpacing = 5,
                    HeightRequest = 24,
                    Margin = new Thickness(-5, -15, -15, -15)
                };

                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.Children.Add(_emailEntry, 0, 0);
                grid.Children.Add(_sendFrame, 1, 0);
                grid.Children.Add(_sendFrameDisable, 1, 0);

                _mainFrame.Content = grid;

                var task3 = _emailEntry.FadeTo(1, _animateDuration);
                var task4 = _emailEntry.ScaleTo(1, _animateDuration);
                var task6 = _sendFrame.FadeTo(1, _animateDuration);
                var task7 = _sendFrame.ScaleTo(1, _animateDuration);
                var task5 = _sendFrameDisable.FadeTo(0.5, _animateDuration);
                var task8 = _sendFrameDisable.ScaleTo(1, _animateDuration);

                await Task.WhenAll(task3, task4, task5, task6, task7, task8);

                _mainFrame.GestureRecognizers.Remove(_tapNotifyMe);
            }

            private async Task OnInternalSendCommandCommandAsync()
            {
                if (SendCommand != null && IsEnabled && SendCommand.CanExecute(SendCommandParameter))
                {
                    SendCommand?.Execute(SendCommandParameter);
                }

                var task0 = _emailEntry.FadeTo(0, _animateDuration);
                var task1 = _emailEntry.ScaleTo(0, _animateDuration);
                var task2 = _sendFrame.FadeTo(0, _animateDuration);
                var task3 = _sendFrame.ScaleTo(0, _animateDuration);
                await Task.WhenAll(task0, task1, task2, task3);

                var animate = new Animation(d => this.WidthRequest = d, this.Width, this.Width - 200, Easing.SpringOut);
                animate.Commit(this, "ButtonName", 16, _animateDuration);

                _buttonName.Text = "Thank you!";
                _emailEntry.Text = string.Empty;

                _mainFrame.Content = _buttonName;

                var task4 = _mainFrame.Content.FadeTo(1, _animateDuration);
                var task5 = _mainFrame.Content.ScaleTo(1, _animateDuration);
                await Task.WhenAll(task4, task5);

                await Task.Delay(1000);

                var task6 = _mainFrame.Content.ScaleTo(0, _animateDuration);
                var task7 = _mainFrame.Content.FadeTo(0, _animateDuration);
                await Task.WhenAll(task6, task7);

                _buttonName.Text = "Notify me";

                var task8 = _mainFrame.Content.FadeTo(1, _animateDuration);
                var task9 = _mainFrame.Content.ScaleTo(1, _animateDuration);
                await Task.WhenAll(task8, task9);

                _mainFrame.GestureRecognizers.Add(_tapNotifyMe);
            }

            private bool IsValidEmail(string email)
            {
                try
                {
                    var addr = new System.Net.Mail.MailAddress(email);
                    return addr.Address == email;
                }
                catch
                {
                    return false;
                }
            }
            #endregion
        }
}

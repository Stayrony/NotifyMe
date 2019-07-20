using System;
using Xamarin.Forms;

namespace NotifyMe.Controls
{
    public class ExtendedEntry : Entry
    {
        #region -- Public properties --

        public static readonly BindableProperty HasBorderProperty =
            BindableProperty.Create(nameof(HasBorder), typeof(bool), typeof(ExtendedEntry), false);

        public bool HasBorder
        {
            get { return (bool)GetValue(HasBorderProperty); }
            set { SetValue(HasBorderProperty, value); }
        }

        #endregion
    }
}

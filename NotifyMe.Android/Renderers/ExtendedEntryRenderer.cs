using System;
using Android.Content;
using NotifyMe.Controls;
using NotifyMe.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtendedEntry), typeof(ExtendedEntryRenderer))]
namespace NotifyMe.Droid.Renderers
{
    public class ExtendedEntryRenderer : EntryRenderer
    {
        public ExtendedEntryRenderer(Context context)
            : base(context)
        {
        }

        #region -- Overrides --

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                return;
            }

            var view = (ExtendedEntry)Element;

            if (!view.HasBorder)
            {
                SetBorder(view);
            }

            Control.SetPadding(20, 0, 0, 0);
            this.Control.Hint = view.Placeholder;
            this.Control.SetHintTextColor(view.PlaceholderColor.ToAndroid()); // Placeholder Color
            Control.TextAlignment = Android.Views.TextAlignment.Center;
            Control.Gravity = Android.Views.GravityFlags.CenterVertical;
        }

        #endregion

        #region -- Private helpers --

        private void SetBorder(ExtendedEntry view)
        {
            if (Control != null)
            {
                Control.SetBackgroundColor(Color.Transparent.ToAndroid());
            }
        }

        #endregion
    }
}

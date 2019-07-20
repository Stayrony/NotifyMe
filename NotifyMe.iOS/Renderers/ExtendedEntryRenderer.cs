using System;
using NotifyMe.Controls;
using NotifyMe.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedEntry), typeof(ExtendedEntryRenderer))]
namespace NotifyMe.iOS.Renderers
{
    public class ExtendedEntryRenderer : EntryRenderer
    {
        #region -- Overrides --

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            var view = (ExtendedEntry)Element;

            if (view == null)
            {
                return;
            }

            Control.BorderStyle = view.HasBorder ? UITextBorderStyle.RoundedRect : UITextBorderStyle.None;
            Control.TintColor = ((Color)Xamarin.Forms.Application.Current.Resources["cbg_i1"]).ToUIColor();
        }

        #endregion
    }
}

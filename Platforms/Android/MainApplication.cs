﻿using Android.App;
using Android.Runtime;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;

namespace TesisAppSINMVVM
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        //protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
        protected override MauiApp CreateMauiApp()
        {
            // Remove Entry control underline       **** (CONTROL TRANSPARENTE) ****
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoUnderline", (h, v) =>
            {
                h.PlatformView.BackgroundTintList =
                    Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToAndroid());
            });
            //Remove Picker control underline       ****(CONTROL TRANSPARENTE) ****
            Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping("PickerCustomization", (handler, view) =>
            {
                handler.PlatformView.BackgroundTintList =
                Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToAndroid());
            });
            Microsoft.Maui.Handlers.DatePickerHandler.Mapper.AppendToMapping("DatePickerCustomization", (handler, view) =>
            {
                handler.PlatformView.BackgroundTintList =
                Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToAndroid());
            });

            return MauiProgram.CreateMauiApp();
        }
    }
}

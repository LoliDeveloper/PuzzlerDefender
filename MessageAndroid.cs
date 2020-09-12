
using Android.App;
using Android.Widget;

namespace PuzzlerDefender
{
    static class MessageAndroid
    {
        public static void LongAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }

        public static void ShortAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }

    }
}
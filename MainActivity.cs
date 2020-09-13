using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Widget;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace PuzzlerDefender
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        static PersonData personData;

        ImageView hpBarGreenMain;
        ImageView hpBarRedMain;

        Button startButton;
        Intent intent;
        TextView hpBarText;
        RelativeLayout relLayHpBar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            if (Build.VERSION.SdkInt < BuildVersionCodes.JellyBean)
            {
                Window.SetFlags(Android.Views.WindowManagerFlags.Fullscreen,
                                Android.Views.WindowManagerFlags.Fullscreen);
            }
            RequestWindowFeature(Android.Views.WindowFeatures.NoTitle);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            startButton = (Button)FindViewById(Resource.Id.startButton);
            hpBarText = (TextView)FindViewById(Resource.Id.hpBarText);
            hpBarGreenMain = (ImageView)FindViewById(Resource.Id.hpBarGreenMain);
            hpBarRedMain = (ImageView)FindViewById(Resource.Id.hpBarRedMain);
            relLayHpBar = (RelativeLayout)FindViewById(Resource.Id.relLayHpBar);
            hpBarGreenMain.LayoutParameters.Width = 124;
        }
        protected override void OnStart()
        {
            base.OnStart();
            startButton.Click += StartButton_Click;
            intent = new Intent(this, typeof(LevelActivity));
        }
        protected override void OnResume()
        {
            base.OnResume();
            MessageAndroid.ShortAlert("OnResume MainActivity");
            GetPersonDataAsync();
        }

        private async void GetPersonDataAsync()
        {
            await Task.Run(() => GetPersonData());
            MessageAndroid.ShortAlert(personData.HPDino.ToString());
            MessageAndroid.ShortAlert(relLayHpBar.LayoutParameters.Width.ToString());
            hpBarGreenMain.LayoutParameters.Width = (personData.HPDino * relLayHpBar.Width) / 100;
            MessageAndroid.ShortAlert(hpBarGreenMain.LayoutParameters.Width.ToString());

            hpBarText.Text = $"{personData.HPDino}/100 HP";
        }

        private void GetPersonData()
        {
            string jsonString;
            var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "data.json");
            #region ifNotExist
            if (!File.Exists(backingFile))
            {
                string serializeString;
                serializeString = JsonConvert.SerializeObject(
                    new PersonData(40, 10)
                    {
                        EasySecRecord = 0,
                        HardSecRecord = 0,
                        HPDino = 100,
                        MediumSecRecord = 0,
                        Coins = 0,
                        StatusData = "Okay"
                    });
                using (var writer = File.CreateText(backingFile))
                {
                    writer.WriteLine(serializeString);
                }
            }
            #endregion
            using (var reader = File.ReadAllTextAsync(backingFile))
            {
                jsonString = reader.Result;
            }
            personData = JsonConvert.DeserializeObject<PersonData>(jsonString);
        }

        private void StartButton_Click(object sender, System.EventArgs e)
        {
            StartActivity(intent);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public override void OnBackPressed()
        {
            base.OnBackPressed();
        }
    }
}
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using PuzzlerDefender.Enums;
using System;
using System.IO;

namespace PuzzlerDefender
{
    [Activity(Label = "LevelActivity", LaunchMode = default)]
    public class LevelActivity : Activity, View.IOnClickListener
    {
        public static event Action backPressed;

        TextView hpBarText2;
        TextView brainPowerLevAct;

        ImageView hpBarGreen;
        ImageView hpBarRed;

        Button backButtonLevels;
        Button easyButton;
        Button mediumButton;
        Button hardButton;

        PersonData personData;

        string easyDif = JsonConvert.SerializeObject(TypeDiff.Easy);
        string mediumDif = JsonConvert.SerializeObject(TypeDiff.Medum);
        string hardDif = JsonConvert.SerializeObject(TypeDiff.Hard);

        public void OnClick(View v)
        {
            Intent intent;
            switch (v.Id)
            {
                case Resource.Id.easyButton:
                    intent = new Intent(this, typeof(GameActivity));
                    intent.PutExtra("TypeDiff", easyDif);
                    StartActivity(intent);
                    break;
                case Resource.Id.mediumButton:
                    intent = new Intent(this, typeof(GameActivity));
                    intent.PutExtra("TypeDiff", mediumDif);
                    StartActivity(intent);
                    break;
                case Resource.Id.hardButton:
                    intent = new Intent(this, typeof(GameActivity));
                    intent.PutExtra("TypeDiff", hardDif);
                    StartActivity(intent);
                    break;
                default: throw new Exception("SomeThing wrong...");
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            if (Build.VERSION.SdkInt < BuildVersionCodes.JellyBean)
            {
                Window.SetFlags(Android.Views.WindowManagerFlags.Fullscreen,
                                Android.Views.WindowManagerFlags.Fullscreen);
            }
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.levels);

            hpBarText2 = (TextView)FindViewById(Resource.Id.hpBarText2);
            brainPowerLevAct = (TextView)FindViewById(Resource.Id.brainPowerLevAct);

            hpBarGreen = (ImageView)FindViewById(Resource.Id.hpBarGreen);
            hpBarRed = (ImageView)FindViewById(Resource.Id.hpBarRed);

            backButtonLevels = (Button)FindViewById(Resource.Id.backButtonLevels);
            easyButton = (Button)FindViewById(Resource.Id.easyButton);
            easyButton.SetOnClickListener(this);
            mediumButton = (Button)FindViewById(Resource.Id.mediumButton);
            mediumButton.SetOnClickListener(this);
            hardButton = (Button)FindViewById(Resource.Id.hardButton);
            hardButton.SetOnClickListener(this);
            backButtonLevels.Click += BackButtonLevels_Click;
            GameActivity.backPressed += GameActivity_backPressed;
            GetPersonDataAsync();
        }

        private void GameActivity_backPressed()
        {
            GetPersonDataAsync();
        }

        private void BackButtonLevels_Click(object sender, EventArgs e)
        {
            OnBackPressed();
        }
        public override void OnBackPressed()
        {
            backPressed.Invoke();
            base.OnBackPressed();

        }

        private async void GetPersonDataAsync()
        {
            string jsonString;
            var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "data.json");

            using (var reader = File.ReadAllTextAsync(backingFile))
            {
                jsonString = await reader;
            }
            personData = JsonConvert.DeserializeObject<PersonData>(jsonString);
            hpBarGreen.LayoutParameters.Width = (personData.HPDino * hpBarRed.LayoutParameters.Width) / 100;
            hpBarText2.Text = $"{personData.HPDino}/100 HP";
            easyButton.Text = personData.Coins > 0 ? $"-{personData.Coins * 1} HP" : "-1 HP";
            mediumButton.Text = personData.Coins > 0 ? $"-{personData.Coins * 2} HP" : "-2 HP";
            hardButton.Text = personData.Coins > 0 ? $"-{personData.Coins * 3} HP" : "-3 HP";
            brainPowerLevAct.Text = $"Brain Power: {personData.Coins}";
        }
    }
}
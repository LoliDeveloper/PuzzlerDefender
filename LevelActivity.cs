using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using PuzzlerDefender.Enums;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PuzzlerDefender
{
    [Activity(Label = "LevelActivity", LaunchMode = default)]
    public class LevelActivity : Activity, View.IOnClickListener
    {
        TextView hpBarText2;
        TextView brainPowerLevAct;

        ImageView hpBarGreen;
        ImageView hpBarRed;

        Button backButtonLevels;
        Button easyButton;
        Button mediumButton;
        Button hardButton;

        Intent intent;
        PersonData personData;

        string easyDif = JsonConvert.SerializeObject(TypeDiff.Easy);
        string mediumDif = JsonConvert.SerializeObject(TypeDiff.Medum);
        string hardDif = JsonConvert.SerializeObject(TypeDiff.Hard);

        int hpBarRedWidth;

        public void OnClick(View v)
        {
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
                case Resource.Id.backButtonLevels:
                    this.OnBackPressed();
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
            mediumButton = (Button)FindViewById(Resource.Id.mediumButton);
            hardButton = (Button)FindViewById(Resource.Id.hardButton);

            hpBarRedWidth = hpBarRed.LayoutParameters.Width;
            //easyButton.SetOnClickListener(this);
            //mediumButton.SetOnClickListener(this);
            //hardButton.SetOnClickListener(this);
            backButtonLevels.SetOnClickListener(this);

            easyButton.Touch += (s, e) =>
            {
                if (e.Event.Action == MotionEventActions.Down) 
                { 
                    easyButton.SetBackgroundResource(Resource.Drawable.darkGreenBg);
                }
                if (e.Event.Action == MotionEventActions.Up)
                {
                    easyButton.SetBackgroundResource(Resource.Drawable.damage1GreenButton);
                    OnClick(easyButton);
                }
            };
            mediumButton.Touch += (s, e) =>
              {
                  if (e.Event.Action == MotionEventActions.Down)
                  {
                      mediumButton.SetBackgroundResource(Resource.Drawable.darkYellowBg);
                  }
                  if (e.Event.Action == MotionEventActions.Up)
                  {
                      mediumButton.SetBackgroundResource(Resource.Drawable.damage2OrangeButton);
                      OnClick(mediumButton);
                  }
              };
            hardButton.Touch += (s, e) =>
              {
                  if (e.Event.Action == MotionEventActions.Down)
                  {
                      hardButton.SetBackgroundResource(Resource.Drawable.darkRedBg);
                  }
                  if (e.Event.Action == MotionEventActions.Up)
                  {
                      hardButton.SetBackgroundResource(Resource.Drawable.damage3RedButton); 
                      OnClick(hardButton);
                  }
              };

        }
        protected override void OnStart()
        {
            base.OnStart();
        }
        protected override void OnResume()
        {
            base.OnResume();
            GetPersonDataAsync();
        }

        private async void GetPersonDataAsync()
        {
            await Task.Run(() => GetPersonData());
            hpBarGreen.LayoutParameters.Width = (personData.HPDino * hpBarRedWidth) / 100;
            hpBarGreen.RequestLayout();
            hpBarText2.Text = $"{personData.HPDino}/100 HP";
            easyButton.Text = personData.Coins > 0 ? $"-{personData.Coins * 1} HP" : "-1 HP";
            mediumButton.Text = personData.Coins > 0 ? $"-{personData.Coins * 2} HP" : "-2 HP";
            hardButton.Text = personData.Coins > 0 ? $"-{personData.Coins * 3} HP" : "-3 HP";
            brainPowerLevAct.Text = $"Brain Power: {personData.Coins}";
        }

        private async void GetPersonData()
        {
            string jsonString;
            var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "data.json");

            using (var reader = File.ReadAllTextAsync(backingFile))
            {
                jsonString = await reader;
            }
            personData = JsonConvert.DeserializeObject<PersonData>(jsonString);
        }
    }
}
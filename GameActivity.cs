using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using PuzzlerDefender.Enums;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PuzzlerDefender
{
    [Activity(Label = "GameActivity", LaunchMode = default)]
    public class GameActivity : Activity, View.IOnClickListener
    {
        Button backButtonGameMain;
        Button one;
        Button two;
        Button three;
        Button four;
        Button five;
        Button six;
        Button seven;
        Button eight;

        TextView emptyTextView;
        TextView diffText;

        Button[] arrNums = new Button[8];

        ViewGroup.LayoutParams[] arrRightsLayouts = new ViewGroup.LayoutParams[9];

        ProgressBar determinateBar;

        int rightColor = 0;
        TypeDiff typeDiff;
        int moveCounter;
        int sixCounter = 0;

        PersonData personData;
        string backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "data.json");

        Button loadBarButton; // TEST LOADBAR

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.gameMain);
            one = (Button)FindViewById(Resource.Id.one);
            two = (Button)FindViewById(Resource.Id.two);
            three = (Button)FindViewById(Resource.Id.three);
            four = (Button)FindViewById(Resource.Id.four);
            five = (Button)FindViewById(Resource.Id.five);
            six = (Button)FindViewById(Resource.Id.six);
            seven = (Button)FindViewById(Resource.Id.seven);
            eight = (Button)FindViewById(Resource.Id.eight);
            emptyTextView = (TextView)FindViewById(Resource.Id.emptyTextView);
            diffText = (TextView)FindViewById(Resource.Id.diffText);
            backButtonGameMain = (Button)FindViewById(Resource.Id.backButtonGameMain);
            determinateBar = (ProgressBar)FindViewById(Resource.Id.determinateBar);
        }
        protected override void OnStart()
        {
            base.OnStart();

            typeDiff = JsonConvert.DeserializeObject<TypeDiff>(Intent.GetStringExtra("TypeDiff"));
            if (typeDiff > 0)
            {
                MessageAndroid.ShortAlert(typeDiff.ToString());
            }
            arrNums[0] = one;
            arrRightsLayouts[0] = one.LayoutParameters;
            one.SetOnClickListener(this);
            arrNums[1] = two;
            arrRightsLayouts[1] = two.LayoutParameters;
            two.SetOnClickListener(this);
            arrNums[2] = three;
            arrRightsLayouts[2] = three.LayoutParameters;
            three.SetOnClickListener(this);
            arrNums[3] = four;
            arrRightsLayouts[3] = four.LayoutParameters;
            four.SetOnClickListener(this);
            arrNums[4] = five;
            arrRightsLayouts[4] = five.LayoutParameters;
            five.SetOnClickListener(this);
            arrNums[5] = six;
            arrRightsLayouts[5] = six.LayoutParameters;
            six.SetOnClickListener(this);
            arrNums[6] = seven;
            arrRightsLayouts[6] = seven.LayoutParameters;
            seven.SetOnClickListener(this);
            arrNums[7] = eight;
            arrRightsLayouts[7] = eight.LayoutParameters;
            eight.SetOnClickListener(this);
            arrRightsLayouts[8] = emptyTextView.LayoutParameters;
            emptyTextView.SetOnClickListener(this);
            diffText.Text = typeDiff.ToString();
            backButtonGameMain.Click += BackButtonGameMain_Click;

            determinateBar.Progress = 0;
        }

        protected override void OnResume()
        {
            base.OnResume();
            GetPersonDataAsync();
            randomizeButton();
        }

        private void BackButtonGameMain_Click(object sender, EventArgs e)
        {
            OnBackPressed();
        }
        private async void IncreaseBarAsync(int toThat)
        {
            await Task.Run(() => IncreaseBar(toThat));
        }
        private void IncreaseBar(int toThat)
        {
            while (determinateBar.Progress < toThat)
            {
                determinateBar.Progress += 1;
                Thread.Sleep(5);
            }
        }

        public void OnClick(View v)
        {
            if (v.Id == Resource.Id.six)
            {
                sixCounter++;
                if (sixCounter == 6)
                {
                    randomizeButton();
                }
            }
            else
            {
                sixCounter = 0;
            }
            if (v.Id != Resource.Id.emptyTextView)
            {
                if ((Math.Abs(FindViewById(v.Id).Left - emptyTextView.Left) == 0
                && Math.Abs(FindViewById(v.Id).Top - emptyTextView.Top) == 190)
                ||
                (Math.Abs(FindViewById(v.Id).Top - emptyTextView.Top) == 0
                && Math.Abs(FindViewById(v.Id).Left - emptyTextView.Left) == 190))
                {
                    ViewGroup.LayoutParams ll = emptyTextView.LayoutParameters;
                    emptyTextView.LayoutParameters = FindViewById(v.Id).LayoutParameters;
                    FindViewById(v.Id).LayoutParameters = ll;
                    moveCounter++;

                    UpdateColorButton();
                }
            }
        }
        private void randomizeButton()
        {
            determinateBar.Progress = 0;
            sixCounter = 0;
            moveCounter = 0;
            int rndNum;
            int hard = 8;
            Random random = new Random();
            IncreaseBarAsync(10);
            if (typeDiff == TypeDiff.Easy)
            {
                hard = 1;
            }
            else if (typeDiff == TypeDiff.Medum)
            {
                hard = 3;
            }
            else if (typeDiff == TypeDiff.Hard)
            {
                hard = 7;
            }
            else
            {
                MessageAndroid.ShortAlert("Something Wrong");
                OnBackPressed();
            }

            IncreaseBarAsync(20);
            ViewGroup.LayoutParams ll;
            for (int i = 0; i < hard * 20;)
            {
                if (i == hard * 10)
                {
                    IncreaseBarAsync(80);
                }

                rndNum = random.Next(0, 8);

                if (emptyTextView.LayoutParameters == arrRightsLayouts[0] && ((rndNum == 3) || (rndNum == 1))
                    || emptyTextView.LayoutParameters == arrRightsLayouts[1] && ((rndNum == 0) || (rndNum == 4) || (rndNum == 2))
                    || emptyTextView.LayoutParameters == arrRightsLayouts[2] && ((rndNum == 1) || (rndNum == 5))
                    || emptyTextView.LayoutParameters == arrRightsLayouts[3] && ((rndNum == 0) || (rndNum == 4) || (rndNum == 6))
                    || emptyTextView.LayoutParameters == arrRightsLayouts[4] && ((rndNum == 1) || (rndNum == 3) || (rndNum == 5) || (rndNum == 7))
                    || emptyTextView.LayoutParameters == arrRightsLayouts[5] && ((rndNum == 2) || (rndNum == 4) || (rndNum == 8))
                    || emptyTextView.LayoutParameters == arrRightsLayouts[6] && ((rndNum == 3) || (rndNum == 7))
                    || emptyTextView.LayoutParameters == arrRightsLayouts[7] && ((rndNum == 6) || (rndNum == 4) || (rndNum == 8))
                    || emptyTextView.LayoutParameters == arrRightsLayouts[8] && ((rndNum == 5) || (rndNum == 7)))
                {
                    foreach (Button item in arrNums)
                    {
                        if (item.LayoutParameters == arrRightsLayouts[rndNum])
                        {
                            ll = emptyTextView.LayoutParameters;
                            emptyTextView.LayoutParameters = item.LayoutParameters;
                            item.LayoutParameters = ll;
                            break;
                        }
                    }
                    i++;
                }
            }
            IncreaseBarAsync(80);
            UpdateColorButton();
            IncreaseBarAsync(100);
        }
        private void UpdateColorButton()
        {
            rightColor = 0;
            for (int i = 0; i < arrNums.Length; i++)
            {
                if (arrNums[i].LayoutParameters == arrRightsLayouts[i])
                {
                    arrNums[i].SetBackgroundResource(Resource.Color.green);
                    rightColor++;
                }
                else
                {
                    arrNums[i].SetBackgroundResource(Resource.Color.white);
                }
            }
            if (rightColor == 8)
            {
                MessageAndroid.ShortAlert($"Congratulations! You Win at {moveCounter} moves!\n You Damage is {typeDiff}");
                HitDamage(typeDiff);
            }
        }

        private async void HitDamage(TypeDiff td)
        {
            personData.HPDino -= personData.Coins > 0 ? (int)td * personData.Coins : (int)td;
            if (personData.HPDino < 1)
            {
                personData.HPDino = 100;
                personData.Coins++;
            }
            switch (td)
            {
                case TypeDiff.Easy:
                    if (moveCounter < personData.EasySecRecord)
                    {
                        personData.EasySecRecord = moveCounter;
                        MessageAndroid.ShortAlert("New Record!");
                    }
                    break;
                case TypeDiff.Medum:
                    if (moveCounter < personData.MediumSecRecord)
                    {
                        personData.MediumSecRecord = moveCounter;
                        MessageAndroid.ShortAlert("New Record!");
                    }
                    break;
                case TypeDiff.Hard:
                    if (moveCounter < personData.HardSecRecord)
                    {
                        personData.HardSecRecord = moveCounter;
                        MessageAndroid.ShortAlert("New Record!");
                    }
                    break;
            }
            string serializeString;
            serializeString = JsonConvert.SerializeObject(personData);
            using (var writer = File.CreateText(backingFile))
            {
                await writer.WriteLineAsync(serializeString); //Async
            }
            randomizeButton();
        }

        private async void GetPersonDataAsync()
        {
            await Task.Run(() => GetPersonData());
        }
        private void GetPersonData()
        {
            IncreaseBarAsync(50);
            string jsonString;

            using (var reader = File.ReadAllTextAsync(backingFile))
            {
                jsonString = reader.Result;
            }
            personData = JsonConvert.DeserializeObject<PersonData>(jsonString);
            IncreaseBarAsync(100);
        }
        public override void OnBackPressed()
        {
            base.OnBackPressed();
        }
    }
}
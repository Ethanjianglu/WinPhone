using ExerciseApp.Common;
using ExerciseApp.Data;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Pivot Application template is documented at http://go.microsoft.com/fwlink/?LinkID=391641

namespace ExerciseApp
{
    public sealed partial class PivotPage : Page
    {
        private string ka = "";
        private string sports = "";
        private string spentka = "";

        private const string FirstGroupName = "FirstGroup";
        private const string SecondGroupName = "SecondGroup";
        private const string ThirdGroupName = "ThirdGroup";

        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");

        public PivotPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            CreateDB();
        }

        //<connection of sqllite db
        private SQLiteAsyncConnection GetConn(string dbName)
        {
            return new SQLiteAsyncConnection(ApplicationData.Current.LocalFolder.Path + "\\" + dbName);
        }

        //<create food and sports db
        private async void CreateDB()
        {
            if (!await CheckFileAsync("food.db"))
            {
                SQLiteAsyncConnection conn = GetConn("food.db");
                await conn.CreateTableAsync<FoodClass>();

                FoodClass food = new FoodClass
                {
                    FoodName = "Fruit",
                    FoodCaloriePerKG = 50
                };
                await conn.InsertAsync(food);
                food = new FoodClass
                {
                    FoodName = "Rice",
                    FoodCaloriePerKG = 60
                };
                await conn.InsertAsync(food);

                food = new FoodClass
                {
                    FoodName = "Fried Chicken",
                    FoodCaloriePerKG = 70
                };
                await conn.InsertAsync(food);
            }

            if (!await CheckFileAsync("sports.db"))
            {
                SQLiteAsyncConnection conn = GetConn("sports.db");
                await conn.CreateTableAsync<SportsClass>();

                SportsClass sport = new SportsClass
                {
                    SportsName = "Walk",
                    SportsCaloriePerHour = 30
                };
                await conn.InsertAsync(sport);
                sport = new SportsClass
                {
                    SportsName = "Run",
                    SportsCaloriePerHour = 40
                };
                await conn.InsertAsync(sport);

                sport = new SportsClass
                {
                    SportsName = "Swim",
                    SportsCaloriePerHour = 50
                };
                await conn.InsertAsync(sport);
            }

            AddItems();
        }

        //check whether the db is existed
        public async System.Threading.Tasks.Task<bool> CheckFileAsync(string fileName)
        {
            bool fileIsExist = true;
            try
            {
                Windows.Storage.StorageFile sf = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
            }
            catch
            {
                fileIsExist = false;
            }
            return fileIsExist;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>.
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            var sampleDataGroup = await SampleDataSource.GetGroupAsync("Group-1");
            this.DefaultViewModel[FirstGroupName] = sampleDataGroup;
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache. Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/>.</param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            // TODO: Save the unique state of the page here.
        }


        /// <summary>
        /// Invoked when an item within a section is clicked.
        /// </summary>
        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;
            if (!Frame.Navigate(typeof(ItemPage), itemId))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

        /// <summary>
        /// Loads the content for the second pivot item when it is scrolled into view.
        /// </summary>
        private async void SecondPivot_Loaded(object sender, RoutedEventArgs e)
        {
            //var sampleDataGroup = await SampleDataSource.GetGroupAsync("Group-2");
            //this.DefaultViewModel[SecondGroupName] = sampleDataGroup;
        }

        private async void FirstPivot_Loaded(object sender, RoutedEventArgs e)
        {
            //var sampleDataGroup = await SampleDataSource.GetGroupAsync("Group-1");
            //this.DefaultViewModel[FirstGroupName] = sampleDataGroup;
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        //query the colorie of food
        private async void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            decimal calorie = 0;

            try
            {
                string foodName = "";
                string sportsName = "";
                if (lpFoodName.SelectedIndex != -1)
                {
                    foodName = lpFoodName.SelectedItem.ToString();
                }
                if (lpSports.SelectedIndex != -1)
                {
                    sportsName = lpSports.SelectedItem.ToString();
                }
                //
                string kg = txtInTake.Text.ToString().Trim();

                if (foodName != null && sportsName != null && kg != null && foodName != "" && sportsName != "" && kg != "")
                {
                    SQLite.SQLiteAsyncConnection conn1 = new SQLite.SQLiteAsyncConnection("food.db");
                    var query1 = await conn1.QueryAsync<FoodClass>("select * from FoodClass where FoodName=?", new object[] { foodName });
                    if (query1 != null && query1.Count > 0)
                    {
                        calorie = (query1[0].FoodCaloriePerKG) * (decimal.Parse(kg));
                        txtCalorie.Text = (calorie).ToString() + " C";
                    }

                    SQLite.SQLiteAsyncConnection conn2 = new SQLite.SQLiteAsyncConnection("sports.db");
                    var query2 = await conn2.QueryAsync<SportsClass>("select * from SportsClass where SportsName=?", new object[] { sportsName });
                    if (query2 != null && query2.Count > 0)
                    {
                        txtTime.Text = ((decimal)(calorie / query2[0].SportsCaloriePerHour)).ToString() + " H";
                    }
                }
                else
                {
                    await new MessageDialog("Please Complete All Parts!").ShowAsync();
                }
            }
            catch (Exception ex)
            {
                new MessageDialog(ex.Message.ToString());
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// to add items to combox for user to choose
        /// </summary>
        public async void AddItems()
        {
            SQLite.SQLiteAsyncConnection conn = GetConn("food.db");
            var query = await conn.QueryAsync<FoodClass>("select * from FoodClass");
            if (query != null && query.Count > 0)
            {
                lpFoodName.Items.Clear();
                for (int i = 0; i < query.Count; i++)
                {
                    lpFoodName.Items.Add(((FoodClass)query[i]).FoodName);
                }
            }

            SQLite.SQLiteAsyncConnection conn1 = GetConn("sports.db");
            var query1 = await conn1.QueryAsync<SportsClass>("select * from SportsClass");
            if (query1 != null && query1.Count > 0)
            {
                lpSports.Items.Clear();
                for (int i = 0; i < query1.Count; i++)
                {
                    lpSports.Items.Add(((SportsClass)query1[i]).SportsName);
                }
            }
        }

        //value your fit accord to the gender weight and height
        //suggest some advices, 
        //modify the data in Sample Data.json
        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string gender = "";
            decimal height = 0.0M;
            decimal weight = 0.0M;


            if (female.IsChecked == true)
            {
                gender = "female";
            }

            if (male.IsChecked == true)
            {
                gender = "male";
            }
            height = decimal.Parse(txtHeight.Text);
            weight = decimal.Parse(txtWeight.Text);

            if (gender != "" && weight != 0.0M && height != 0.0M)
            {
                GetSuggestion(gender, height, weight);
            }
            else
            {
                await new MessageDialog("Please Complete All Parts!").ShowAsync();
            }
        }

        private async void GetSuggestion(string gender, decimal height, decimal weight)
        {
            decimal balanceWight = 0.0M;
            switch (gender)
            {
                case "female":
                    balanceWight = (height - 80) * 0.7M;
                    if (weight < balanceWight * 0.9M)
                    {
                        tbResult.Text = "YOU ARE LEAN";

                        var sampleDataGroup = await SampleDataSource.GetGroupAsync("Group-1");
                        this.DefaultViewModel[SecondGroupName] = sampleDataGroup;
                    }
                    else if (weight > balanceWight * 1.1M)
                    {
                        tbResult.Text = "YOU ARE OVERWEIGHT";

                        var sampleDataGroup = await SampleDataSource.GetGroupAsync("Group-2");
                        this.DefaultViewModel[SecondGroupName] = sampleDataGroup;
                    }
                    break;
                case "male":
                    balanceWight = (height - 70) * 0.6M;
                    if (weight < balanceWight * 0.9M)
                    {
                        tbResult.Text = "YOU ARE LEAN";

                        var sampleDataGroup = await SampleDataSource.GetGroupAsync("Group-1");
                        this.DefaultViewModel[SecondGroupName] = sampleDataGroup;
                    }
                    else if (weight > balanceWight * 1.1M)
                    {
                        tbResult.Text = "YOU ARE OVERWEIGHT";

                        var sampleDataGroup = await SampleDataSource.GetGroupAsync("Group-2");
                        this.DefaultViewModel[SecondGroupName] = sampleDataGroup;
                    }
                    break;
            }
        }
    }
}

using Bingie.Models;
using Bingie.Services;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bingie.Views
{
    public partial class BingeRecordsPage : ContentPage
    {
        private readonly IDataStore<BingeEntry> _dataStore;

        public BingeRecordsPage(IDataStore<BingeEntry> dataStore)
        {
            InitializeComponent();
            _dataStore = dataStore;
            LoadBingeRecordsAsync();
        }

        private async void LoadBingeRecordsAsync()
        {
            try
            {
                var records = await _dataStore.GetItemsAsync();
                var todayRecords = records.Where(r => r.Date.Date == DateTime.Today).ToList();
                BingeRecordsCollectionView.ItemsSource = todayRecords;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load records: {ex.Message}", "OK");
            }
        }

        private async void OnAddBingeRecordClicked(object sender, EventArgs e)
        {
            try
            {
                var newRecord = new BingeEntry
                {
                    Username = "User1", // Replace with logged-in user's username
                    Date = DateTime.Now,
                    Duration = TimeSpan.FromMinutes(20) // Example duration
                };

                bool success = await _dataStore.AddItemAsync(newRecord);

                if (success)
                {
                    await DisplayAlert("Success", "Binge record added successfully!", "OK");
                    LoadBingeRecordsAsync(); // Refresh records
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to add record: {ex.Message}", "OK");
            }
        }
    }
}
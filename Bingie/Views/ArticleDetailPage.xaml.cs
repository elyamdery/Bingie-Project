using System;
using Microsoft.Maui.Controls;

namespace Bingie.Views;

public partial class ArticleDetailPage : ContentPage
{
    public ArticleDetailPage(Article article)
    {
        InitializeComponent();
        
        // Set the article content
        TitleLabel.Text = article.Title;
        ContentLabel.Text = article.Content;
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}

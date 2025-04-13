using Bingie.Models;
using Bingie.Services;
using System.Collections.ObjectModel;

namespace Bingie.Views;

public partial class ExplorePage : ContentPage
{
    private readonly Random random;
    private List<string> quotes;
    private ObservableCollection<LeaderboardEntry> leaderboardEntries;
    private ObservableCollection<Article> articles;
    private readonly IDataStore<BingeEntry> _dataStore;
    private readonly string _username;

    // Default constructor for design-time and preview
    public ExplorePage()
    {
        InitializeComponent();
        random = new Random();
        LoadQuotes();
        ShowRandomQuote();
        LoadMockLeaderboard();
        LoadArticles();
    }

    // Constructor with data store and username
    public ExplorePage(IDataStore<BingeEntry> dataStore, string username)
    {
        InitializeComponent();
        _dataStore = dataStore;
        _username = username;
        random = new Random();

        LoadQuotes();
        ShowRandomQuote();
        LoadLeaderboardAsync();
        LoadArticles();
    }

    private void LoadQuotes()
    {
        quotes =
        [
            "Recovery is not a race. You don't have to feel guilty if it takes you longer than you thought it would.",
            "The strongest people are not those who show strength in front of us, but those who win battles we know nothing about.",
            "Your body hears everything your mind says. Stay positive.",
            "Every day is a second chance.",
            "You are stronger than you think.",
            "Progress, not perfection.",
            "The only way out is through.",
            "You don't have to be perfect to be worthy of love and respect."
        ];
    }

    private void ShowRandomQuote()
    {
        if (quotes != null && quotes.Count > 0)
        {
            var index = random.Next(quotes.Count);
            RandomQuoteLabel.Text = quotes[index];
        }
    }

    private void LoadMockLeaderboard()
    {
        leaderboardEntries = new ObservableCollection<LeaderboardEntry>
        {
            new LeaderboardEntry { Rank = "1", Username = "Sarah", Score = "450" },
            new LeaderboardEntry { Rank = "2", Username = "Michael", Score = "385" },
            new LeaderboardEntry { Rank = "3", Username = "Jessica", Score = "320" },
            new LeaderboardEntry { Rank = "4", Username = "David", Score = "275" },
            new LeaderboardEntry { Rank = "5", Username = "Emily", Score = "240" },
            new LeaderboardEntry { Rank = "6", Username = "Alex", Score = "210" },
            new LeaderboardEntry { Rank = "7", Username = "Sophia", Score = "185" },
            new LeaderboardEntry { Rank = "8", Username = "Daniel", Score = "150" },
            new LeaderboardEntry { Rank = "9", Username = "Olivia", Score = "125" },
            new LeaderboardEntry { Rank = "10", Username = "Ethan", Score = "100" }
        };

        // If we have a username, mark the entry as current user or add a mock rank
        if (!string.IsNullOrEmpty(_username))
        {
            // Try to find a mock entry that matches the current username
            var userEntry = leaderboardEntries.FirstOrDefault(e =>
                e.Username.Equals(_username, StringComparison.OrdinalIgnoreCase));

            if (userEntry != null)
            {
                userEntry.IsCurrentUser = true;
                YourRankLabel.Text = $"Your Rank: {userEntry.Rank}";
            }
            else
            {
                // Add a random rank for the current user
                Random rand = new Random();
                int userRank = rand.Next(11, 20); // Random rank outside top 10
                YourRankLabel.Text = $"Your Rank: {userRank}";
            }
        }
        else
        {
            YourRankLabel.Text = "Not Ranked Yet";
        }

        LeaderboardCollectionView.ItemsSource = leaderboardEntries;
    }

    private async void LoadLeaderboardAsync()
    {
        if (_dataStore == null)
        {
            LoadMockLeaderboard();
            return;
        }

        try
        {
            // Get all binge entries
            var entries = await _dataStore.GetItemsAsync();

            // Group by username and calculate days since last binge
            var userStats = entries
                .GroupBy(e => e.Username)
                .Select(g => new
                {
                    Username = g.Key,
                    LastBingeDate = g.Max(e => e.Date),
                    DaysSinceLastBinge = (int)(DateTime.Today - g.Max(e => e.Date).Date).TotalDays,
                    TotalPoints = CalculatePoints(g.ToList())
                })
                .OrderByDescending(u => u.TotalPoints)
                .ThenByDescending(u => u.DaysSinceLastBinge)
                .Take(10)
                .ToList();

            // Create leaderboard entries
            leaderboardEntries = new ObservableCollection<LeaderboardEntry>();
            for (int i = 0; i < userStats.Count; i++)
            {
                var stat = userStats[i];
                leaderboardEntries.Add(new LeaderboardEntry
                {
                    Rank = (i + 1).ToString(),
                    Username = stat.Username,
                    Score = stat.TotalPoints.ToString(),
                    IsCurrentUser = stat.Username == _username
                });
            }

            // Update the user's rank display
            var currentUserEntry = leaderboardEntries.FirstOrDefault(e => e.Username == _username);
            if (currentUserEntry != null)
            {
                YourRankLabel.Text = $"Your Rank: {currentUserEntry.Rank}";
            }
            else
            {
                // User not in top 10, calculate their position
                var allUserStats = entries
                    .GroupBy(e => e.Username)
                    .Select(g => new
                    {
                        Username = g.Key,
                        TotalPoints = CalculatePoints(g.ToList())
                    })
                    .OrderByDescending(u => u.TotalPoints)
                    .ToList();

                var userRank = allUserStats.FindIndex(u => u.Username == _username) + 1;
                if (userRank > 0)
                {
                    YourRankLabel.Text = $"Your Rank: {userRank}";
                }
                else
                {
                    YourRankLabel.Text = "Not Ranked Yet";
                }
            }

            LeaderboardCollectionView.ItemsSource = leaderboardEntries;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load leaderboard: {ex.Message}", "OK");
            LoadMockLeaderboard();
        }
    }

    private int CalculatePoints(List<BingeEntry> entries)
    {
        if (entries == null || !entries.Any())
            return 0;

        // Calculate points based on:
        // 1. Days since last binge (more days = more points)
        // 2. Frequency reduction (fewer binges in recent period compared to earlier period = more points)

        // Points for days since last binge
        var lastBingeDate = entries.Max(e => e.Date).Date;
        var daysSinceLastBinge = (int)(DateTime.Today - lastBingeDate).TotalDays;
        int streakPoints = Math.Min(daysSinceLastBinge * 10, 300); // Cap at 300 points (30 days)

        // Points for frequency reduction
        var now = DateTime.Now;
        var oneMonthAgo = now.AddMonths(-1);
        var twoMonthsAgo = now.AddMonths(-2);

        var recentBinges = entries.Count(e => e.Date >= oneMonthAgo);
        var previousBinges = entries.Count(e => e.Date >= twoMonthsAgo && e.Date < oneMonthAgo);

        int improvementPoints = 0;
        if (previousBinges > 0)
        {
            // Calculate improvement percentage
            double improvementRate = previousBinges > 0 ?
                Math.Max(0, (previousBinges - recentBinges) / (double)previousBinges) : 0;

            improvementPoints = (int)(improvementRate * 200); // Up to 200 points for improvement
        }

        // Consistency bonus
        int consistencyPoints = 0;
        if (entries.Count >= 5)
        {
            consistencyPoints = 50; // Bonus for consistent tracking
        }

        return streakPoints + improvementPoints + consistencyPoints;
    }

    private void LoadArticles()
    {
        articles = new ObservableCollection<Article>
        {
            new Article
            {
                Title = "Understanding Binge Eating Disorder",
                Summary = "Learn about the causes, symptoms, and treatments for Binge Eating Disorder (BED).",
                Content = "Binge Eating Disorder (BED) is characterized by recurrent episodes of eating large quantities of food, a feeling of a loss of control during the binge, and experiencing shame, distress, or guilt afterward. BED is the most common eating disorder in the United States.\n\nSymptoms include:\n- Eating unusually large amounts of food in a specific amount of time\n- Feeling that your eating behavior is out of control\n- Eating even when you're full or not hungry\n- Eating rapidly during binge episodes\n- Eating until you're uncomfortably full\n- Frequently eating alone or in secret\n- Feeling depressed, disgusted, or guilty about your eating\n\nTreatment options include cognitive behavioral therapy (CBT), interpersonal psychotherapy (IPT), dialectical behavior therapy (DBT), and medication. If you think you might have BED, it's important to seek help from a healthcare professional."
            },
            new Article
            {
                Title = "Strategies to Overcome Binge Eating",
                Summary = "Practical tips and techniques to help manage and reduce binge eating episodes.",
                Content = "Here are some strategies that may help you manage binge eating:\n\n1. Keep a food and mood journal to identify triggers\n2. Practice mindful eating - pay attention to what and why you're eating\n3. Develop a regular eating pattern with planned meals and snacks\n4. Learn to distinguish between physical and emotional hunger\n5. Find healthy ways to manage stress (meditation, exercise, hobbies)\n6. Remove trigger foods from your home\n7. Seek support from friends, family, or a support group\n8. Practice self-compassion and avoid negative self-talk\n9. Get enough sleep\n10. Stay hydrated\n\nRemember that recovery is a process, and setbacks are a normal part of that process. Be patient with yourself and celebrate small victories along the way."
            },
            new Article
            {
                Title = "The Emotional Aspects of Binge Eating",
                Summary = "Exploring the emotional triggers and psychological factors behind binge eating behaviors.",
                Content = "Binge eating often serves as a coping mechanism for difficult emotions. Many people use food to self-soothe when experiencing negative feelings such as stress, anxiety, depression, boredom, or loneliness.\n\nEmotional triggers for binge eating may include:\n- Work or school stress\n- Relationship conflicts\n- Financial worries\n- Health problems\n- Poor body image\n- Low self-esteem\n- History of dieting or restrictive eating\n- Perfectionism\n\nUnderstanding your emotional triggers is an important step in breaking the binge eating cycle. Therapy approaches like Cognitive Behavioral Therapy (CBT) can help you identify these triggers and develop healthier coping strategies.\n\nLearning to tolerate difficult emotions without turning to food is a skill that takes practice. Techniques such as deep breathing, progressive muscle relaxation, and mindfulness meditation can help you manage emotions without binge eating."
            },
            new Article
            {
                Title = "The Role of Nutrition in Recovery",
                Summary = "How balanced nutrition can support recovery from binge eating disorder.",
                Content = "Proper nutrition plays a crucial role in recovery from binge eating disorder. Here are some nutritional principles that can support your recovery journey:\n\n1. Regular eating pattern: Aim to eat every 3-4 hours to maintain stable blood sugar levels and prevent extreme hunger that can trigger binges.\n\n2. Balanced meals: Include protein, complex carbohydrates, healthy fats, and fiber in your meals to promote satiety and energy balance.\n\n3. Adequate hydration: Sometimes thirst can be mistaken for hunger. Staying well-hydrated can help prevent this confusion.\n\n4. Mindful portion sizes: Learn to recognize appropriate portion sizes without being overly restrictive.\n\n5. Include all food groups: Avoid eliminating entire food groups, as restriction often leads to bingeing later.\n\n6. Plan for challenging foods: Gradually incorporate foods you tend to binge on in moderate amounts and in structured settings.\n\n7. Mindful eating: Pay attention to hunger and fullness cues, and eat slowly to enhance satisfaction.\n\nWorking with a registered dietitian who specializes in eating disorders can provide personalized guidance for your specific needs."
            },
            new Article
            {
                Title = "Building a Support System",
                Summary = "The importance of social support in recovering from binge eating disorder.",
                Content = "Recovery from binge eating disorder is not something you have to face alone. Building a strong support system can significantly improve your chances of successful recovery.\n\nComponents of an effective support system may include:\n\n1. Professional support: Therapists, dietitians, and physicians who specialize in eating disorders\n\n2. Support groups: Both in-person and online groups where you can connect with others who understand your experiences\n\n3. Friends and family: Loved ones who are educated about your condition and can provide emotional support\n\n4. Recovery apps and tools: Digital resources that help you track progress and provide coping strategies\n\n5. Crisis resources: Hotlines and text lines for moments when you need immediate support\n\nWhen building your support network, it's important to communicate your needs clearly. Let people know specific ways they can help, whether that's providing distraction during difficult times, joining you for regular meals, or simply listening without judgment.\n\nRemember that vulnerability is a strength, not a weakness. Reaching out for help shows courage and commitment to your recovery."
            }
        };

        ArticlesCollectionView.ItemsSource = articles;
    }

    private async void OnArticleSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Article selectedArticle)
        {
            // Clear selection
            ArticlesCollectionView.SelectedItem = null;

            // Navigate to article detail page
            await Navigation.PushAsync(new ArticleDetailPage(selectedArticle));
        }
    }

    private async void OnResourceTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is string resourceType)
        {
            string title = "Resource Information";
            string message = "";

            switch (resourceType)
            {
                case "support":
                    title = "Support Groups";
                    message = "Support groups provide a safe space to share experiences and strategies with others who understand what you're going through. Both in-person and online options are available.";
                    break;

                case "therapy":
                    title = "Therapy Options";
                    message = "Professional therapy is highly effective for treating binge eating disorder. Options include cognitive-behavioral therapy (CBT), interpersonal psychotherapy (IPT), and dialectical behavior therapy (DBT).";
                    break;

                case "nutrition":
                    title = "Nutrition Guide";
                    message = "Working with a registered dietitian who specializes in eating disorders can help you develop a balanced approach to nutrition without triggering binge episodes.";
                    break;

                case "crisis":
                    title = "Crisis Hotlines";
                    message = "If you're experiencing a crisis or need immediate support, help is available 24/7 through various hotlines and text services dedicated to eating disorder support.";
                    break;

                default:
                    message = "Additional information about this resource will be available soon.";
                    break;
            }

            await DisplayAlert(title, message, "OK");
        }
    }
}

// Model classes for the ExplorePage
public class LeaderboardEntry
{
    public string Rank { get; set; }
    public string Username { get; set; }
    public string Score { get; set; }
    public bool IsCurrentUser { get; set; }
}

public class Article
{
    public string Title { get; set; }
    public string Summary { get; set; }
    public string Content { get; set; }
}
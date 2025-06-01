# Bingie User Manual

This document provides a guide on how to use the Bingie application.

## Table of Contents

1.  [Introduction](#introduction)
2.  [Getting Started](#getting-started)
3.  [Core Concepts](#core-concepts)
    *   [Users](#users)
4.  [Features](#features)
    *   [Authentication (Login/Registration)](#authentication)
    *   [Recording Binge Entries](#recording-binge-entries)
    *   [Viewing History](#viewing-history)
    *   [Viewing Statistics](#viewing-statistics)
    *   [Exploring Data](#exploring-data)
5.  [Troubleshooting](#troubleshooting)

## 1. Introduction

Welcome to Bingie! This application helps you track and manage specific activities (referred to as "binges").

*(You can expand this section to provide a more detailed overview of the application.)*

## 2. Getting Started

To start using Bingie:

1.  **Installation**: Ensure the application is correctly installed on your device (Android, iOS, Windows, or macOS).
2.  **First Launch**: Open the application. You will likely be greeted with a login or registration page.
3.  **Registration**: If you are a new user, you will need to register an account. This typically involves providing a username and a password.
4.  **Login**: If you already have an account, log in with your credentials.

## 3. Core Concepts

### Users

The application uses a `User` model to manage user accounts. Each user has a `Username` and a `Password`.

**Developer Note: How to use the `User` model in code**

The `Bingie.Models.User` class (defined in `Models/User.cs`) is used to represent a user in your application, typically for storing username and password information.

Here's a simple example of how you can create a `User` object in C#:

```csharp
// This example shows how to create a new User object.
// In a real application, always store hashed passwords, not plain text.

var newUser = new Bingie.Models.User
{
    Username = "exampleUser",
    Password = "hashedPasswordValue" // This should be a securely hashed password
};

// After creating a newUser object, you might add it to your database
// using your AppDBContext from Data/AppDBContext.cs.
// For example (conceptual):
//
// using (var dbContext = new AppDBContext()) // Assuming AppDBContext is set up
// {
//     dbContext.Users.Add(newUser);
//     await dbContext.SaveChangesAsync();
// }
```

**Important Security Note**: When handling passwords, always ensure they are securely hashed before storing them. Do not store plain-text passwords.

## 4. Features

*(This section should detail the main features of your application. Below are placeholders based on the project structure.)*

### Authentication

*   **Login Page (`Views/Auth/LoginPage.xaml`)**: Allows existing users to sign in.
*   **Registration Page (`Views/Auth/RegistrationPage.xaml`)**: Allows new users to create an account.

### Recording Binge Entries

*   **Binge Record Page (`Views/BingeRecordPage.xaml`)**: This is where users can input and save new binge entries.
    *   *(Describe what information can be recorded, e.g., type of activity, duration, notes, etc.)*

### Viewing History

*   **History Page (`Views/HistoryPage.xaml`)**: Displays a list or log of past binge entries.
    *   *(Describe how users can interact with the history, e.g., view details, filter, sort.)*

### Viewing Statistics

*   **Statistics Page (`Views/StatisticsPage.xaml`)**: Presents an overview of binge activity through charts or summaries.
*   **Day Statistics Page (`Views/DayStatisticsPage.xaml`)**: Shows statistics for a specific day.
    *   *(Describe what kind of statistics are shown, e.g., frequency, trends over time.)*

### Exploring Data

*   **Explore Page (`Views/ExplorePage.xaml`)**: This page might offer different ways to explore or analyze the recorded data.
    *   *(Describe its functionality.)*

## 5. Troubleshooting

*(Add common issues and their solutions here. For example:)*
*   **Cannot log in**:
    *   Verify your username and password.
    *   Ensure you have an internet connection if authentication is server-based.
    *   Try resetting your password (if this feature exists).
*   **Data not saving**:
    *   Check device storage.
    *   Ensure the app has necessar permissions.

---

*This is a basic template. You should expand on each section with specific details relevant to the Bingie application's functionality and user interface.*

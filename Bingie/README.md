# Bingie App

Bingie is an app designed to help users track and overcome binge eating. This README provides instructions on how to run the app on different platforms.

## Running the App

### Prerequisites

- Visual Studio 2022 with .NET MAUI workload installed
- For iOS development: 
  - Mac with Xcode 13 or later
  - Apple Developer account (for device deployment)

### Running on Windows

1. Open the solution in Visual Studio
2. Select "Windows Machine" from the run target dropdown
3. Click the Run button (green triangle) or press F5

### Running on iOS Simulator

1. Open the solution in Visual Studio
2. Connect to a Mac build host (Visual Studio → Tools → iOS → Connect to Mac)
3. Select "iOS Simulator" from the run target dropdown
4. Choose your preferred simulator device
5. Click the Run button (green triangle) or press F5

### Running on iOS Device

1. Open the solution in Visual Studio
2. Connect to a Mac build host (Visual Studio → Tools → iOS → Connect to Mac)
3. Connect your iOS device to your Mac
4. Select your iOS device from the run target dropdown
5. Ensure you have a valid provisioning profile configured in the project
6. Click the Run button (green triangle) or press F5

## Troubleshooting

### App Tries to Run as a Web Application

If the app tries to run as a web application instead of a native app:

1. Right-click on the Bingie project in Solution Explorer
2. Select "Properties"
3. Go to "Debug"
4. Make sure the correct launch profile is selected (Windows Machine, iOS Simulator, etc.)
5. Ensure "Launch browser" is unchecked

### iOS Build Issues

If you encounter issues building for iOS:

1. Make sure your Mac build host is properly connected
2. Verify that Xcode is up to date on your Mac
3. Check that your Apple Developer account is properly configured
4. Ensure the provisioning profile is valid and matches your app's bundle identifier

## User Manual

For detailed instructions on how to use the app, please refer to the [User Manual](UserManual.md) included in the project.

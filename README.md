
# Social Media Authentication System in ASP.NET Core with Identity

This project demonstrates how to integrate social media authentication (Google and Facebook) in an ASP.NET Core MVC application using Identity.

## Features

- Google and Facebook authentication using OAuth 2.0
- ASP.NET Core Identity integration
- Custom login page with Google Sign-In
- Optional use of default Identity UI
- SQL Server support for user data storage

## Prerequisites

- Visual Studio 2022 (Community or higher)
- SQL Server running locally
- Google Developer Console account
- Facebook Developer account

## Getting Started

### 1. Clone the Repository

Download the project source code or clone the repository:

```bash
git clone https://github.com/your-repo/SocialMediaAuthApp.git
cd SocialMediaAuthApp
```

### 2. Configure the Database

Edit `appsettings.json` to match your SQL Server configuration:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=SocialMediaAuthDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

Run the following commands in Package Manager Console:

```powershell
Add-Migration InitialCreate
Update-Database
```

### 3. Set Up Google OAuth

- Go to [Google Developer Console](https://console.developers.google.com/)
- Create a new project
- Configure OAuth consent screen and credentials
- Add `https://localhost:7149/signin-google` as an authorized redirect URI

### 4. Set Up Facebook OAuth

- Go to [Facebook Developers](https://developers.facebook.com/)
- Create a new app
- Add Facebook Login to your app and configure settings
- Add `https://localhost:7149/signin-facebook` as a valid OAuth redirect URI

### 5. Install Required NuGet Packages

```bash
Install-Package Microsoft.AspNetCore.Authentication.Google
Install-Package Microsoft.AspNetCore.Authentication.Facebook
Install-Package Microsoft.VisualStudio.Web.CodeGeneration.Design
```

### 6. Configure Authentication in `Program.cs`

```csharp
builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = "YOUR_GOOGLE_CLIENT_ID";
        options.ClientSecret = "YOUR_GOOGLE_CLIENT_SECRET";
    })
    .AddFacebook(options =>
    {
        options.AppId = "YOUR_FACEBOOK_APP_ID";
        options.AppSecret = "YOUR_FACEBOOK_APP_SECRET";
    });
```

Ensure authentication middleware is added:

```csharp
app.UseAuthentication();
app.UseAuthorization();
```

### 7. Custom Login Page (Optional)

- Create `Controllers/AccountController.cs`
- Create `Views/Account/Login.cshtml`
- Create `Models/LoginViewModel.cs`

Use this to add a custom login button for Google.

### 8. Testing

Run the app and test:

- Custom Login: `https://localhost:7149/Account/Login`
- Default Identity Login: `https://localhost:7149/Identity/Account/Login`

### 9. Notes

- Ensure redirect URIs match exactly in Google and Facebook dev consoles.
- Use the correct port (e.g., 7149) in the redirect URIs.

## License

Free to use for educational and non-commercial purposes.

---

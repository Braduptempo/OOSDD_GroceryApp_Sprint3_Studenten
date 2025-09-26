using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Data.Repositories;
using Grocery.Core.Helpers;
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.App.ViewModels;

public partial class RegisterViewModel : ObservableObject
{
    private readonly IClientService _clientService;

    public RegisterViewModel(IClientService clientService)
    {
        _clientService = clientService;
    }

    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private string email;

    [ObservableProperty]
    private string password;

    [ObservableProperty]
    private string confirmPassword;

    [ObservableProperty]
    private string message;

    [RelayCommand]
    private async Task Register()
    {
        if (string.IsNullOrWhiteSpace(Name) || 
            string.IsNullOrWhiteSpace(Email) || 
            string.IsNullOrWhiteSpace(Password))
        {
            Message = "Vul alle velden in.";
            return;
        }
        
        if (!Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            Message = "Voer een geldig e-mailadres in.";
            return;
        }

        if (Password != ConfirmPassword)
        {
            Message = "Wachtwoorden komen niet overeen.";
            return;
        }
        
        // Minimaal 8 tekens, 1 hoofdletter, 1 kleine letter en 1 cijfer
        if (!Regex.IsMatch(Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$"))
        {
            Message = "Wachtwoord moet minimaal 8 tekens bevatten, inclusief hoofdletter, kleine letter en cijfer.";
            return;
        }
        
        var hashPassword = PasswordHelper.HashPassword(Password);

        bool success = await RegisterUserAsync(Name, Email, hashPassword);

        if (success)
        {
            if (Application.Current.MainPage is NavigationPage nav)
            {
                await nav.PopAsync(); // terug naar login
            }
        }
        else
        {
            Message = "Registratie mislukt. Probeer opnieuw.";
        }
    }

    private Task<bool> RegisterUserAsync(string name, string email, string password)
    {
        // Controleer eerst of e-mail al bestaat
        if (_clientService.Get(email) != null)
            return Task.FromResult(false);

        // Maak nieuwe client aan
        var newClient = new Client(0, name, email, password); // Id wordt door Add automatisch gezet

        _clientService.Add(newClient);

        return Task.FromResult(true);
    }
}

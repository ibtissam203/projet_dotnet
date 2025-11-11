using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProductApi.Models;
using ProductApi.Services;

namespace ProductApi.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly AuthService _authService;

        public RegisterModel(AuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        public string FirstName { get; set; } = string.Empty;

        [BindProperty]
        public string LastName { get; set; } = string.Empty;

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        [BindProperty]
        public string ConfirmPassword { get; set; } = string.Empty;

        [BindProperty]
        public bool Newsletter { get; set; } = true;

        public string ErrorMessage { get; set; } = string.Empty;
        public string SuccessMessage { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Validation manuelle des mots de passe
                if (Password != ConfirmPassword)
                {
                    ErrorMessage = "Les mots de passe ne correspondent pas.";
                    return Page();
                }

                if (Password.Length < 6)
                {
                    ErrorMessage = "Le mot de passe doit contenir au moins 6 caractères.";
                    return Page();
                }

                // Créer l'utilisateur
                var user = new User
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Email = Email,
                    PasswordHash = Password, // Sera hashé dans le service
                    NewsletterSubscription = Newsletter,
                    Role = "Client" // Toujours Client à l'inscription
                };

                await _authService.CreateUserAsync(user);

                SuccessMessage = "Compte créé avec succès ! Vous pouvez maintenant vous connecter.";

                // Optionnel : Rediriger après succès
                // return RedirectToPage("/Auth/Login");

                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }
    }
}
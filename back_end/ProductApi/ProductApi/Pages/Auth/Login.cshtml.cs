using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProductApi.Services;

namespace ProductApi.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly IAuthService _authService;  // 👈 UTILISE L'INTERFACE

        public LoginModel(IAuthService authService)  // 👈 INJECTION PAR INTERFACE
        {
            _authService = authService;
        }

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        [BindProperty]
        public bool RememberMe { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // VÉRIFICATION 1 : Les champs sont-ils remplis ?
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "Veuillez remplir tous les champs.";
                return Page();
            }

            // VÉRIFICATION 2 : Format email valide ?
            try
            {
                var emailAddress = new System.Net.Mail.MailAddress(Email);
            }
            catch
            {
                ErrorMessage = "Format d'email invalide.";
                return Page();
            }

            // MAINTENANT on appelle le service d'authentification
            var user = await _authService.AuthenticateAsync(Email, Password);

            if (user != null)
            {
                // Créer la session
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("UserRole", user.Role);
                HttpContext.Session.SetString("UserName", $"{user.FirstName} {user.LastName}");

                // Redirection selon le rôle
                if (user.Role == "Admin")
                {
                    return RedirectToPage("/Admin/Dashboard");
                }
                else
                {
                    return RedirectToPage("/Products/Index");
                }
            }
            else
            {
                ErrorMessage = "Email ou mot de passe incorrect.";
                return Page();
            }
        }
    }
}
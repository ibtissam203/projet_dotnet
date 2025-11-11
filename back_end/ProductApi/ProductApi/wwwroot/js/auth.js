// Gestion des tabs
function showAuthTab(tabName) {
    // Masquer tous les contenus
    document.querySelectorAll('.auth-content').forEach(content => {
        content.classList.remove('active');
    });

    // Désactiver tous les onglets
    document.querySelectorAll('.auth-tab').forEach(tab => {
        tab.classList.remove('active');
    });

    // Afficher le contenu sélectionné
    document.getElementById(tabName + '-form').classList.add('active');

    // Activer l'onglet sélectionné
    event.currentTarget.classList.add('active');

    // Reset des erreurs
    clearErrors();
}

// Toggle password visibility
function togglePassword(inputId) {
    const input = document.getElementById(inputId);
    const button = event.currentTarget;
    const type = input.getAttribute('type') === 'password' ? 'text' : 'password';

    input.setAttribute('type', type);
    button.textContent = type === 'password' ? '👁️' : '🔒';
}

// Validation des formulaires
function validateLoginForm() {
    const email = document.getElementById('login-email').value;
    const password = document.getElementById('login-password').value;
    let isValid = true;

    clearErrors();

    // Validation email
    if (!email) {
        showError('login-email', 'L\'email est requis');
        isValid = false;
    } else if (!isValidEmail(email)) {
        showError('login-email', 'Format d\'email invalide');
        isValid = false;
    }

    // Validation password
    if (!password) {
        showError('login-password', 'Le mot de passe est requis');
        isValid = false;
    }

    return isValid;
}

function validateRegisterForm() {
    const firstName = document.getElementById('register-firstname').value;
    const lastName = document.getElementById('register-lastname').value;
    const email = document.getElementById('register-email').value;
    const password = document.getElementById('register-password').value;
    const confirmPassword = document.getElementById('register-confirm-password').value;
    let isValid = true;

    clearErrors();

    // Validation prénom
    if (!firstName) {
        showError('register-firstname', 'Le prénom est requis');
        isValid = false;
    }

    // Validation nom
    if (!lastName) {
        showError('register-lastname', 'Le nom est requis');
        isValid = false;
    }

    // Validation email
    if (!email) {
        showError('register-email', 'L\'email est requis');
        isValid = false;
    } else if (!isValidEmail(email)) {
        showError('register-email', 'Format d\'email invalide');
        isValid = false;
    }

    // Validation password
    if (!password) {
        showError('register-password', 'Le mot de passe est requis');
        isValid = false;
    } else if (password.length < 6) {
        showError('register-password', 'Le mot de passe doit contenir au moins 6 caractères');
        isValid = false;
    }

    // Validation confirmation password
    if (password !== confirmPassword) {
        showError('register-confirm-password', 'Les mots de passe ne correspondent pas');
        isValid = false;
    }

    return isValid;
}

// Utilitaires
function isValidEmail(email) {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}

function showError(fieldId, message) {
    const field = document.getElementById(fieldId);
    const formGroup = field.closest('.auth-form-group');

    field.classList.add('error');

    // Supprimer l'erreur existante
    const existingError = formGroup.querySelector('.auth-error-message');
    if (existingError) {
        existingError.remove();
    }

    // Ajouter le message d'erreur
    const errorElement = document.createElement('div');
    errorElement.className = 'auth-error-message';
    errorElement.style.color = 'var(--error)';
    errorElement.style.fontSize = '0.8rem';
    errorElement.style.marginTop = '0.5rem';
    errorElement.textContent = message;

    formGroup.appendChild(errorElement);
}

function clearErrors() {
    // Supprimer les classes d'erreur
    document.querySelectorAll('.auth-form-group input').forEach(input => {
        input.classList.remove('error');
    });

    // Supprimer les messages d'erreur
    document.querySelectorAll('.auth-error-message').forEach(error => {
        error.remove();
    });
}

// Simulation d'authentification
async function handleLogin(event) {
    event.preventDefault();

    if (!validateLoginForm()) {
        return;
    }

    const button = event.target.querySelector('.auth-btn');
    const originalText = button.textContent;

    // Show loading
    button.disabled = true;
    button.innerHTML = '<div class="spinner"></div>';

    // Simuler appel API
    try {
        await new Promise(resolve => setTimeout(resolve, 1500));

        // Ici tu feras l'appel réel à ton API
        const email = document.getElementById('login-email').value;
        const password = document.getElementById('login-password').value;

        // Simulation de réponse
        if (email === 'admin@beauty.com' && password === 'admin123') {
            showMessage('Connexion réussie ! Redirection...', 'success');
            setTimeout(() => {
                window.location.href = '/Admin/Dashboard';
            }, 1000);
        } else if (email === 'client@beauty.com' && password === 'client123') {
            showMessage('Connexion réussie ! Redirection...', 'success');
            setTimeout(() => {
                window.location.href = '/Products';
            }, 1000);
        } else {
            showMessage('Email ou mot de passe incorrect', 'error');
        }
    } catch (error) {
        showMessage('Erreur de connexion', 'error');
    } finally {
        // Reset button
        button.disabled = false;
        button.textContent = originalText;
    }
}

async function handleRegister(event) {
    event.preventDefault();

    if (!validateRegisterForm()) {
        return;
    }

    const button = event.target.querySelector('.auth-btn');
    const originalText = button.textContent;

    // Show loading
    button.disabled = true;
    button.innerHTML = '<div class="spinner"></div>';

    // Simuler appel API
    try {
        await new Promise(resolve => setTimeout(resolve, 1500));

        // Ici tu feras l'appel réel à ton API
        showMessage('Compte créé avec succès ! Vous pouvez vous connecter.', 'success');

        // Switch to login tab
        setTimeout(() => {
            document.querySelectorAll('.auth-tab')[0].click();
        }, 2000);

    } catch (error) {
        showMessage('Erreur lors de la création du compte', 'error');
    } finally {
        // Reset button
        button.disabled = false;
        button.textContent = originalText;
    }
}

function showMessage(message, type) {
    // Supprimer les messages existants
    const existingMessages = document.querySelectorAll('.auth-message');
    existingMessages.forEach(msg => msg.remove());

    // Créer le message
    const messageElement = document.createElement('div');
    messageElement.className = `auth-message alert alert-${type}`;
    messageElement.textContent = message;
    messageElement.style.marginBottom = '1rem';

    // Ajouter au formulaire actif
    const activeForm = document.querySelector('.auth-content.active');
    activeForm.insertBefore(messageElement, activeForm.firstChild);

    // Auto-remove after 5 seconds
    setTimeout(() => {
        messageElement.remove();
    }, 5000);
}

// Initialisation
document.addEventListener('DOMContentLoaded', function () {
    // Ajouter les écouteurs d'événements
    const loginForm = document.getElementById('login-form');
    const registerForm = document.getElementById('register-form');

    if (loginForm) {
        loginForm.addEventListener('submit', handleLogin);
    }

    if (registerForm) {
        registerForm.addEventListener('submit', handleRegister);
    }
});
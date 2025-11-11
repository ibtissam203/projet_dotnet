// Gestion du panier avec localStorage
class CartManager {
    constructor() {
        this.storageKey = 'beautyCart'; // nom de la clé dans localStorage
        this.init();// on lance l'initialisation
    }
    //lance les fonctions de base dès que la classe est créée.
    init() {
        this.updateCartCount();
        this.setupCartEventListeners();
    }

    // Récupérer le panier depuis localStorage
    getCart() {
        try {
            const cart = localStorage.getItem(this.storageKey);
            return cart ? JSON.parse(cart) : [];
        } catch (error) {
            console.error('Erreur lecture panier:', error);
            return [];
        }
    }

    // Sauvegarder le panier dans localStorage sous format JSON
    saveCart(cart) {
        try {
            //on convertit le tableau cart en texte JSON
            localStorage.setItem(this.storageKey, JSON.stringify(cart));
            this.updateCartCount();
            this.dispatchCartUpdate();
        } catch (error) {
            console.error('Erreur sauvegarde panier:', error);
        }
    }

    // Ajouter un produit au panier
    addToCart(productId, productName, productPrice, productImage) {
        const cart = this.getCart();
        // Vérifie si le produit est déjà dans le panier
        const existingItem = cart.find(item => item.id === productId);

        if (existingItem) {
            existingItem.quantity += 1;
        } else {
            //ajoute le nouvel objet produit au tableau cart
            cart.push({
                id: productId,
                name: productName,
                price: productPrice,
                image: productImage,
                quantity: 1
            });
        }

        this.saveCart(cart);
        this.showNotification(`"${productName}" ajouté au panier !`, 'success');
        return cart;
    }

    // Modifier la quantité d’un produit dans le panier.
    updateQuantity(productId, quantity) {
        if (quantity < 1) {
            this.removeFromCart(productId);
            return;
        }

        const cart = this.getCart();
        //On cherche dans le tableau le produit qui a l’identifiant correspondant.
        const item = cart.find(item => item.id === productId);

        if (item) {
            item.quantity = quantity;
            this.saveCart(cart);
        }
    }

    // Supprimer un produit du panier
    removeFromCart(productId) {
        //On utilise .filter() pour créer un nouveau tableau qui exclut le produit dont l’ID correspond à productId
        const cart = this.getCart().filter(item => item.id !== productId);
        //On sauvegarde ce nouveau panier dans localStorage via saveCart()
        this.saveCart(cart);
        this.showNotification('Produit retiré du panier', 'info');
    }

    // Vider le panier
    clearCart() {
        //Supprime complètement la clé 'beautyCart' du localStorage, donc le panier entier.
        localStorage.removeItem(this.storageKey);
        this.updateCartCount();
        this.dispatchCartUpdate();
        this.showNotification('Panier vidé', 'info');
    }

    // Mettre à jour le compteur du panier
    updateCartCount() {
        //Récupération du panier
        const cart = this.getCart();
        //Calcul du nombre total d’articles
        const totalItems = cart.reduce((sum, item) => sum + item.quantity, 0);

        // Sélection des éléments HTML à mettre à jour
        const cartBadges = document.querySelectorAll('.cart-count');
        //Mise à jour visuelle du compteur
        cartBadges.forEach(badge => {
            badge.textContent = totalItems;
            badge.style.display = totalItems > 0 ? 'inline' : 'none';
        });
    }

    // Événement personnalisé pour les mises à jour du panier
    dispatchCartUpdate() {
        window.dispatchEvent(new CustomEvent('cartUpdated', {
            detail: { cart: this.getCart() }
        }));
    }

    // Configuration des écouteurs d'événements
    //Le compteur du panier reste à jour dans tous les onglets
    setupCartEventListeners() {
        // Écouter les changements depuis d'autres onglets/fenêtres
        window.addEventListener('storage', (e) => {
            //la clé modifiée est bien celle du panier
            if (e.key === this.storageKey) {
                this.updateCartCount();
            }
        });

        // Écouter les événements personnalisés
        //Il est déclenché chaque fois que le panier est modifié (ajout, suppression, etc.).
        window.addEventListener('cartUpdated', () => {
            this.updateCartCount();
        });
    }

    // Afficher une notification
    showNotification(message, type = 'info') {
        const alertClass = {
            'success': 'alert-success',
            'error': 'alert-danger',
            'warning': 'alert-warning',
            'info': 'alert-info'
        }[type] || 'alert-info';

        const alert = document.createElement('div');
        alert.className = `alert ${alertClass} alert-dismissible fade show`;
        alert.style.cssText = `
            position: fixed; 
            top: 20px; 
            right: 20px; 
            z-index: 9999; 
            min-width: 300px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.15);
        `;

        const icon = {
            'success': '✅',
            'error': '❌',
            'warning': '⚠️',
            'info': 'ℹ️'
        }[type] || 'ℹ️';

        alert.innerHTML = `
            <strong>${icon}</strong> ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        `;

        document.body.appendChild(alert);

        // Auto-suppression après 3 secondes
        setTimeout(() => {
            if (alert.parentNode) {
                const bsAlert = new bootstrap.Alert(alert);
                bsAlert.close();
            }
        }, 3000);
    }

    // Calculer le total du panier
    getCartTotal() {
        const cart = this.getCart();
        //additionner le prix total de chaque produit
        return cart.reduce((total, item) => total + (item.price * item.quantity), 0);
    }

    // Récupérer les données du panier pour affichage
    getCartData() {
        const cart = this.getCart();
        return {
            items: cart,
            //le nombre total d’articles
            totalItems: cart.reduce((sum, item) => sum + item.quantity, 0),
            //le montant total, calculé via getCartTotal()
            totalPrice: this.getCartTotal(),
            isEmpty: cart.length === 0
        };
    }
}

// Initialiser le gestionnaire de panier
const cartManager = new CartManager();

// Fonction globale pour être appelée depuis HTML
function addToCart(productId, productName, productPrice, productImage) {
    return cartManager.addToCart(productId, productName, productPrice, productImage);
}

function removeFromCart(productId) {
    return cartManager.removeFromCart(productId);
}

function updateCartQuantity(productId, quantity) {
    return cartManager.updateQuantity(productId, quantity);
}

function clearCart() {
    return cartManager.clearCart();
}

function getCart() {
    return cartManager.getCart();
}

function saveCart(cart) {
    return cartManager.saveCart(cart);
}

function updateCartCount() {
    return cartManager.updateCartCount();
}

function showNotification(message, type) {
    return cartManager.showNotification(message, type);
}

//Ces fonctions sont attachées à l’objet global window,ce qui les rend accessibles partout dans le site

window.removeFromCart = function (productId) {
    return cartManager.removeFromCart(productId);
};

// FONCTION POUR LA MISE À JOUR DE QUANTITÉ
window.updateQuantity = function (productId, quantity) {
    return cartManager.updateQuantity(productId, quantity);
};

// Exposer le gestionnaire globalement
window.cartManager = cartManager;
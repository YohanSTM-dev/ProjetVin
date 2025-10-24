Readme fonctionnement Connection
---

# 🍷 Projet Cave à Vin – Système de Connexion et Gestion des Rôles

## 📘 Objectif

Ce projet met en place un système de connexion sécurisé (normalement) basé sur les rôles gérant une cave à vin.
Trois types d’utilisateurs existent :

* **Serveur**
* **Sommelier**
* **Administrateur**

Chaque rôle possède un mot de passe d’accès distinct, permettant de protéger les fonctionnalités selon le niveau d’autorisation.

---

## 🧩 1. Présentation du système de rôles

Le système de rôles repose sur une **table SQL dédiée : `RoleAccess`**, qui stocke les informations d’accès pour chaque type d’utilisateur.
Cette table est indépendante de la table `Utilisateur` (qui gère les comptes individuels).

### Structure de la table `RoleAccess`

| Colonne         | Type           | Description                                              |
| --------------- | -------------- | -------------------------------------------------------- |
| `id_roleaccess` | `INT IDENTITY` | Identifiant unique                                       |
| `role_name`     | `VARCHAR(50)`  | Nom du rôle (`Serveur`, `Sommelier`, `Administrateur`)   |
| `password_hash` | `VARCHAR(255)` | Mot de passe chiffré                                     |
| `salt`          | `VARCHAR(255)` | Valeur aléatoire utilisée pour sécuriser le mot de passe |
| `created_at`    | `DATETIME`     | Date de création du rôle                                 |

---

```sql
CREATE TABLE RoleAccess (
  id_roleaccess INT IDENTITY(1,1) PRIMARY KEY,
  role_name NVARCHAR(50) NOT NULL,
  password_hash NVARCHAR(255) NOT NULL,
  salt NVARCHAR(255) NOT NULL,
  created_at DATETIME DEFAULT GETDATE()
);

```

## 🔐 2. Fonctionnement du système d’accès

### Étape 1 – Sélection du rôle

Au lancement de l’application, une **fenêtre de pré-connexion (`RoleAccessWindow`)** s’ouvre.
L’utilisateur choisit son rôle :

* Serveur
* Sommelier
* Administrateur

### Étape 2 – Vérification du mot de passe de rôle

L’utilisateur doit ensuite entrer le **mot de passe associé au rôle**.
Le programme vérifie le mot de passe dans la table `RoleAccess` à l’aide de la méthode :

```csharp
RoleAccess.CheckPassword(string roleName, string password);
```

Cette vérification utilise un **hachage sécurisé avec sel** (salt) défini dans la classe `PasswordHelper`.

### Étape 3 – Redirection

Une fois le rôle validé :

* Si c’est un **Administrateur**, ouverture de la fenêtre `LoginAdminWindow`
  → l’administrateur peut alors **créer de nouveaux utilisateurs** (serveurs, sommeliers, autres admins).
* Sinon, ouverture de la fenêtre `LoginWindow`
  → l’utilisateur doit se connecter avec son **email** et **mot de passe personnel**.
ici l'administrateur pourra egalement inscrire une nouvelle personne. Une fonctionnalité qui lui seul peut faire.
---

## 🧮 3. Classe `RoleAccess`

### But

Cette classe permet de gérer la lecture, l’écriture et la vérification des rôles dans la base de données.

### Principales méthodes

```csharp
public static RoleAccess GetByRole(string roleName);
```

→ Récupère un rôle depuis la base.

```csharp
public static void InsertRoleAccess(string roleName, string password);
```

→ Insère un nouveau rôle dans la table, en **hachant le mot de passe** automatiquement.

```csharp
public static bool CheckPassword(string roleName, string password);
```

→ Vérifie si le mot de passe fourni correspond à celui stocké.

### Sécurité

Les mots de passe ne sont jamais stockés en clair.
Ils sont hachés avec un **salt aléatoire** via la classe `PasswordHelper` :

```csharp
public static (string Hash, string Salt) HashPassword(string password);
public static bool VerifyPassword(string password, string storedHash, string storedSalt);
```

---

## ⚙️ 4. Initialisation automatique des rôles

(code par default pour l'instant - mise en automatique grace a la base de donnée a faire plus tard )
Lors du **démarrage de l’application**, le code suivant s’exécute dans `App.xaml.cs` :

```csharp
protected override void OnStartup(StartupEventArgs e)
{
    base.OnStartup(e);

    string connectionString = @"Server=localhost\SQLEXPRESS;Database=Cave;Trusted_Connection=True;Encrypt=False;";
    var conn = new DatabaseConnexion(connectionString);
    conn.Open();

    using (var cmd = conn.CreateCommand())
    {
        cmd.CommandText = "SELECT COUNT(*) FROM RoleAccess;";
        long count = Convert.ToInt64(cmd.ExecuteScalar());

        if (count == 0)
        {
            RoleAccess.InsertRoleAccess("Serveur", "serveur123");
            RoleAccess.InsertRoleAccess("Sommelier", "sommelier123");
            RoleAccess.InsertRoleAccess("Administrateur", "admin123");
        }
    }

    var roleAccessWindow = new RoleAccessWindow();
    roleAccessWindow.Show();
}
```

✅ Cela crée automatiquement les rôles de base **avec leurs mots de passe par défaut** si la table est vide.

---

## 💻 5. Comportement global

| Étape | Action                                                | Fenêtre concernée                    |
| ----- | ----------------------------------------------------- | ------------------------------------ |
| 1     | Lancement de l’application                            | `RoleAccessWindow`                   |
| 2     | Sélection du rôle et saisie du mot de passe de rôle   | `RoleAccessWindow`                   |
| 3     | Vérification du rôle                                  | `RoleAccessViewModel` + `RoleAccess` |
| 4     | Si rôle = Administrateur → Accès à `LoginAdminWindow` |                                      |
| 5     | Si rôle = Serveur/Sommelier → Accès à `LoginWindow`   |                                      |

---

## MDP

mot de passe:

sommelier -> sommelier123

serveur -> serveur123

administrateur -> admin123


# 🍷 Suite de la connexion 

---

## 👥 Gestion des utilisateurs

### 🔹 Table `Utilisateur` ajout du salt

| Champ               | Type    | Description                          |
| ------------------- | ------- | ------------------------------------ |
| id_utilisateur      | int     | Identifiant unique                   |
| Nom                 | varchar | Nom de l’utilisateur                 |
| Prenom              | varchar | Prénom de l’utilisateur              |
| Email               | varchar | Email unique                         |
| PasswordHash        | varchar | Mot de passe chiffré                 |
| Salt                | varchar | Clé aléatoire pour sécuriser le hash |
| id_role_utilisateur | int     | Clé étrangère vers la table `Role`   |

### 🔹 Table `Role`

| Champ   | Type    | Description                                     |
| ------- | ------- | ----------------------------------------------- |
| id_role | int     | Identifiant unique du rôle                      |
| nom     | varchar | Nom du rôle (Serveur, Sommelier, Administratif) |

---

## 🔐 Gestion des mots de passe

Les mots de passe sont **hachés et salés** avant insertion dans la base grâce à la classe `PasswordHelper`.

### Fonctionnalités :

* `HashPassword(password)` → génère un hash et un salt
* `VerifyPassword(password, storedHash, storedSalt)` → vérifie le mot de passe entré

### Exemple :

```csharp
var (hash, salt) = PasswordHelper.HashPassword("monmotdepasse");
```

---

## ⚙️ Méthodes principales

### `Utilisateur.InsertUtilisateur()`

Ajoute un utilisateur dans la base avec un mot de passe haché et un salt unique.

```csharp
Utilisateur.InsertUtilisateur("Paul", "Dupont", "paul@cave.fr", "123456", "Administratif");
```

### `Utilisateur.GetByCredentials()`

Vérifie les identifiants de connexion :

* Si `Salt` existe → utilise le hachage
* Sinon → compare directement le mot de passe (pour compatibilité temporaire)

---

✅ **Ajout de la colonne `Salt` dans la table `Utilisateur`**

* Permet de sécuriser les mots de passe hachés.

✅ **Correction du login avec mot de passe haché**

* Prend désormais en compte le salt associé.

✅ **Correction de l’insertion utilisateur**

* Récupération automatique de l’id du rôle via le nom du rôle.

✅ **Suppression des utilisateurs non sécurisés**

* Supprimé via SQL :

  ```sql
  DELETE FROM Utilisateur WHERE Salt IS NULL;
  ```

---

## 🧭 À faire plus tard

### 🔜 Fonctionnalités à implémenter

* [ ] Gestion complète des utilisateurs (CRUD)

  * Supprimer / modifier un utilisateur
  * Lister tous les utilisateurs
* [ ] Interface d’administration dédiée
* [ ] Journalisation des connexions (logs)
* [ ] Amélioration de la validation des formulaires
* [ ] Ajout d’une authentification sécurisée (JWT, si extension web)
* [ ] Gestion des vins (table `Vin`, `Bouteille`, etc.)

### 🧰 Améliorations techniques ( un petit plus pour plus tard s)

* [ ] Centraliser les chaînes de connexion dans `appsettings.json`
* [ ] Gestion des exceptions plus fine (try/catch global)
* [ ] Implémenter des tests unitaires sur le ViewModel

---
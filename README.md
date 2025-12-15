Voici le contenu formaté en Markdown. Vous n'avez plus qu'à copier le code ci-dessous et le coller dans un fichier nommé **`README.md`** ou **`INSTALL.md`**.

````markdown
# 🛠️ Guide d'Installation et de Configuration

Ce guide détaille la procédure pour installer l'application et configurer la base de données à partir des fichiers fournis.

---

## 📋 Prérequis

Avant de commencer, assurez-vous que les éléments suivants sont installés sur votre machine :

- **SQL Server Express** : [Télécharger ici](https://www.microsoft.com/sql-server/sql-server-downloads)
- **SQL Server Management Studio (SSMS)** : [Télécharger ici](https://aka.ms/ssms)
- Une instance **SQL Express** opérationnelle (généralement `.\SQLEXPRESS`)
- Le logiciel **HashTab** (pour la vérification d'intégrité)

---

## ☁️ 1. Récupération des fichiers

Accédez au dossier partagé via le lien suivant :  
🔗 **[OneDrive - Lycée Fulbert](https://lyceefulbert-my.sharepoint.com/:f:/g/personal/paul_redler_lyceefulbert_fr/EtiEjREviJFNkLEwAhEntPQBrCCU2BlnoAiEan7Hyqhv3Q?e=6ordKP)**

Téléchargez les deux fichiers suivants :
- `Setup1.zip` (Installateur de l’application)
- `cave.bak` (Sauvegarde de la base de données)

---

## 🌐 2. Configuration de la connexion SQL

L’application utilise une variable d'environnement nommée `DB_CONNECTION` pour se connecter. Vous devez la définir avant de lancer l'application.

### Option A : Authentification Windows (Par défaut)
Si vous utilisez `localhost\SQLEXPRESS` sans mot de passe spécifique (*Trusted Connection*).

Ouvrez un terminal en **Administrateur** et exécutez :
```powershell
setx DB_CONNECTION "Server=localhost\SQLEXPRESS;Database=cave;Trusted_Connection=True;Encrypt=False;" /M
````

### Option B : Authentification SQL (User/Password)

Si votre serveur nécessite un identifiant :

```powershell
setx DB_CONNECTION "Server=<VOTRE_IP>\SQLEXPRESS;Database=cave;User Id=<NOM_USER>;Password=<MOT_DE_PASSE>;Encrypt=False;" /M
```

> **Note :** L'option `/M` applique la variable au niveau système (nécessite les droits administrateur).

-----

## 🧩 3. Installation de l’application

1.  **Décompressez** le fichier `Setup1.zip`.
2.  **Exécutez** le fichier `setup1.msi` et suivez l'assistant.
3.  Une fois installé, localisez le fichier `.exe`.
4.  **Vérification de sécurité (HashTab) :**
      - Clic droit sur le `.exe` \> **Propriétés** \> Onglet **Hachages**.
      - Vérifiez que le **SHA-1** est égal à :
    > `75C923082F4CBF3A473298724068D7B7B76D844C`

-----

## 🗄️ 4. Restauration de la base de données

1.  Copiez le fichier `cave.bak` dans le dossier de backup SQL pour garantir les droits d'accès :
    `C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\Backup`
2.  Ouvrez **SQL Server Management Studio (SSMS)**.
3.  Connectez-vous à votre instance (ex: `.\SQLEXPRESS`).
4.  Clic droit sur **Bases de données** \> **Restaurer la base de données...**
5.  Sélectionnez **Périphérique** (Device), cliquez sur `...` puis **Ajouter**.
6.  Sélectionnez le fichier `cave.bak` copié précédemment.
7.  Cliquez sur **OK** pour lancer la restauration.

-----

## 🚀 5. Test de l'application

1.  Lancez l'application installée.
2.  Cliquez sur **Connexion**.
3.  Utilisez les identifiants suivants :
      - **Email :** `paul@cave.fr`
      - **Mot de passe :** `popo123`

*(Note : le mot de passe administrateur global est `admin123`)*

**Fonctionnalités :**

  - 📉 **Stats :** Cliquez sur l'image en bas à droite.
  - ⚙️ **Gestion :** Cliquez sur l'image en bas à gauche.

<!-- end list -->

```

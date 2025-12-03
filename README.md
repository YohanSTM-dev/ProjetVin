# 🛠️ Installation du Projet

Ce guide explique comment installer et configurer le projet à partir des fichiers fournis sur onedrive.  
Vous devrez récupérer un **`.zip`** ainsi qu’un fichier de base de données .

---

## 📦 Prérequis

Avant de commencer, assurez-vous d’avoir installé :

- **SQL Server Express**  
  Téléchargement : https://www.microsoft.com/sql-server/sql-server-downloads
- **SQL Server Management Studio (SSMS)**  
  Téléchargement : https://aka.ms/ssms
- Une instance **SQL Express** opérationnelle (`.\SQLEXPRESS` par défaut)

---

## ☁️ Récupération des fichiers nécessaires
https://lyceefulbert-my.sharepoint.com/:f:/g/personal/paul_redler_lyceefulbert_fr/EtiEjREviJFNkLEwAhEntPQBrCCU2BlnoAiEan7Hyqhv3Q?e=6ordKP
Téléchargez sur le cloud  les fichiers suivants :

- `Setup1.zip` — Installateur de l’application
- `cave.bak` — Sauvegarde de la base de données

---

## 🧩 Installation de l’application (`setup1.msi`)
1. Dezipez `setup1.zip`.
2. Exécutez `setup1.msi`.
3. Suivez les étapes de l’assistant d’installation.
4. Une fois installé, lancez le .exe

---

## 🗄️ Restauration de la base de données (`cave.bak`)

1. Ouvrez **SQL Server Management Studio (SSMS)**.
2. Connectez-vous à votre instance **SQL Express** (ex. : `.\SQLEXPRESS`).
3. Faites un clic droit sur **Base de données** → **Restaurer la base**
4. Sélectionnez **Support** puis ajoutez le fichier `cave.bak`.(vous devez copier le fichier dans C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\Backup pour que sqlserver le voit)
5. Cliquez sur **OK** pour lancer la restauration.

Pour essayer l'application, connectez vous en administrateur (mot de passe admin123) puis cliquez sur connexion, ici rentrer l'email paul@cave.fr et le mot de passe popo123. Vous pouvez ensuite cliquer sur les images afin en bas a droite et gauche.


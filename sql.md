```sql

Create databases cave; 

-- Table Role 
CREATE TABLE Role ( 
    id_role INT IDENTITY(1,1) PRIMARY KEY, 
    nom VARCHAR(100) NOT NULL ); 


-- Table Zone  
CREATE TABLE Zone ( 
    id_zone INT IDENTITY(1,1) PRIMARY KEY, 
    Code VARCHAR(50) NOT NULL, 
    Superficie DECIMAL(15,2), 
    id_zone_parent INT NULL, 
    FOREIGN KEY (id_zone_parent) REFERENCES Zone(id_zone) ); 


-- Table Origine 
CREATE TABLE Origine ( 
    id_origine INT IDENTITY(1,1) PRIMARY KEY, 
    Ville VARCHAR(50) NOT NULL ); 


-- Table Utilisateur 
CREATE TABLE Utilisateur ( 
    id_utilisateur INT IDENTITY(1,1) PRIMARY KEY, 
    Nom VARCHAR(50) NOT NULL, 
    Prenom VARCHAR(50) NOT NULL, 
    Email VARCHAR(100) NOT NULL UNIQUE, 
    PasswordHash VARCHAR(255) NOT NULL,
    Salt VARCHAR(255) NOT NULL  
    id_role_utilisateur INT NOT NULL, 
    FOREIGN KEY (id_role_utilisateur) REFERENCES Role(id_role) ); 


-- Table Bouteille 
CREATE TABLE Bouteille ( 
    id_bouteille INT IDENTITY(1,1) PRIMARY KEY, 
    Libelle VARCHAR(50) NOT NULL, 
    Millesime VARCHAR(50), Type VARCHAR(50), 
    Contenance DECIMAL(15,2), 
    id_origine INT NOT NULL, 
    FOREIGN KEY (id_origine) REFERENCES Origine(id_origine) ); 


-- Table Emplacement 
CREATE TABLE Emplacement ( 
    id_emplacement INT IDENTITY(1,1) PRIMARY KEY, 
    Code_Emplacement VARCHAR(50) NOT NULL, 
    Limite_Bouteille INT, 
    Qte_Bouteille INT, 
    id_zone INT NOT NULL, 
    FOREIGN KEY (id_zone) REFERENCES Zone(id_zone) ); 


-- Table HistoriqueDeplacement 
CREATE TABLE HistoriqueDeplacement ( 
    id_historique INT IDENTITY(1,1) PRIMARY KEY, 
    Date_Deplacement DATETIME NOT NULL DEFAULT GETDATE(), 
    id_utilisateur INT NOT NULL, 
    id_bouteille INT NOT NULL, 
    id_emplacement INT NOT NULL, 
    FOREIGN KEY (id_utilisateur) REFERENCES Utilisateur(id_utilisateur), 
    FOREIGN KEY (id_bouteille) REFERENCES Bouteille(id_bouteille), 
    FOREIGN KEY (id_emplacement) REFERENCES Emplacement(id_emplacement) ); 

 
-- Table TypeBouteille
CREATE TABLE TypeBouteille ( 
    id_type INT IDENTITY(1,1) PRIMARY KEY, 
    LibelleType VARCHAR(50) NOT NULL ); 


-- Table RoleAccess
CREATE TABLE RoleAccess (
  id_roleaccess INT IDENTITY(1,1) PRIMARY KEY,
  role_name NVARCHAR(50) NOT NULL,
  password_hash NVARCHAR(255) NOT NULL,
  salt NVARCHAR(255) NOT NULL,
  created_at DATETIME DEFAULT GETDATE()
);



```
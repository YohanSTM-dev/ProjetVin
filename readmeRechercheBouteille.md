📚 Documentation Complète - Gestion des Bouteilles (MVVM)
📁 Architecture Générale
Model (Données & Logique d'accès)
   ↓
Services (Communication avec la BDD)
   ↓
ViewModel (Logique métier & Filtres)
   ↓
View (Interface utilisateur)

1️⃣ MODEL LAYER - Services d'accès aux données
📄 Model/Services/IBouteilleService.cs
Rôle : Interface définissant le contrat pour les opérations sur les bouteilles
csharppublic interface IBouteilleService
{
    List<Bouteille> GetAllBouteilles();
    List<Bouteille> GetBouteillesParZone(string codeZone);
}
Pourquoi une interface ?

Permet de changer l'implémentation facilement (ex: passer d'une BDD SQL à une API)
Facilite les tests unitaires (on peut créer des "fausses" implémentations)
Respecte le principe SOLID (Dependency Inversion)

Méthodes :

GetAllBouteilles() : Récupère toutes les bouteilles de la cave
GetBouteillesParZone(string codeZone) : Récupère uniquement les bouteilles d'une zone spécifique


📄 Model/Services/BouteilleService.cs
Rôle : Implémentation concrète de l'accès aux données des bouteilles
csharppublic class BouteilleService : IBouteilleService
{
    private readonly string _connectionString;
    
    public BouteilleService()
    {
        _connectionString = @"Server=...;Database=cave;...";
    }
}
🔍 Méthode : GetAllBouteilles()
Objectif : Récupérer toutes les bouteilles avec leur dernière position
Fonctionnement :

Connexion à la BDD

csharpvar db = new DatabaseConnexion(_connectionString);
db.Open();

Requête SQL avec CTE (Common Table Expression)

sqlWITH DernierDeplacement AS (
    -- Pour chaque bouteille, on récupère son dernier déplacement
    SELECT 
        h.id_bouteille,
        h.id_emplacement,
        h.Date_Deplacement,
        ROW_NUMBER() OVER (PARTITION BY h.id_bouteille 
                          ORDER BY h.Date_Deplacement DESC) AS rn
    FROM HistoriqueDeplacement h
)
Explication de la CTE :

PARTITION BY h.id_bouteille : Groupe par bouteille
ORDER BY h.Date_Deplacement DESC : Trie du plus récent au plus ancien
ROW_NUMBER() : Numérote les lignes (1 = le plus récent)
rn = 1 signifie "le dernier déplacement"


Jointure pour récupérer toutes les infos

sqlSELECT 
    b.id_bouteille,
    b.Libelle,           -- Nom de la bouteille
    b.Millesime,         -- Année
    b.Prix,              -- Prix
    t.LibelleType AS Type,        -- Type (Rouge, Blanc, etc.)
    z.Code AS ZoneCode,           -- Code de la zone (A, B, C...)
    e.Code_Emplacement,           -- Emplacement précis
    t.PhotoURL                    -- URL de la photo
FROM Bouteille b
INNER JOIN DernierDeplacement d ON b.id_bouteille = d.id_bouteille AND d.rn = 1
INNER JOIN Emplacement e ON d.id_emplacement = e.id_emplacement
INNER JOIN Zone z ON e.id_zone = z.id_zone
INNER JOIN TypeBouteille t ON b.id_type = t.id_type

Lecture des résultats et création des objets Bouteille

csharpwhile (reader.Read())
{
    bouteilles.Add(new Bouteille
    {
        Id = reader.GetInt32(0),      // Colonne 0
        Libelle = reader.GetString(1), // Colonne 1
        Millesime = reader.GetString(2),
        Prix = reader.GetDecimal(3),
        Type = reader.GetString(4),
        Code = reader.GetString(5),
        Code_Emplacement = reader.GetString(6),
        Photo = reader.GetString(7),
    });
}
Retour : List<Bouteille> contenant toutes les bouteilles

🔍 Méthode : GetBouteillesParZone(string codeZone)
Objectif : Récupérer uniquement les bouteilles d'une zone spécifique
Différence avec GetAllBouteilles() :

Même requête SQL de base
Ajout d'un filtre WHERE z.Code = @zone
Utilisation d'un paramètre SQL pour éviter les injections SQL

csharpcommand.Parameters.AddWithValue("@zone", codeZone);
Exemple d'utilisation :
csharpvar bouteillesZoneA = _bouteilleService.GetBouteillesParZone("A");
// Retourne uniquement les bouteilles de la zone A

📄 Model/Services/IZoneService.cs
Rôle : Interface pour les opérations sur les zones
csharppublic interface IZoneService
{
    List<Zone> GetAllZones();
}
Pourquoi ? : Même principe que IBouteilleService (testabilité, flexibilité)

📄 Model/Services/ZoneService.cs
Rôle : Récupération des zones depuis la BDD
🔍 Méthode : GetAllZones()
Requête SQL simple :
sqlSELECT id_zone, Code
FROM Zone
```

**Fonctionnement** :
1. Connexion à la BDD
2. Exécution de la requête
3. Pour chaque ligne, création d'un objet `Zone`
4. Retour d'une `List<Zone>`

**Exemple de données retournées** :
```
Zone 1: Id = 1, Code = "A"
Zone 2: Id = 2, Code = "B"
Zone 3: Id = 3, Code = "C"

2️⃣ VIEWMODEL LAYER - Logique métier
📄 ViewModel/GestionBouteillePageViewModel.cs
Rôle : Orchestration entre les services (Model) et la vue (View)
🏗️ Structure de la classe
csharppublic class GestionBouteillePageViewModel : BaseViewModel
{
    // Services injectés
    private readonly IBouteilleService _bouteilleService;
    private readonly IZoneService _zoneService;
    
    // Collections observables (pour le binding XAML)
    private ObservableCollection<Bouteille> _bouteilles;
    private ObservableCollection<Zone> _zones;
    
    // Filtres actifs
    private string _filtreZone;
    private string _filtreLibelle;
    
    // Cache de toutes les bouteilles
    private List<Bouteille> _allBouteilles;
}
📊 Propriétés observables
csharppublic ObservableCollection<Bouteille> Bouteilles
{
    get => _bouteilles;
    set
    {
        _bouteilles = value;
        OnPropertyChanged(); // Notifie la vue du changement
    }
}
Pourquoi ObservableCollection ?

Notifie automatiquement la vue quand la collection change
Permet le binding bidirectionnel avec XAML
Met à jour l'interface utilisateur en temps réel


🔧 Constructeur
csharppublic GestionBouteillePageViewModel()
{
    // Création des services
    _bouteilleService = new BouteilleService();
    _zoneService = new ZoneService();

    // Chargement initial des données
    ChargerDonnees();
}
Flux d'initialisation :

Création des instances de services
Appel de ChargerDonnees()
L'interface est prête à afficher les données


📥 Méthode : ChargerDonnees()
Objectif : Charger les données initiales depuis la BDD
csharpprivate void ChargerDonnees()
{
    // 1. Charger toutes les zones (A, B, C...)
    Zones = new ObservableCollection<Zone>(_zoneService.GetAllZones());

    // 2. Charger toutes les bouteilles
    _allBouteilles = _bouteilleService.GetAllBouteilles();
    
    // 3. Afficher toutes les bouteilles
    Bouteilles = new ObservableCollection<Bouteille>(_allBouteilles);
}
```

**Ordre d'exécution** :
```
Appel du service → Récupération BDD → Stockage en cache → Affichage dans la vue

🔄 Méthode : RechargerBouteilles()
Objectif : Rafraîchir les données depuis la BDD
csharppublic void RechargerBouteilles()
{
    // Réinitialiser les filtres
    _filtreZone = null;
    _filtreLibelle = null;
    
    // Recharger depuis la BDD
    ChargerDonnees();
}
Cas d'utilisation :

Bouton "Recharger" cliqué
Après ajout/suppression d'une bouteille
Pour avoir les données les plus récentes


🔍 Méthode : FiltrerParZone(string codeZone)
Objectif : Filtrer l'affichage par zone
csharppublic void FiltrerParZone(string codeZone)
{
    _filtreZone = codeZone;  // Stocker le filtre actif
    AppliquerFiltres();       // Appliquer tous les filtres
}
Exemple :
csharp// L'utilisateur clique sur la zone "A"
FiltrerParZone("A");
// → Affiche uniquement les bouteilles de la zone A

🔍 Méthode : FiltrerParLibelle(string libelle)
Objectif : Filtrer l'affichage par nom de bouteille
csharppublic void FiltrerParLibelle(string libelle)
{
    _filtreLibelle = libelle;  // Stocker le texte de recherche
    AppliquerFiltres();         // Appliquer tous les filtres
}
Exemple :
csharp// L'utilisateur tape "Château"
FiltrerParLibelle("Château");
// → Affiche uniquement les bouteilles contenant "Château" dans le nom

🎯 Méthode : AppliquerFiltres() (LA PLUS IMPORTANTE)
Objectif : Combiner tous les filtres actifs
csharpprivate void AppliquerFiltres()
{
    // ÉTAPE 1 : Filtrer par ZONE (ou prendre toutes les bouteilles)
    var bouteillesFiltrees = string.IsNullOrWhiteSpace(_filtreZone)
        ? _allBouteilles  // Pas de filtre zone → toutes les bouteilles
        : _bouteilleService.GetBouteillesParZone(_filtreZone); // Filtre zone actif

    // ÉTAPE 2 : Filtrer par LIBELLÉ (si un texte est saisi)
    if (!string.IsNullOrWhiteSpace(_filtreLibelle))
    {
        bouteillesFiltrees = bouteillesFiltrees
            .Where(b => b.Libelle.ToLower().Contains(_filtreLibelle.ToLower()))
            .ToList();
    }

    // ÉTAPE 3 : Mettre à jour l'affichage
    Bouteilles = new ObservableCollection<Bouteille>(bouteillesFiltrees);
}
Scénarios possibles :
Filtre ZoneFiltre LibelléRésultat❌ Aucun❌ AucunToutes les bouteilles✅ "A"❌ AucunBouteilles de la zone A❌ Aucun✅ "Château"Bouteilles contenant "Château"✅ "A"✅ "Château"Bouteilles de la zone A contenant "Château"
Exemple concret :
csharp// Utilisateur sélectionne zone "A"
FiltrerParZone("A");
// → Affiche : Château Margaux (A), Bordeaux Rouge (A)

// Puis il tape "Château" dans la recherche
FiltrerParLibelle("Château");
// → Affiche : Château Margaux (A) uniquement
// (combine les deux filtres !)

🔄 Méthode : ReinitialiserFiltres()
Objectif : Revenir à l'affichage initial (toutes les bouteilles)
csharppublic void ReinitialiserFiltres()
{
    _filtreZone = null;
    _filtreLibelle = null;
    Bouteilles = new ObservableCollection<Bouteille>(_allBouteilles);
}
Utilisation : Bouton "Réinitialiser les filtres"

3️⃣ VIEW LAYER - Interface utilisateur
📄 View/GestionBouteillePage.xaml.cs (Code-behind)
Rôle : Liaison entre l'interface XAML et le ViewModel
csharppublic partial class GestionBouteillePage : Page
{
    private GestionBouteillePageViewModel _viewModel;

    public GestionBouteillePage()
    {
        InitializeComponent();
        _viewModel = new GestionBouteillePageViewModel();
        this.DataContext = _viewModel; // ← Liaison avec le ViewModel
    }
}
DataContext : Permet au XAML d'accéder aux propriétés du ViewModel

🎬 Gestionnaires d'événements
1. Bouton Recharger
csharpprivate void Recharger_Click(object sender, RoutedEventArgs e)
{
    _viewModel.RechargerBouteilles();
    ZoneComboBox.SelectedIndex = -1;  // Réinitialiser la ComboBox
    LibelleTextBox.Text = string.Empty; // Vider le TextBox
}
```

**Flux** :
```
Clic bouton → Appel ViewModel → Rechargement BDD → Mise à jour UI

2. Sélection d'une zone (clic sur Border)
csharpprivate void Zone_Click(object sender, MouseButtonEventArgs e)
{
    if (sender is Border border && border.DataContext is Zone zone)
    {
        _viewModel.FiltrerParZone(zone.Code);
    }
}
Explication :

sender is Border : Vérifie que c'est bien un Border qui a été cliqué
border.DataContext is Zone : Récupère la zone associée au Border
Appel du filtre avec le code de la zone


3. Saisie dans le TextBox de recherche
csharpprivate void LibelleTextBox_TextChanged(object sender, TextChangedEventArgs e)
{
    _viewModel.FiltrerParLibelle(LibelleTextBox.Text);
}
Comportement :

Se déclenche à chaque caractère tapé
Filtre en temps réel
Combine avec le filtre de zone si actif


4. Bouton Réinitialiser
csharpprivate void ResetButton_Click(object sender, RoutedEventArgs e)
{
    _viewModel.ReinitialiserFiltres();
    LibelleTextBox.Text = string.Empty;
}
```

---

## 🔄 Flux Complet d'Utilisation

### Scénario 1 : Ouverture de la page
```
1. new GestionBouteillePage()
   ↓
2. new GestionBouteillePageViewModel()
   ↓
3. ChargerDonnees()
   ↓
4. _zoneService.GetAllZones() → BDD
   ↓
5. _bouteilleService.GetAllBouteilles() → BDD
   ↓
6. Zones et Bouteilles affichées dans l'UI
```

---

### Scénario 2 : Filtrage combiné
```
1. Utilisateur clique sur zone "A"
   ↓
2. Zone_Click() → FiltrerParZone("A")
   ↓
3. AppliquerFiltres()
   ↓
4. _bouteilleService.GetBouteillesParZone("A") → BDD
   ↓
5. Affichage : 50 bouteilles de la zone A

6. Utilisateur tape "Château" dans la recherche
   ↓
7. LibelleTextBox_TextChanged() → FiltrerParLibelle("Château")
   ↓
8. AppliquerFiltres()
   ↓
9. Filtre en mémoire (LINQ) sur les 50 bouteilles
   ↓
10. Affichage : 5 bouteilles de la zone A contenant "Château"

🎯 Avantages de cette Architecture
✅ Séparation des responsabilités

Services : Gèrent uniquement l'accès aux données
ViewModel : Gère uniquement la logique métier
View : Gère uniquement l'affichage

✅ Testabilité
csharp// On peut tester le ViewModel sans BDD
var fakeService = new FakeBouteilleService();
var viewModel = new GestionBouteillePageViewModel(fakeService);
✅ Réutilisabilité
csharp// Les services peuvent être utilisés ailleurs
var service = new BouteilleService();
var bouteilles = service.GetAllBouteilles(); // Utilisable partout
✅ Maintenabilité

Changer de BDD → Modifier uniquement les Services
Changer la logique de filtre → Modifier uniquement le ViewModel
Changer l'UI → Modifier uniquement la View


📌 Points Clés à Retenir

Services (Model) = "Comment récupérer les données ?"
ViewModel = "Quelles données afficher et comment les filtrer ?"
View = "Comment les présenter visuellement ?"
ObservableCollection = Mise à jour automatique de l'UI
DataContext = Pont entre XAML et ViewModel
Filtres cumulatifs = Chaque filtre s'ajoute aux précédents

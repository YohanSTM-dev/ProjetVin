# 📚 Gestion des Bouteilles (MVVM)

Cette documentation explique l’architecture et le fonctionnement d’une application de gestion de bouteilles basée sur le pattern **MVVM**.

---

## 📁 Architecture Générale

```
Model (Données & Logique d'accès)
   ↓
Services (Communication avec la BDD)
   ↓
ViewModel (Logique métier & Filtres)
   ↓
View (Interface utilisateur)
```

---

## 1️⃣ Model Layer – Services d’accès aux données

### **IBouteilleService.cs**
**Rôle** : Interface définissant le contrat pour les opérations sur les bouteilles.

```csharp
public interface IBouteilleService
{
    List<Bouteille> GetAllBouteilles();
    List<Bouteille> GetBouteillesParZone(string codeZone);
}
```

**Pourquoi une interface ?**
- Permet de changer l’implémentation facilement (ex : BDD SQL → API)
- Facilite les tests unitaires (fausses implémentations possibles)
- Respecte le principe SOLID (Dependency Inversion)

**Méthodes :**
- `GetAllBouteilles()` : récupère toutes les bouteilles de la cave.
- `GetBouteillesParZone(string codeZone)` : récupère les bouteilles d’une zone spécifique.

---

### **BouteilleService.cs**
**Rôle** : Implémentation concrète de l’accès aux données.

```csharp
public class BouteilleService : IBouteilleService
{
    private readonly string _connectionString;

    public BouteilleService()
    {
        _connectionString = @"Server=...;Database=cave;...";
    }
}
```

#### 🔍 Méthode : `GetAllBouteilles()`
**Objectif** : récupérer toutes les bouteilles avec leur dernière position.

**Fonctionnement :**
1. Connexion à la BDD.
2. Requête SQL avec **CTE** pour récupérer le dernier déplacement de chaque bouteille.
3. Jointures avec `Emplacement`, `Zone` et `TypeBouteille`.
4. Lecture des résultats → création de la `List<Bouteille>`.

#### 🔍 Méthode : `GetBouteillesParZone(string codeZone)`
- Même requête que `GetAllBouteilles()` avec un filtre `WHERE z.Code = @zone`.
- Paramètre SQL pour éviter les injections.

```csharp
var bouteillesZoneA = _bouteilleService.GetBouteillesParZone("A");
```

---

### **IZoneService.cs**
**Rôle** : Interface pour les opérations sur les zones.

```csharp
public interface IZoneService
{
    List<Zone> GetAllZones();
}
```

### **ZoneService.cs**
**Rôle** : Récupération des zones depuis la BDD.

```sql
SELECT id_zone, Code FROM Zone
```

**Exemple de données retournées :**
```
Zone 1: Id = 1, Code = "A"
Zone 2: Id = 2, Code = "B"
Zone 3: Id = 3, Code = "C"
```

---

## 2️⃣ ViewModel Layer – Logique métier

### **GestionBouteillePageViewModel.cs**
**Rôle** : Orchestration entre services et interface utilisateur.

```csharp
public class GestionBouteillePageViewModel : BaseViewModel
{
    private readonly IBouteilleService _bouteilleService;
    private readonly IZoneService _zoneService;

    private ObservableCollection<Bouteille> _bouteilles;
    private ObservableCollection<Zone> _zones;

    private string _filtreZone;
    private string _filtreLibelle;

    private List<Bouteille> _allBouteilles;
}
```

**Pourquoi `ObservableCollection` ?**
- Mise à jour automatique de l’UI lors des changements.
- Permet le binding bidirectionnel avec XAML.

---

### 🔧 Constructeur

```csharp
public GestionBouteillePageViewModel()
{
    _bouteilleService = new BouteilleService();
    _zoneService = new ZoneService();

    ChargerDonnees();
}
```

**Flux d’initialisation :**
1. Création des services.
2. Chargement des données depuis la BDD.
3. Affichage initial dans la vue.

---

### Méthodes clés

#### **ChargerDonnees()**
Charge toutes les zones et bouteilles.

```csharp
Zones = new ObservableCollection<Zone>(_zoneService.GetAllZones());
_allBouteilles = _bouteilleService.GetAllBouteilles();
Bouteilles = new ObservableCollection<Bouteille>(_allBouteilles);
```

#### **RechargerBouteilles()**
Rafraîchit les données depuis la BDD et réinitialise les filtres.

#### **FiltrerParZone(string codeZone)**
Filtre l’affichage par zone.

#### **FiltrerParLibelle(string libelle)**
Filtre l’affichage par nom de bouteille.

#### **AppliquerFiltres()**
Combine tous les filtres actifs et met à jour l’UI.

```csharp
Bouteilles = new ObservableCollection<Bouteille>(bouteillesFiltrees);
```

#### **ReinitialiserFiltres()**
Retourne à l’affichage initial de toutes les bouteilles.

---

## 3️⃣ View Layer – Interface utilisateur

### **GestionBouteillePage.xaml.cs**
**Rôle** : Liaison entre XAML et ViewModel.

```csharp
public partial class GestionBouteillePage : Page
{
    private GestionBouteillePageViewModel _viewModel;

    public GestionBouteillePage()
    {
        InitializeComponent();
        _viewModel = new GestionBouteillePageViewModel();
        this.DataContext = _viewModel;
    }
}
```

**Gestionnaires d’événements :**
1. Bouton Recharger
2. Sélection d’une zone
3. Saisie dans le TextBox de recherche
4. Bouton Réinitialiser

---

## 🔄 Flux Complet d’Utilisation

### Scénario 1 : Ouverture de la page
```
new GestionBouteillePage()
   ↓
new GestionBouteillePageViewModel()
   ↓
ChargerDonnees() → Zones + Bouteilles depuis BDD
   ↓
Affichage dans l’UI
```

### Scénario 2 : Filtrage combiné
```
1. Clique sur zone "A" → FiltrerParZone("A")
2. Tape "Château" → FiltrerParLibelle("Château")
3. AppliquerFiltres() → Affiche bouteilles de la zone A contenant "Château"
```

---

## 🎯 Avantages de cette architecture

- **Séparation des responsabilités** :  Services → accès aux données, ViewModel → logique métier, View → affichage
- **Testabilité** : possibilité de mocker les services pour tester le ViewModel.
- **Réutilisabilité** : les services peuvent être utilisés ailleurs.
- **Maintenabilité** : changer la BDD, les filtres ou l’UI sans impacter les autres couches.

---

## 📌 Points clés à retenir

- **Services (Model)** : "Comment récupérer les données ?"
- **ViewModel** : "Quelles données afficher et comment les filtrer ?"
- **View** : "Comment présenter les données visuellement ?"
- **ObservableCollection** : mise à jour automatique de l’UI
- **DataContext** : pont entre XAML et ViewModel
- **Filtres cumulatifs** : chaque filtre s’ajoute aux précédents


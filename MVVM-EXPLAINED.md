# Understanding MVVM in Your Stock Broker Project

## 🤔 Why Put Code in ViewModels Instead of Code-Behind?

### The Problem with Code-Behind
When you put logic directly in `.xaml.cs` files (code-behind), you create tight coupling between your UI and logic:

```csharp
// ❌ BAD: Logic in code-behind (MainWindow.xaml.cs)
private void ShowOverview_Click(object sender, RoutedEventArgs e)
{
    MainContent.Content = new Overview();
}

private void ShowStockInfo_Click(object sender, RoutedEventArgs e)
{
    MainContent.Content = new StockInfo();
}
```

**Problems:**
- Can't test without creating UI
- Hard to reuse logic
- Mixes UI concerns with business logic
- Difficult to maintain as app grows

### The MVVM Solution
With MVVM, logic goes in ViewModels:

```csharp
// ✅ GOOD: Logic in ViewModel (MainViewModel.cs)
public ICommand ShowOverviewCommand { get; }

public MainViewModel()
{
    ShowOverviewCommand = new RelayCommand(_ => CurrentViewModel = OverviewVM);
}
```

**Benefits:**
- ✅ Testable without UI
- ✅ Reusable across different Views
- ✅ Clear separation of concerns
- ✅ Easy to maintain and extend

## 📐 The MVVM Architecture

```
┌─────────────────────────────────────────────────────────┐
│                         VIEW                             │
│                   (XAML Files)                           │
│  ┌──────────────────────────────────────────────────┐  │
│  │  MainWindow.xaml                                  │  │
│  │  Overview.xaml                                    │  │
│  │  StockInfo.xaml                                   │  │
│  │                                                    │  │
│  │  • Only UI markup                                 │  │
│  │  • Bindings to ViewModel properties               │  │
│  │  • No logic (minimal .xaml.cs)                    │  │
│  └──────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────┘
                            ↕ Data Binding
┌─────────────────────────────────────────────────────────┐
│                      VIEWMODEL                           │
│                  (ViewModel Classes)                     │
│  ┌──────────────────────────────────────────────────┐  │
│  │  MainViewModel.cs                                 │  │
│  │  OverviewViewModel.cs                             │  │
│  │  StockInfoViewModel.cs                            │  │
│  │                                                    │  │
│  │  • Properties (bound to View)                     │  │
│  │  • Commands (bound to buttons)                    │  │
│  │  • Navigation logic                               │  │
│  │  • Business logic                                 │  │
│  │  • State management                               │  │
│  │  • INotifyPropertyChanged                         │  │
│  └──────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────┘
                            ↕
┌─────────────────────────────────────────────────────────┐
│                        MODEL                             │
│                   (Data Classes)                         │
│  ┌──────────────────────────────────────────────────┐  │
│  │  Stock.cs                                         │  │
│  │                                                    │  │
│  │  • Data structures                                │  │
│  │  • Business entities                              │  │
│  │  • No UI concerns                                 │  │
│  └──────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────┘
```

## 🔍 How Your Project Uses MVVM

### 1. View Layer (XAML Files)

**MainWindow.xaml** - Just UI markup:
```xml
<Button Content="Dashboard" Command="{Binding ShowOverviewCommand}"/>
```
- Binds to `ShowOverviewCommand` in ViewModel
- No Click events!

**MainWindow.xaml.cs** - Almost empty:
```csharp
public MainWindow()
{
    InitializeComponent();
    DataContext = new MainViewModel();  // Only sets DataContext
}
```

### 2. ViewModel Layer

**MainViewModel.cs** - Contains all logic:
```csharp
public class MainViewModel : INotifyPropertyChanged
{
    // Property that View binds to
    private object? _currentViewModel;
    public object? CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            if (_currentViewModel != value)
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));  // Notify UI
            }
        }
    }

    // Command that button binds to
    public ICommand ShowOverviewCommand { get; }

    public MainViewModel()
    {
        OverviewVM = new OverviewViewModel();
        CurrentViewModel = OverviewVM;
        
        // Define what command does
        ShowOverviewCommand = new RelayCommand(_ => CurrentViewModel = OverviewVM);
    }
}
```

### 3. Model Layer

**Stock.cs** - Pure data:
```csharp
public sealed class Stock
{
    public string Symbol { get; init; }
    public string Name { get; init; }
    public decimal Price { get; init; }
    // ... just data, no logic
}
```

## 🔄 Data Flow Example

Let's trace what happens when you click "Dashboard":

```
1. USER ACTION:
   User clicks "Dashboard" button in MainWindow.xaml

2. VIEW → VIEWMODEL:
   Button is bound to {Binding ShowOverviewCommand}
   
3. VIEWMODEL EXECUTES:
   MainViewModel.ShowOverviewCommand.Execute() is called
   
4. VIEWMODEL UPDATES STATE:
   Command sets: CurrentViewModel = OverviewVM
   
5. PROPERTY CHANGE NOTIFICATION:
   OnPropertyChanged(nameof(CurrentViewModel)) fires
   
6. VIEWMODEL → VIEW:
   Binding system detects change
   
7. VIEW UPDATES:
   ContentControl shows new view automatically
   DataTemplate maps OverviewViewModel → Overview.xaml
   
8. RESULT:
   User sees Markets view, all without code-behind!
```

## 🎯 Key Concepts

### INotifyPropertyChanged
```csharp
public event PropertyChangedEventHandler? PropertyChanged;
protected void OnPropertyChanged(string propertyName)
    => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
```
- Tells UI when a property changes
- UI automatically updates via binding
- Essential for MVVM

### Commands (RelayCommand)
```csharp
public ICommand ShowOverviewCommand { get; }
```
- Encapsulates user actions
- Can be bound to buttons
- Can have CanExecute logic
- Better than Click events

### Data Binding
```xml
{Binding PropertyName}
{Binding Path=PropertyName}
{Binding CommandName}
```
- Connects View to ViewModel
- Two-way synchronization
- No manual UI updates needed

### DataTemplates
```xml
<DataTemplate DataType="{x:Type vm:OverviewViewModel}">
    <v:Overview/>
</DataTemplate>
```
- Automatically maps ViewModels to Views
- No manual view switching
- Clean separation

## 📋 Comparison: Code-Behind vs MVVM

### Scenario: Navigate to Overview

**❌ Code-Behind Way (Don't do this):**
```csharp
// MainWindow.xaml
<Button Content="Overview" Click="ShowOverview_Click"/>

// MainWindow.xaml.cs
private void ShowOverview_Click(object sender, RoutedEventArgs e)
{
    MainContent.Content = new Overview();
}
```
Problems: Can't test, hard to maintain, tightly coupled

**✅ MVVM Way (Do this):**
```xml
<!-- MainWindow.xaml -->
<Button Content="Overview" Command="{Binding ShowOverviewCommand}"/>
```

```csharp
// MainViewModel.cs
public ICommand ShowOverviewCommand { get; }

public MainViewModel()
{
    ShowOverviewCommand = new RelayCommand(_ => CurrentViewModel = OverviewVM);
}
```
Benefits: Testable, maintainable, loosely coupled

## 🎓 Quick Rules

### ✅ DO Put in ViewModel:
- Navigation logic
- Button command handlers
- Data manipulation
- Validation
- State management
- Business rules

### ❌ DON'T Put in Code-Behind:
- Navigation
- Data processing
- Business logic
- Command handlers
- Anything testable

### ✅ Code-Behind CAN Have:
- `InitializeComponent()`
- Setting `DataContext`
- View-specific animations
- Focus management
- That's it!

## 🚀 Your Project's Flow

```
App.xaml
  ↓ (starts)
MainWindow.xaml
  ↓ (sets DataContext)
MainViewModel.cs
  ↓ (creates)
OverviewViewModel.cs
  ↓ (has data)
Stock.cs models
  ↓ (displayed in)
Overview.xaml
  ↓ (via DataTemplate in)
App.xaml
```

## 💡 Remember

1. **View = What user sees** (XAML)
2. **ViewModel = How it behaves** (Commands, Properties, Logic)
3. **Model = What data looks like** (Stock class)

Keep these separate, and your code will be:
- Easy to test
- Easy to maintain
- Easy to extend
- Easy to understand

The extra setup is worth it! 🎉

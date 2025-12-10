# Stock Broker WPF Application

This is a modern WPF application following the MVVM (Model-View-ViewModel) pattern, styled to match your Figma design.

## 📁 Project Structure

```
StockBrokerProject/
├── Models/
│   └── Stock.cs                    # Data model for stocks
├── ViewModels/
│   ├── MainViewModel.cs            # Main navigation & state management
│   ├── OverviewViewModel.cs        # Markets view logic
│   └── StockInfoViewModel.cs       # Individual stock details logic
├── Views/
│   ├── MainWindow.xaml/.cs         # Main window with navigation bar
│   ├── Overview.xaml/.cs           # Markets/stock list view
│   └── StockInfo.xaml/.cs          # Individual stock detail view
├── Converters/
│   └── ChangeToColorConverter.cs   # Converts +/- values to green/red
├── App.xaml/.cs                    # Application entry & global styles
├── RelayCommand.cs                 # Command implementation for MVVM
├── AssemblyInfo.cs                 # Assembly metadata
└── StockBrokerProject.csproj       # Project file
```

## 🎯 MVVM Pattern Explained

### Why Code Goes in ViewModels, Not Code-Behind

**The MVVM Pattern Separates:**
- **View (XAML)**: UI layout and appearance only
- **ViewModel**: Logic, commands, properties, navigation
- **Model**: Data structures (Stock class)

**Benefits:**
1. ✅ **Testable**: ViewModels can be unit tested without UI
2. ✅ **Reusable**: Same ViewModel can power different Views
3. ✅ **Maintainable**: Clear separation of concerns
4. ✅ **Designer-Friendly**: Designers work on XAML, developers on ViewModels
5. ✅ **Data Binding**: WPF binding system works seamlessly with ViewModels

### Code-Behind (.xaml.cs) Should Only:
- Set the DataContext
- Handle view-specific animations
- **NOT contain business logic or navigation**

### ViewModel Should Contain:
- Properties that Views bind to
- Commands that buttons invoke
- Navigation logic
- Business logic
- State management

## 🎨 Design System (from Figma)

### Colors
- **Primary Blue**: `#4A90E2`
- **Positive Green**: `#00C853`
- **Negative Red**: `#E53935`
- **Background Gray**: `#F5F7FA`
- **Card White**: `#FFFFFF`
- **Text Primary**: `#2C3E50`
- **Text Secondary**: `#7F8C8D`

### Styles (defined in App.xaml)
- `NavButtonStyle` - Navigation buttons in header
- `ActionButtonStyle` - Green action buttons (Trade)
- `ViewButtonStyle` - Gray view buttons
- `CardStyle` - White cards with shadow

## 🔄 How Navigation Works

### The Flow:
```
User clicks button → Command in ViewModel executes → 
CurrentViewModel property changes → 
DataTemplate automatically shows correct View
```

### Example:
1. User clicks "Markets" button in MainWindow.xaml
2. Button is bound to `ShowOverviewCommand`
3. Command executes in MainViewModel.cs
4. Sets `CurrentViewModel = OverviewVM`
5. DataTemplate in App.xaml automatically displays Overview.xaml
6. No manual view switching needed!

## 🔑 Key Components

### MainViewModel.cs
- Manages navigation between views
- Holds references to all ViewModels
- Tracks which view is currently active
- Commands: `ShowOverviewCommand`, `ShowStockInfoCommand`

### OverviewViewModel.cs (Markets View)
- Manages collection of stocks
- Tracks selected stock
- Provides data for the stock market table

### StockInfoViewModel.cs (Stock Detail View)
- Displays individual stock information
- Shows price, change, sector, details
- Contains trading panel data

### MainWindow.xaml
- Top navigation bar with logo and buttons
- ContentControl that displays current view
- Styled to match Figma design

### Overview.xaml (Markets View)
- Stock market table with DataGrid
- Search bar (placeholder)
- View and Trade buttons for each stock
- Color-coded price changes (green/red)

### StockInfo.xaml (Stock Detail View)
- Stock header with price and change
- Price chart placeholder
- Company information tabs
- Trade panel on right side
- Your position card

## 🎮 How to Use

### Running the Application
1. Open `StockBrokerProject.sln` in Visual Studio
2. Build and run (F5)
3. Application opens showing Markets view

### Navigation
- Click **Dashboard/Markets** buttons in top bar to switch views
- Click **View** button on any stock to see details
- All navigation handled by ViewModels via Commands

### Adding New Stocks
Edit `OverviewViewModel.cs` constructor to add more sample stocks:

```csharp
new Stock("SYMBOL", "Company Name", price, change, changePercent, 
          "Sector", "Market Cap", "Volume", "Description")
```

## 🔧 Customization

### Changing Colors
Edit color brushes in `App.xaml`:
```xml
<SolidColorBrush x:Key="PrimaryBlue" Color="#YourColor"/>
```

### Adding New Views
1. Create new View (XAML + .xaml.cs)
2. Create new ViewModel
3. Add DataTemplate in App.xaml:
```xml
<DataTemplate DataType="{x:Type vm:YourViewModel}">
    <v:YourView/>
</DataTemplate>
```
4. Add navigation command in MainViewModel

### Modifying Styles
All button and card styles are in `App.xaml` under `<Application.Resources>`

## 📊 Current Features

✅ Markets view with stock table
✅ Individual stock detail view
✅ Price change color coding (green/red)
✅ Navigation between views
✅ Figma-inspired modern design
✅ MVVM architecture
✅ Command-based navigation
✅ Data binding throughout

## 🚀 Future Enhancements

- [ ] Real-time stock data (API integration)
- [ ] Actual chart rendering (use LiveCharts or similar)
- [ ] Search functionality
- [ ] Trade execution
- [ ] Portfolio management
- [ ] User authentication
- [ ] Database persistence

## 💡 Learning Resources

- **MVVM Pattern**: https://learn.microsoft.com/en-us/dotnet/desktop/wpf/
- **WPF Data Binding**: https://learn.microsoft.com/en-us/dotnet/desktop/wpf/data/
- **Commands in WPF**: https://learn.microsoft.com/en-us/dotnet/desktop/wpf/advanced/commanding-overview

## ❓ FAQ

**Q: Why don't I see Click events in XAML?**
A: We use Command binding instead - it's the MVVM way and keeps logic in ViewModels.

**Q: Where is the navigation code?**
A: In `MainViewModel.cs` - commands set the `CurrentViewModel` property.

**Q: How does the app know which View to show?**
A: DataTemplates in `App.xaml` automatically map ViewModels to Views.

**Q: Can I add more pages?**
A: Yes! Create ViewModel + View, add DataTemplate, add command to MainViewModel.

## 📝 Notes

- This project uses .NET 8 with nullable reference types enabled
- All views use data binding - no code-behind logic
- Color converter handles automatic green/red for +/- values
- Sample stock data is hardcoded in OverviewViewModel
- Project follows WPF best practices and MVVM pattern

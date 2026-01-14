using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using StockBrokerProject.Models;
using StockBrokerProject.Services;

namespace StockBrokerProject.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private readonly Portfolio _portfolio;
        private readonly DataService _dataService;
        private readonly MainViewModel _mainViewModel;

        public ICommand ExportPortfolioCommand { get; }
        public ICommand ImportPortfolioCommand { get; }
        public ICommand ExportPricesCommand { get; }
        public ICommand ImportPricesCommand { get; }
        public ICommand ExportTransactionsCommand { get; }
        public ICommand ImportTransactionsCommand { get; }
        public ICommand ExportAllCommand { get; }
        public ICommand ImportAllCommand { get; }

        public SettingsViewModel(Portfolio portfolio, DataService dataService, MainViewModel mainViewModel)
        {
            _portfolio = portfolio;
            _dataService = dataService;
            _mainViewModel = mainViewModel;

            ExportPortfolioCommand = new RelayCommand(_ => ExportPortfolio());
            ImportPortfolioCommand = new RelayCommand(_ => ImportPortfolio());
            ExportPricesCommand = new RelayCommand(_ => ExportPrices());
            ImportPricesCommand = new RelayCommand(_ => ImportPrices());
            ExportTransactionsCommand = new RelayCommand(_ => ExportTransactions());
            ImportTransactionsCommand = new RelayCommand(_ => ImportTransactions());
            ExportAllCommand = new RelayCommand(_ => ExportAll());
            ImportAllCommand = new RelayCommand(_ => ImportAll());
        }

        private void ExportPortfolio()
        {
            var dialog = new SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                FileName = $"portfolio_{DateTime.Now:yyyyMMdd_HHmmss}.json",
                Title = "Export Portfolio"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    File.Copy("portfolio.json", dialog.FileName, true);
                    MessageBox.Show(
                        $"Portfolio exported successfully to:\n{dialog.FileName}",
                        "Export Successful",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Failed to export portfolio:\n{ex.Message}",
                        "Export Failed",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
            }
        }

        private void ImportPortfolio()
        {
            var result = MessageBox.Show(
                "Importing a portfolio will overwrite your current portfolio data. Continue?",
                "Confirm Import",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (result != MessageBoxResult.Yes)
                return;

            var dialog = new OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                Title = "Import Portfolio"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    File.Copy(dialog.FileName, "portfolio.json", true);

                    MessageBox.Show(
                        "Portfolio imported successfully! Please restart the application for changes to take effect.",
                        "Import Successful",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Failed to import portfolio:\n{ex.Message}",
                        "Import Failed",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
            }
        }

        private void ExportPrices()
        {
            var dialog = new SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                FileName = $"prices_{DateTime.Now:yyyyMMdd_HHmmss}.json",
                Title = "Export Stock Prices"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    File.Copy("prices.json", dialog.FileName, true);
                    MessageBox.Show(
                        $"Stock prices exported successfully to:\n{dialog.FileName}",
                        "Export Successful",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Failed to export prices:\n{ex.Message}",
                        "Export Failed",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
            }
        }

        private void ImportPrices()
        {
            var result = MessageBox.Show(
                "Importing prices will overwrite current stock prices. Continue?",
                "Confirm Import",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (result != MessageBoxResult.Yes)
                return;

            var dialog = new OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                Title = "Import Stock Prices"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    File.Copy(dialog.FileName, "prices.json", true);

                    MessageBox.Show(
                        "Stock prices imported successfully! Please restart the application for changes to take effect.",
                        "Import Successful",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Failed to import prices:\n{ex.Message}",
                        "Import Failed",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
            }
        }

        private void ExportTransactions()
        {
            var dialog = new SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                FileName = $"transactions_{DateTime.Now:yyyyMMdd_HHmmss}.json",
                Title = "Export Transaction History"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    File.Copy("transactions.json", dialog.FileName, true);
                    MessageBox.Show(
                        $"Transaction history exported successfully to:\n{dialog.FileName}",
                        "Export Successful",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Failed to export transactions:\n{ex.Message}",
                        "Export Failed",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
            }
        }

        private void ImportTransactions()
        {
            var result = MessageBox.Show(
                "Importing transactions will overwrite your current transaction history. Continue?",
                "Confirm Import",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (result != MessageBoxResult.Yes)
                return;

            var dialog = new OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                Title = "Import Transaction History"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    File.Copy(dialog.FileName, "transactions.json", true);

                    MessageBox.Show(
                        "Transaction history imported successfully! Please restart the application for changes to take effect.",
                        "Import Successful",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Failed to import transactions:\n{ex.Message}",
                        "Import Failed",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
            }
        }

        private void ExportAll()
        {
            var dialog = new SaveFileDialog
            {
                Filter = "ZIP files (*.zip)|*.zip|All files (*.*)|*.*",
                FileName = $"stockbroker_backup_{DateTime.Now:yyyyMMdd_HHmmss}.zip",
                Title = "Export All Data"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    string tempDir = Path.Combine(Path.GetTempPath(), "StockBrokerBackup_" + Guid.NewGuid());
                    Directory.CreateDirectory(tempDir);

                    if (File.Exists("portfolio.json"))
                        File.Copy("portfolio.json", Path.Combine(tempDir, "portfolio.json"));
                    if (File.Exists("prices.json"))
                        File.Copy("prices.json", Path.Combine(tempDir, "prices.json"));
                    if (File.Exists("transactions.json"))
                        File.Copy("transactions.json", Path.Combine(tempDir, "transactions.json"));

                    System.IO.Compression.ZipFile.CreateFromDirectory(tempDir, dialog.FileName);

                    Directory.Delete(tempDir, true);

                    MessageBox.Show(
                        $"All data exported successfully to:\n{dialog.FileName}\n\nThis backup contains:\n• Portfolio data\n• Stock prices\n• Transaction history",
                        "Export Successful",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Failed to export data:\n{ex.Message}",
                        "Export Failed",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
            }
        }

        private void ImportAll()
        {
            var result = MessageBox.Show(
                "Importing a backup will overwrite ALL your current data including:\n• Portfolio\n• Stock prices\n• Transaction history\n\nContinue?",
                "Confirm Import",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (result != MessageBoxResult.Yes)
                return;

            var dialog = new OpenFileDialog
            {
                Filter = "ZIP files (*.zip)|*.zip|All files (*.*)|*.*",
                Title = "Import Backup"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    string tempDir = Path.Combine(Path.GetTempPath(), "StockBrokerRestore_" + Guid.NewGuid());
                    Directory.CreateDirectory(tempDir);

                    System.IO.Compression.ZipFile.ExtractToDirectory(dialog.FileName, tempDir);

                    string portfolioFile = Path.Combine(tempDir, "portfolio.json");
                    string pricesFile = Path.Combine(tempDir, "prices.json");
                    string transactionsFile = Path.Combine(tempDir, "transactions.json");

                    if (File.Exists(portfolioFile))
                        File.Copy(portfolioFile, "portfolio.json", true);
                    if (File.Exists(pricesFile))
                        File.Copy(pricesFile, "prices.json", true);
                    if (File.Exists(transactionsFile))
                        File.Copy(transactionsFile, "transactions.json", true);

                    Directory.Delete(tempDir, true);

                    MessageBox.Show(
                        "All data imported successfully! Please restart the application for changes to take effect.",
                        "Import Successful",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Failed to import backup:\n{ex.Message}",
                        "Import Failed",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
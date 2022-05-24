using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using СadastralBlockChain.Components;
using СadastralBlockChain.Models;
using Block = СadastralBlockChain.Models.Block;

namespace СadastralBlockChain
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string _cred;
        private readonly string _filePath;
        private List<Block> BlockChain;

        private BlockData _currentBlockData;
        private readonly string _credHashed;

        public MainWindow(string cred, string filePath)
        {
            _cred = cred;
            _credHashed = Convert.ToBase64String(SHA512.HashData(Encoding.UTF8.GetBytes(_cred)));
            _filePath = filePath;
            InitializeComponent();

            BlockChain = GetData(filePath)
                ?? throw new ApplicationException();

            _currentBlockData = BlockChain.Last().Data;

            LoadList();
        }

        List<Block>? GetData(string path)
        {
            return JsonConvert.DeserializeObject<List<Block>>(File.ReadAllText(path));
        }

        void LoadList()
        {
            BlockList.ItemsSource = BlockChain.Select(x => new BlockListModel
            {
                CreatedAt = x.CreatedOn,
                IsNew = false,
                Block = x.Data,
                Cred = _cred
            }).Append(new BlockListModel
            {
                IsNew = true,
                Cred = _credHashed
            });
        }

        void UpdateBlock(BlockData data, string cred, bool isReadOnly)
        {
            LandPanel.LoadPanel(data, cred, isReadOnly);
        }

        private void LandPanel_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void BlockList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ListBox)?.SelectedItem is not BlockListModel selected)
                return;

            WriteBlock.IsEnabled = selected.IsNew;
            UpdateBlock(selected.IsNew ? _currentBlockData : selected.Block, selected.Cred, !selected.IsNew);
        }

        private void WriteBlock_Click(object sender, RoutedEventArgs e)
        {
            BlockChain.Add(new Block(BlockChain.Last(), _currentBlockData, _cred));
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(BlockChain));

            LoadList();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(BlockChain));
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            LandPanel.UpdateWorkPanel();
        }

        private void ValidateBlocks_Click(object sender, RoutedEventArgs e)
        {
            var blocksValidation = BlockChain
                .Select(x => $"{x.CreatedOn}: {(x.Validate(_cred) ? "Валидный" : "Битый")}");

            MessageBox.Show(string.Join(Environment.NewLine, blocksValidation), "Валидация", MessageBoxButton.OK);
        }
    }
}

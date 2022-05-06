using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Windows;
using СadastralBlockChain.Models;

namespace СadastralBlockChain
{
    /// <summary>
    /// Логика взаимодействия для OpenOrNewDialog.xaml
    /// </summary>
    public partial class OpenOrNewDialog : Window
    {
        public string Credential => CredentialText.Text;


        public OpenOrNewDialog()
        {
            InitializeComponent();
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                new MainWindow(Credential, openFileDialog.FileName).Show();
                Close();
            }
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            var x = int.Parse(XText.Text);
            var y = int.Parse(YText.Text);

            var saveFileDialog = new SaveFileDialog();

            if (saveFileDialog.ShowDialog() == true)
            {
                var obj = new List<Models.Block>()
                {
                    new(new BlockData
                    {
                        Width = x,
                        Height = y,
                        Model = new LandBlockModel(y, x),
                    }, Credential)
                };

                File.Delete(saveFileDialog.FileName);
                using var fileStream = File.Open(saveFileDialog.FileName, FileMode.OpenOrCreate);
                new DataContractSerializer(typeof(List<Block>))
                    .WriteObject(fileStream, obj);
                fileStream.Close();



                new MainWindow(Credential, saveFileDialog.FileName).Show();
                Close();
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}

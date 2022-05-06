using System.Windows;
using System.Windows.Controls;
using СadastralBlockChain.Models;

namespace СadastralBlockChain
{
    /// <summary>
    /// Логика взаимодействия для EditLand.xaml
    /// </summary>
    public partial class EditLand : Window
    {
        private readonly LandBlockModel _parent;
        private bool isVertical = true;

        public EditLand(LandBlockModel parent)
        {
            _parent = parent;
            InitializeComponent();

            ParentSize.Text = _parent.Height.ToString();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (!isVertical)
            {
                isVertical = true;
                ParentSize.Text = _parent.Height.ToString();
                StackPanel.Orientation = Orientation.Vertical;
            }
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            if (isVertical)
            {
                isVertical = false;
                ParentSize.Text = _parent.Width.ToString();
                StackPanel.Orientation = Orientation.Horizontal;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var firstLen = int.Parse(FirstLenTextBox.Text);
            var secondLen = int.Parse(SecondLenTextBox.Text);

            if (firstLen + secondLen != (isVertical ? _parent.Height : _parent.Width))
                return;

            _parent.SetChilden(new[]
            {
                new LandBlockModel()
                {
                    X = 0,
                    Y = 0,
                    Width = isVertical ? _parent.Width : firstLen,
                    Height = !isVertical ? _parent.Height : firstLen,
                },
                new LandBlockModel()
                {
                    X = isVertical ? 0 : firstLen,
                    Y = !isVertical ? 0 : firstLen,
                    Width = isVertical ? _parent.Width : secondLen,
                    Height = !isVertical ? _parent.Height : secondLen,
                }
            });

            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _parent.LandType = LandType.Rent;
            Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            _parent.LandType = LandType.Sent;
            Close();
        }
    }
}

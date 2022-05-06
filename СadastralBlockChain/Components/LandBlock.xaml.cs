using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using СadastralBlockChain.Models;

namespace СadastralBlockChain.Components
{
    /// <summary>
    /// Логика взаимодействия для LandBlock.xaml
    /// </summary>
    public partial class LandBlock : UserControl
    {
        private readonly LandBlockModel _block;
        private readonly Ratio _ratio;
        private readonly bool _isReadOnly;

        public LandBlock(LandBlockModel block, Ratio ratio, bool isReadOnly)
        {
            DataContext = _block = block;
            _ratio = ratio;
            _isReadOnly = isReadOnly;
            InitializeComponent();

            Height = _block.Height * ratio.Y;
            Width = _block.Width * ratio.X;
            Margin = new Thickness(0)
            {
                Left = _block.X * ratio.X,
                Top = _block.Y * ratio.Y,
            };
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;

            UpdateView();
        }

        private void UpdateView()
        {
            holderRect.Fill = _block.LandType switch
            {
                LandType.None => new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                LandType.Rent => new SolidColorBrush(Color.FromRgb(144, 202, 249)),
                LandType.Sent => new SolidColorBrush(Color.FromRgb(239, 154, 154)),
                _ => throw new ArgumentOutOfRangeException()
            };

            var children = _block.Children?
                .Select(x => new LandBlock(x, _ratio, _isReadOnly))
                .ToList();

            if (children?.Any() != true)
            {
                holder.Visibility = Visibility.Visible;
                return;
            }
            else
                holder.Visibility = Visibility.Collapsed;

            foreach (var landBlock in children)
            {
                grid.Children.Add(landBlock);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateView();
        }

        private void holder_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_block.LandType == LandType.None && !_isReadOnly)
            {
                new EditLand(_block).ShowDialog();
                UpdateView();
            }
        }
    }
}

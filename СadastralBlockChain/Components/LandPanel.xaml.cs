using System.Windows.Controls;
using СadastralBlockChain.Models;

namespace СadastralBlockChain.Components
{
    /// <summary>
    /// Логика взаимодействия для LandPanel.xaml
    /// </summary>
    public partial class LandPanel : UserControl
    {
        private BlockData _block;
        private bool _isReadOnly;

        public LandPanel()
        {
            InitializeComponent();
        }

        public void LoadPanel(BlockData block, bool isReadOnly)
        {
            _block = block;
            _isReadOnly = isReadOnly;
            UpdateWorkPanel();
        }

        public void UpdateWorkPanel()
        {
            if (_block is null)
                return;

            var ratio = new Ratio
            {
                X = ActualWidth / _block.Width,
                Y = ActualHeight / _block.Height,
            };
            grid.Children.Clear();
            grid.Children.Add(new LandBlock(_block.Model, ratio, _isReadOnly));
        }
    }
}

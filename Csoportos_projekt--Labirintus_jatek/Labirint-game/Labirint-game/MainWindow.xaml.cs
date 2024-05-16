using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Labirint_game
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		
		private void settingsBtn_Click(object sender, RoutedEventArgs e)
		{
			Settings settings = new Settings();
			this.Hide();
			settings.Show();
		}

		private void startBtn_Click(object sender, RoutedEventArgs e)
		{
			Game game = new Game();
			this.Hide();
			game.Show();
		}
	}
}
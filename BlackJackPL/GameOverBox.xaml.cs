// Author: Hedda Eriksson
// Date: 2023-10-17
// Malmö University, Bachelor of Engineering - Computer Science & Telecommunications
// Description: When a round is finished this a user control with results will show. There is also alternatives to continue the game or to stop.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BlackJackPL
{
    /// <summary>
    /// Interaction logic for GameOver.xaml
    /// </summary>
    public partial class GameOverBox : UserControl
    {
        public EventHandler ContinueClicked;
        public EventHandler StopClicked;
        public GameOverBox()
        {
            InitializeComponent();
        }

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            ContinueClicked?.Invoke(this, EventArgs.Empty);
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            StopClicked?.Invoke(this, EventArgs.Empty);
        }
        public void SetWinnersList(List<string> winners)
        {
            Grid.SetColumn(winnersListBox, 4);
            Grid.SetRow(winnersListBox, 6);
            winnersListBox.ItemsSource = winners;
        }
    }
}

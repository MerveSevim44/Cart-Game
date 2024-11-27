using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
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

namespace TestProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Game game = new Game();
            game.Show();
            this.Close();
            InitializeComponent();
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string UserName = UserNameTextBox.Text;
            string UserId = UserIDTextBox.Text;

            if(!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(UserId))
            {
                Game game = new Game();
                game.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("inccorect enty");
            }


            
        }
    }
}

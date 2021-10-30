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

namespace AsyncAxis2Client
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

        private void BtnGetContent_Click(object sender, RoutedEventArgs e)
        {
            string fileName = txtFileName.Text;
            lblBlocked.Content = "Searching and unblocked";
            // lblBlocked.Content = "Searching and blocked";
            txtFileContent.Text = "";
            try
            {
                ServiceReference1.TextFileContentRetrieverPortTypeClient client =
                    new ServiceReference1.TextFileContentRetrieverPortTypeClient();
                Task<ServiceReference1.retrieveTextFileContentResponse> response = client.retrieveTextFileContentAsync(fileName);
                response.ContinueWith((t) => asyncMethodCompleted(t));
                // txtFileContent.Text = client.retrieveTextFileContent(fileName);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                lblBlocked.Content = "";
            }
            /*finally
            {
                lblBlocked.Content = "";
            } */

        }

        public void asyncMethodCompleted(Task<ServiceReference1.retrieveTextFileContentResponse> task)
        {
            string result = task.Result.@return;
            this.Dispatcher.Invoke(() =>
            { txtFileContent.Text = result; lblBlocked.Content = ""; });
        }

        private void BtnDoSomething_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Do something else ...");
        }
    }
}

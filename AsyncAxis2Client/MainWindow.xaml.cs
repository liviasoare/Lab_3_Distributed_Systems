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
using System.Timers;

namespace AsyncAxis2Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {  
        private ServiceReference2.TextFileContentRetrieverPortTypeClient client;
        private Timer GetContentTimer = new Timer(1000);
        private int Seconds = 0;
        

        public MainWindow()
        {
            InitializeComponent();
        }

        public void GetResultAndUpdateLabels(IAsyncResult result)
        {
            this.Dispatcher.Invoke(() =>
            {
                txtFileContent.Text = client.EndretrieveTextFileContent(result);
                lblBlocked.Content = "";
                GetContentTimer.Stop();
            });
        }

        private void UpdateLabel(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                lblBlocked.Content = "Seconds elapsed: " + Seconds;
                Seconds += 1;
            });
        }

        private void BtnGetContent_Click(object sender, RoutedEventArgs e)
        {
            string fileName = txtFileName.Text;
            //lblBlocked.Content = "Searching and unblocked";
            // lblBlocked.Content = "Searching and blocked";
            //txtFileContent.Text = "";


            /*try
            {
                ServiceReference1.TextFileContentRetrieverPortTypeClient client =
                    new ServiceReference1.TextFileContentRetrieverPortTypeClient();
                client.retrieveTextFileContentCompleted += asyncMethodCompleted;
                client.retrieveTextFileContentAsync(fileName);

                 Task<ServiceReference1.retrieveTextFileContentResponse> response = client.retrieveTextFileContentAsync(fileName);
                response.ContinueWith((t) => asyncMethodCompleted(t)); 
                 txtFileContent.Text = client.retrieveTextFileContent(fileName);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                lblBlocked.Content = "";
            }
            finally
            {
                lblBlocked.Content = "";
            } */

            try
            {
                client = new
                ServiceReference2.TextFileContentRetrieverPortTypeClient();
                GetContentTimer.Elapsed += UpdateLabel;
                GetContentTimer.Start();
                IAsyncResult result = client.BeginretrieveTextFileContent(fileName, new AsyncCallback(GetResultAndUpdateLabels), null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                lblBlocked.Content = "";
            }
            finally
            {
                Seconds = 0;
                lblBlocked.Content = "";
            }
        }

        /* public void asyncMethodCompleted(Task<ServiceReference1.retrieveTextFileContentResponse> task)
        {
            string result = task.Result.@return;
            this.Dispatcher.Invoke(() =>
            { txtFileContent.Text = result; lblBlocked.Content = ""; });
        } */

         /*   public void asyncMethodCompleted(object sender, ServiceReference1.retrieveTextFileContentCompletedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            { txtFileContent.Text = e.Result; lblBlocked.Content = ""; });
        } */

        private void BtnDoSomething_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Do something else ...");
        }
    }
}

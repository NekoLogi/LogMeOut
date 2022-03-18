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

namespace LogMeOut
{
    public partial class MainWindow : Window
    {
        public static DateTime SHUTDOWN_DATE { get; set; }
        private static bool IS_RUNNING { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }


        private int[] GetValues()
        {
            try
            {
                var values = new List<int>
                {
                    int.Parse(Combobox_Days.SelectedItem.ToString()),
                    int.Parse(Combobox_Hours.SelectedItem.ToString()),
                    int.Parse(Combobox_Minutes.SelectedItem.ToString())
                };
                return values.ToArray();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return null;
        }

        private void CreateComboItems()
        {
            for (int i = 0; i < 30 + 1; i++)
                Combobox_Days.Items.Add(i.ToString());
            for (int i = 0; i < 24 + 1; i++)
                Combobox_Hours.Items.Add(i.ToString());
            for (int i = 0; i < 60 + 1; i++)
                Combobox_Minutes.Items.Add(i.ToString());

            Combobox_Days.SelectedIndex = 0;
            Combobox_Hours.SelectedIndex = 0;
            Combobox_Minutes.SelectedIndex = 0;
        }

        private void ChangeDate()
        {
            if (IS_RUNNING)
            {
                DateTime d1 = DateTime.Now;
                DateTime d2 = DateTime.Now;

                if (Combobox_Days.SelectedItem.ToString() != "0")
                {
                    d2 = d1.AddDays(double.Parse(Combobox_Days.SelectedItem.ToString()));
                    d1 = d2;
                }
                if (Combobox_Hours.SelectedItem.ToString() != "0")
                {
                    d2 = d1.AddHours(double.Parse(Combobox_Hours.SelectedItem.ToString()));
                    d1 = d2;
                }
                if (Combobox_Minutes.SelectedItem.ToString() != "0")
                {
                    d2 = d1.AddMinutes(double.Parse(Combobox_Minutes.SelectedItem.ToString()));
                }
                SHUTDOWN_DATE = d2;
                Text_Datetime.Text = SHUTDOWN_DATE.ToString();
            }
        }



        #region UI
        private void Button_Start_Click(object sender, RoutedEventArgs e)
        {
            if (GetValues() == null)
                return;

            string message;
            int[] values = GetValues();

            if ((bool)Checkbox_Restart.IsChecked)
                message = Timer.RunTimer(values[0], values[1], values[2], Timer.Mode.Restart);
            else
                message = Timer.RunTimer(values[0], values[1], values[2], Timer.Mode.Shutdown);

            MessageBox.Show(message);
        }

        private void OnComboBoxChange(object sender, SelectionChangedEventArgs e)
        {
            ChangeDate();
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            string message = Timer.RunTimer(0, 0, 0, Timer.Mode.Cancel);
            MessageBox.Show(message);
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            CreateComboItems();
            IS_RUNNING = true;
            ChangeDate();
        }

        #endregion
    }
}

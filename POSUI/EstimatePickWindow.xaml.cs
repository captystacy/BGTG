using Microsoft.Win32;
using System.Windows;

namespace POSUI
{
    public partial class EstimatePickWindow : Window
    {
        private readonly CalendarPlanWindow _calendarPlanWindow;
        public string[] EstimatesPaths;

        public EstimatePickWindow(CalendarPlanWindow calendarPlanWindow)
        {
            InitializeComponent();
            _calendarPlanWindow = calendarPlanWindow;
        }

        private void estimatePickButton_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();

            fileDialog.Filter = "Сметы (*.xls;*.xlsx)|*.xls;*.xlsx";
            fileDialog.Multiselect = true;

            if (fileDialog.ShowDialog() == true && fileDialog.FileNames.Length <= 2)
            {
                EstimatesPaths = fileDialog.FileNames;
                Close();
                _calendarPlanWindow.ShowDialog();
            }
        }
    }
}

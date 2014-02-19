using System;
using System.Collections.Generic;
using System.Linq;
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

namespace DataOperations
{
    /// <summary>
    /// Interaction logic for DragNDrop.xaml
    /// </summary>
    public partial class DragNDrop : UserControl
    {
        private List<Object> _LocalResource;
        private string _labelHeader;
        public List<Object> LocalDataSource { 
            get
            {
                return _LocalResource;        
            }
            set
            {
                _LocalResource = value;
                BindData();
            } 
        }

        public string SelectedString
        {
            get
            {
                return _labelHeader;
            }
            set
            {
                _labelHeader = value;
                lblHeader.Content = _labelHeader;
            }
        }
        public Point StartingPOint;
        public bool BindData()
        {
            try
            {
                grdList.DataContext = this._LocalResource;
            }
            catch (Exception ex)
            {

                Utility.WriteLogError("Exception Occurred while Binding Data to Popup"+ex.ToString());
                return false;
            }
            return true;
        }
        public DragNDrop()
        {
            InitializeComponent();

        }

        private void DragInitiated(object sender, MouseButtonEventArgs e)
        {
            StartingPOint = e.GetPosition(null);
        }

        private void DragStarted(object sender, MouseEventArgs e)
        {
             Point mousePos = e.GetPosition(null);
             Vector diff = StartingPOint - mousePos;
            if (e.LeftButton == MouseButtonState.Pressed &&
                Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance )
            {
                SelectedString = (grdList.SelectedValue==null)?string.Empty:grdList.SelectedValue.ToString();
                Utility.WriteLogDebug(string.Format("Drag and Drop initiated with {0} as selected item",SelectedString));
            }
        }
    }
}

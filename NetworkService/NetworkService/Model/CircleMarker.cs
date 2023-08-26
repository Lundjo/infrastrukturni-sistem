using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace NetworkService.Model
{
    public class CircleMarker : BindableBase
    {
        private int cmId;
        private double cmValue;
        private string cmDate;
        private string cmTime;
        private Thickness cmMargin;
        private Brush cmColor;

        public CircleMarker()
        {

        }

        public CircleMarker(int cmId, double cmValue, string cmDate, string cmTime)
        {
            CmId = cmId;
            CmValue = cmValue;
            CmDate = cmDate;
            CmTime = cmTime;
        }

        public int CmId
        {
            get
            {
                return cmId;
            }
            set
            {
                cmId = value;
                OnPropertyChanged("CmId");
            }
        }

        public double CmValue
        {
            get { return cmValue; }
            set
            {
                cmValue = value;
                CmMargin = new Thickness(0, 0, 0, cmValue * 40);
                CmColor = (cmValue < 0.34 || cmValue > 2.73) ? Brushes.Red : Brushes.CadetBlue;
                OnPropertyChanged("CmValue");
            }
        }

        public string CmDate
        {
            get { return cmDate; }
            set
            {
                cmDate = value;
                OnPropertyChanged("CmDate");
            }
        }

        public string CmTime
        {
            get { return cmTime; }
            set
            {
                cmTime = value;
                OnPropertyChanged("CmTime");
            }
        }

        public Thickness CmMargin
        {
            get { return cmMargin; }
            set
            {
                cmMargin = value;
                OnPropertyChanged("CmMargin");
            }
        }

        public Brush CmColor
        {
            get { return cmColor; }
            set
            {
                cmColor = value;
                OnPropertyChanged("CmColor");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace NetworkService.Model
{
    public class Unos : BindableBase
    {
        private Brush bojaId;
        private Brush bojaNaziv;
        private Brush bojaFilter;

        public Brush BojaId
        {
            get { return bojaId; }
            set
            {
                if (bojaId != value)
                {
                    bojaId = value;
                    OnPropertyChanged("BojaId");
                }
            }
        }

        public Brush BojaNaziv
        {
            get { return bojaNaziv; }
            set
            {
                if (bojaNaziv != value)
                {
                    bojaNaziv = value;
                    OnPropertyChanged("BojaNaziv");
                }
            }
        }

        public Brush BojaFilter
        {
            get { return bojaFilter; }
            set
            {
                if (bojaFilter != value)
                {
                    bojaFilter = value;
                    OnPropertyChanged("BojaFilter");
                }
            }
        }
    }
}

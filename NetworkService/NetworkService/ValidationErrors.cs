using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace NetworkService
{
	public class ValidationErrors : BindableBase
	{
		private readonly Dictionary<string, SolidColorBrush> validationErrors = new Dictionary<string, SolidColorBrush>();

		public bool IsValid
		{
			get
			{
				return this.validationErrors.Count < 1;
			}
		}

		public SolidColorBrush this[string fieldName]
		{
			get
			{
				return this.validationErrors.ContainsKey(fieldName) ?
					this.validationErrors[fieldName] : Brushes.White;
			}



			set
			{
				if (this.validationErrors.ContainsKey(fieldName))
				{
					this.validationErrors.Remove(fieldName);
					this.validationErrors[fieldName] = value;
				}
				else
				{
					this.validationErrors.Add(fieldName, value);
				}
				this.OnPropertyChanged("IsValid");
			}
		}

		public void Clear()
		{
			validationErrors.Clear();
		}
	}
}

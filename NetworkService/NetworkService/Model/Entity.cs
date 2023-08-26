using NetworkService.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace NetworkService.Model
{
	public class Entity : ValidationBase
	{
		private string textId;

		private int id;
		private string name;
		private EntityType type;
		private double value;

		public int Id
		{
			get { return id; }
			set
			{
				if (id != value)
				{
					id = value;
					OnPropertyChanged("Id");
				}
			}
		}

		public string TextId
		{
			get { return textId; }
			set
			{
				if (textId != value)
				{
					textId = value;
					OnPropertyChanged("TextId");
				}
			}
		}

		public string Name
		{
			get { return name; }
			set
			{
				if (name != value)
				{
					name = value;
					OnPropertyChanged("Name");
				}
			}
		}

		public EntityType Type
		{
			get { return type; }
			set
			{
				if (type != value)
				{
					type = value;
					Type.Name = value.Name;
					Type.ImgSrc = value.ImgSrc;
					OnPropertyChanged("Type");
				}
			}
		}

		public double Value
		{
			get { return this.value; }
			set
			{
				if (this.value != value)
				{
					this.value = value;
					OnPropertyChanged("Value");
				}
			}
		}

		public bool IsValueValid()
		{
			if(Value < 0.34 || Value > 2.73)
			{
				return false;
			}

			return true;
		}

		protected override void ValidateSelf()
		{
			int tempId;
			bool parsingSuccess = int.TryParse(this.textId, out tempId);

			if (this.DoesIdAlreadyExist || !parsingSuccess || tempId < 0 || string.IsNullOrWhiteSpace(this.textId))
			{
				this.ValidationErrors["Id"] = Brushes.Red;
				
			}
			if (string.IsNullOrWhiteSpace(this.name) || this.name.Equals("Naziv"))
			{
				this.ValidationErrors["Name"] = Brushes.Red;
			}
		}
	}
}

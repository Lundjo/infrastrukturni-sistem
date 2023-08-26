using NetworkService.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NetworkService.ViewModel
{
	public class NetworkEntitiesViewModel : BindableBase
	{
		public List<string> ComboBoxItems { get; set; } = Data.ComboBoxItemsData.entityTypes.Keys.ToList();

		public ObservableCollection<Entity> EntitiesToShow { get; set; }
		public ObservableCollection<Entity> Entities { get; set; }
		public ObservableCollection<Entity> FilteredEntities { get; set; }

		public MyICommand AddEntityCommand { get; set; }
		public MyICommand DeleteEntityCommand { get; set; }
		public MyICommand FilterEntityCommand { get; set; }
		public MyICommand LostFocusIdCommand { get; set; }
        public MyICommand LostFocusNazivCommand { get; set; }
        public MyICommand LostFocusFilterCommand { get; set; }
        public MyICommand GotFocusIdCommand { get; set; }
		public MyICommand GotFocusNazivCommand { get; set; }
        public MyICommand GotFocusFilterCommand { get; set; }

		private Entity currentEntity = new Entity();
		private EntityType currentEntityType = new EntityType();

		private Entity selectedEntity;

		private string selectedEntityTypeToFilter;
		private bool isGreaterThanRadioButtonSelected;
		private bool isLessThanRadioButtonSelected;
		private bool isEqualsThanRadioButtonSelected;
		private string selectedIdMarginToFilterText;
		private string filterErrorMessage;

        private Unos unos = new Unos();
		

		public NetworkEntitiesViewModel()
		{
			Entities = new ObservableCollection<Entity>();
			EntitiesToShow = Entities;
			CurrentEntityType.Name = ComboBoxItems[0];
			selectedEntityTypeToFilter = ComboBoxItems[0];
			currentEntity.TextId = "Id";
			currentEntity.Name = "Naziv";
			SelectedIdMarginToFilterText = "Id";

            unos.BojaId = Brushes.Gray;
			unos.BojaNaziv = Brushes.Gray;
			unos.BojaFilter = Brushes.Gray;


            AddEntityCommand = new MyICommand(OnAdd);
			DeleteEntityCommand = new MyICommand(OnDelete, CanDelete);
			FilterEntityCommand = new MyICommand(OnFilter);
			LostFocusIdCommand = new MyICommand(OnLostFocusId);
			GotFocusIdCommand = new MyICommand(OnGotFocusId);
            LostFocusNazivCommand = new MyICommand(OnLostFocusNaziv);
            GotFocusNazivCommand = new MyICommand(OnGotFocusNaziv);
            LostFocusFilterCommand = new MyICommand(OnLostFocusFilter);
            GotFocusFilterCommand = new MyICommand(OnGotFocusFilter);
        }

		public void OnLostFocusId()
		{
			if (currentEntity.TextId.Trim().Equals(string.Empty))
			{
				currentEntity.TextId = "Id";
                unos.BojaId = Brushes.Gray;
            }
		}

		public void OnGotFocusId()
		{
			if (currentEntity.TextId.Trim().Equals("Id"))
			{
                currentEntity.TextId = "";
                unos.BojaId = Brushes.Black;
            }
		}

        public void OnLostFocusNaziv()
        {
            if (currentEntity.Name.Trim().Equals(string.Empty))
            {
                currentEntity.Name = "Naziv";
                unos.BojaNaziv = Brushes.Gray;
            }
        }

        public void OnGotFocusNaziv()
        {
            if (currentEntity.Name.Trim().Equals("Naziv"))
            {
                currentEntity.Name = "";
                unos.BojaNaziv = Brushes.Black;
            }
        }

        public void OnLostFocusFilter()
        {
            if (SelectedIdMarginToFilterText.Trim().Equals(string.Empty))
            {
                SelectedIdMarginToFilterText = "Id";
                unos.BojaFilter = Brushes.Gray;
            }
        }

        public void OnGotFocusFilter()
        {
            if (SelectedIdMarginToFilterText.Equals("Id"))
            {
                SelectedIdMarginToFilterText = string.Empty;
                unos.BojaFilter = Brushes.Black;
            }
        }

        public Entity SelectedEntity
		{
			get { return selectedEntity; }
			set
			{
				selectedEntity = value;
				DeleteEntityCommand.RaiseCanExecuteChanged();
			}
		}

		public string SelectedEntityTypeToFilter
		{
			get { return selectedEntityTypeToFilter; }
			set
			{
				selectedEntityTypeToFilter = value;
				OnPropertyChanged("SelectedEntityTypeToFilter");
			}
		}

		public bool IsGreaterThanRadioButtonSelected
		{
			get { return isGreaterThanRadioButtonSelected; }
			set
			{
				isGreaterThanRadioButtonSelected = value;
				OnPropertyChanged("IsGreaterThanRadioButtonSelected");
			}
		}

		public bool IsLessThanRadioButtonSelected
		{
			get { return isLessThanRadioButtonSelected; }
			set
			{
				isLessThanRadioButtonSelected = value;
				OnPropertyChanged("IsLessThanRadioButtonSelected");
			}
		}

		public bool IsEqualsThanRadioButtonSelected
		{
			get { return isEqualsThanRadioButtonSelected; }
			set
			{
				isEqualsThanRadioButtonSelected = value;
				OnPropertyChanged("IsEqualsThanRadioButtonSelected");
			}
		}

		public string SelectedIdMarginToFilterText
		{
			get { return selectedIdMarginToFilterText; }
			set
			{
				selectedIdMarginToFilterText = value;
				OnPropertyChanged("SelectedIdMarginToFilterText");
			}
		}
		public string FilterErrorMessage
		{
			get { return filterErrorMessage; }
			set
			{
				filterErrorMessage = value;
				OnPropertyChanged("FilterErrorMessage");
			}
		}

		private void OnFilter()
		{
			if (SelectedEntityTypeToFilter == null &&
				!IsGreaterThanRadioButtonSelected &&
				!IsLessThanRadioButtonSelected &&
				!IsEqualsThanRadioButtonSelected &&
				string.IsNullOrWhiteSpace(SelectedIdMarginToFilterText))
			{
				return;
			}

			List<Entity> filteredList = new List<Entity>();

			foreach (Entity entity in Entities)
			{
				filteredList.Add(entity);
			}

			if (SelectedEntityTypeToFilter != null)
			{
				List<Entity> entitiesToDelete = new List<Entity>();
				for (int i = 0; i < filteredList.Count; i++)
				{
					if (filteredList[i].Type.Name != SelectedEntityTypeToFilter)
					{
						entitiesToDelete.Add(filteredList[i]);
					}
				}

				foreach (Entity entity in entitiesToDelete)
				{
					filteredList.Remove(entity);
				}
			}

			if (IsGreaterThanRadioButtonSelected)
			{
				List<Entity> entitiesToDelete = new List<Entity>();

				int selectedId;
				bool parsingSuccessful = int.TryParse(SelectedIdMarginToFilterText, out selectedId);

				if (parsingSuccessful)
				{
					if (selectedId >= 0)
					{
						for (int i = 0; i < filteredList.Count; i++)
						{
							if (filteredList[i].Id <= selectedId)
							{
								entitiesToDelete.Add(filteredList[i]);
							}
						}

						foreach (Entity entity in entitiesToDelete)
						{
							filteredList.Remove(entity);
						}
					}
				}
			}
			else if (IsLessThanRadioButtonSelected)
			{
				List<Entity> entitiesToDelete = new List<Entity>();
					
				int selectedId;
				bool parsingSuccessful = int.TryParse(SelectedIdMarginToFilterText, out selectedId);

				if (parsingSuccessful)
				{
					if (selectedId >= 0)
					{

						for (int i = 0; i < filteredList.Count; i++)
						{
							if (filteredList[i].Id >= selectedId)
							{
								entitiesToDelete.Add(filteredList[i]);
							}
						}

						foreach (Entity entity in entitiesToDelete)
						{
							filteredList.Remove(entity);
						}
					}
				}
			}
			else if (IsEqualsThanRadioButtonSelected)
			{

				List<Entity> entitiesToDelete = new List<Entity>();

				int selectedId;
				bool parsingSuccessful = int.TryParse(SelectedIdMarginToFilterText, out selectedId);

				if (parsingSuccessful)
				{
					if (selectedId >= 0)
					{

						for (int i = 0; i < filteredList.Count; i++)
						{
							if (filteredList[i].Id > selectedId)
							{
								entitiesToDelete.Add(filteredList[i]);
							}
							else if (filteredList[i].Id < selectedId)
							{
								entitiesToDelete.Add(filteredList[i]);
							}
						}

						foreach (Entity entity in entitiesToDelete)
						{
							filteredList.Remove(entity);
						}
					}
				}
			}

            FilteredEntities = new ObservableCollection<Entity>();

            foreach (Entity entity in filteredList)
            {
                FilteredEntities.Add(entity);
            }

            EntitiesToShow = FilteredEntities;
            OnPropertyChanged("EntitiesToShow");
        }

		private bool CanDelete()
		{
			return SelectedEntity != null;
		}

		private void OnDelete()
		{
			Entities.Remove(SelectedEntity);
		}

		public Entity CurrentEntity
		{
			get { return currentEntity; }
			set
			{
				currentEntity = value;
				OnPropertyChanged("CurrentEntity");
			}
		}

		public EntityType CurrentEntityType
		{
			get { return currentEntityType; }
			set
			{
				currentEntityType = value;
				OnPropertyChanged("CurrentEntityType");
			}
		}

		public void OnAdd()
		{
			int parsedId;
			bool canParse = int.TryParse(CurrentEntity.TextId, out parsedId);
			bool idAlreadyExists = false;

			if (canParse)
			{
				foreach (Entity entity in Entities)
				{
					if (entity.Id == parsedId)
					{
						idAlreadyExists = true;
						break;
					}
				}
			}

			CurrentEntity.DoesIdAlreadyExist = idAlreadyExists;
			CurrentEntity.Validate();
			CurrentEntityType.Validate();

			if (CurrentEntity.IsValid)
			{
				Entities.Add(new Entity()
				{
					Id = int.Parse(CurrentEntity.TextId),
					Name = CurrentEntity.Name,
					Type = new EntityType
					{
						Name = CurrentEntityType.Name,
						ImgSrc = CurrentEntityType.ImgSrc
					},
					Value = 0
				});
			}
		}

        public Unos Unos
        {
            get { return unos; }
            set
            {
                unos = value;
                OnPropertyChanged("Unos");
            }
        }
    }
}

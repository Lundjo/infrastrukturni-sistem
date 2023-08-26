using NetworkService.Model;
using NetworkService.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NetworkService.ViewModel
{
    public class TerminalViewModel : BindableBase
    {
        public ObservableCollection<Entity> Entities { get; set; }

        public MyICommand KeyDownCommand { get; set; }

        private string selectedEntityTypeToFilter;

        private string terminalString;

        public ObservableCollection<Entity> FilteredEntities { get; set; }

        public ObservableCollection<Entity> EntitiesToShow { get; set; }

        public TerminalViewModel() 
        {
            Entities = new ObservableCollection<Entity>();

            KeyDownCommand = new MyICommand(OnKeyDown);

            TerminalString = "terminal:~$ ";
        }

        public void OnKeyDown()
        {
            if (Keyboard.IsKeyDown(Key.Enter))
            {
                string[] delovi = TerminalString.Split(' ');

                int parsedId;
                bool canParse = int.TryParse(delovi[2], out parsedId);

                if (delovi[1].ToLower().Equals("add"))
                {
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
                    if (!idAlreadyExists)
                    {
                        if (delovi[4].ToLower().Equals("interval") || delovi[4].ToLower().Equals("smart"))
                        {
                            Entities.Add(new Entity()
                            {
                                Id = int.Parse(delovi[2]),
                                Name = delovi[3],
                                Type = new EntityType
                                {
                                    Name = delovi[4],
                                    //ImgSrc = "pack://application:,,,/NetworkService;component/Images/SmartMeter.png"
                                },
                                Value = 0
                            });
                        }
                    }
                }
                else if (delovi[1].ToLower().Equals("del"))
                {
                    foreach (Entity entity in Entities)
                    {
                        if (entity.Id == parsedId)
                        {
                            Entities.Remove(entity);
                            break;
                        }
                    }
                }
                else if (delovi[1].ToLower().Equals("fil"))
                {
                    List<Entity> filteredList = new List<Entity>();

                    foreach (Entity entity in Entities)
                    {
                        filteredList.Add(entity);
                    }

                    List<Entity> entitiesToDelete = new List<Entity>();
                    for (int i = 0; i < filteredList.Count; i++)
                    {
                        if (filteredList[i].Type.Name.ToLower() != delovi[3].ToLower())
                        {
                            entitiesToDelete.Add(filteredList[i]);
                        }
                    }

                    foreach (Entity entity in entitiesToDelete)
                    {
                        filteredList.Remove(entity);
                    }

                    if (delovi[4].Equals(">"))
                    {
                        entitiesToDelete.Clear();

                        if (canParse)
                        {
                            if (parsedId >= 0)
                            {
                                for (int i = 0; i < filteredList.Count; i++)
                                {
                                    if (filteredList[i].Id <= parsedId)
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
                    else if (delovi[4].Equals("<"))
                    {
                        entitiesToDelete.Clear();

                        if (canParse)
                        {
                            if (parsedId >= 0)
                            {

                                for (int i = 0; i < filteredList.Count; i++)
                                {
                                    if (filteredList[i].Id >= parsedId)
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
                    else if (delovi[4].Equals("="))
                    {
                        entitiesToDelete.Clear();

                        if (canParse)
                        {
                            if (parsedId >= 0)
                            {

                                for (int i = 0; i < filteredList.Count; i++)
                                {
                                    if (filteredList[i].Id != parsedId)
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
                
                TerminalString = "terminal:~$ ";
            }
        }

        public string TerminalString
        {
            get { return terminalString; }
            set 
            { 
                terminalString = value;
                OnPropertyChanged("TerminalString");
            }
        }
    }
}

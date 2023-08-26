using NetworkService.Data;
using NetworkService.Model;
using NetworkService.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MVVMLight.Messaging;



namespace NetworkService.ViewModel
{
    public class MainWindowViewModel : BindableBase
    {
        public MyICommand<string> NavCommand { get; private set; }

        private NetworkEntitiesViewModel networkEntitiesViewModel = new NetworkEntitiesViewModel();
        private NetworkDisplayViewModel networkDisplayViewModel = new NetworkDisplayViewModel();
        private MeasurementGraphViewModel measurementGraphViewModel = new MeasurementGraphViewModel();
        private TerminalViewModel terminalViewModel = new TerminalViewModel();
        public MyICommand<Window> Izlaz { get; private set; }

        private BindableBase currentViewModel;

        public MainWindowViewModel()
        {
            createListener();

            NavCommand = new MyICommand<string>(OnNav);
            Izlaz = new MyICommand<Window>(CloseWindow);

            CurrentViewModel = networkEntitiesViewModel;

            networkEntitiesViewModel.Entities.CollectionChanged += this.OnCollectionChanged;
            terminalViewModel.Entities.CollectionChanged += this.OnCollectionChanged;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Entity newEntity in e.NewItems)
                {
                    if (!networkDisplayViewModel.EntitiesInList.Contains(newEntity))
                    {
                        networkDisplayViewModel.EntitiesInList.Add(newEntity);
                    }

                    if (!measurementGraphViewModel.Entities.Contains(newEntity))
                    {
                        measurementGraphViewModel.Entities.Add(newEntity);
                    }

                    if (!networkEntitiesViewModel.Entities.Contains(newEntity))
                    {
                        networkEntitiesViewModel.Entities.Add(newEntity);
                    }

                    if (!terminalViewModel.Entities.Contains(newEntity))
                    {
                        terminalViewModel.Entities.Add(newEntity);
                    }

                    foreach (EntitiesByType ebt in networkDisplayViewModel.TreeViewEntities)
                    {
                        if (ebt.TypeName.Equals(newEntity.Type.Name))
                        {
                            if (!ebt.Entities.Contains(newEntity))
                            {
                                ebt.Entities.Add(newEntity);
                            }
                        }
                    }
                }
            }

            if (e.OldItems != null)
            {
                foreach (Entity oldEntity in e.OldItems)
                {
                    if (networkDisplayViewModel.EntitiesInList.Contains(oldEntity))
                    {
                        networkDisplayViewModel.EntitiesInList.Remove(oldEntity);
                    }
                    else
                    {
                        int canvasIndex = networkDisplayViewModel.GetCanvasIndexForEntityId(oldEntity.Id);
                        networkDisplayViewModel.DeleteEntityFromCanvas(oldEntity);
                    }

                    if (measurementGraphViewModel.Entities.Contains(oldEntity))
                    {
                        measurementGraphViewModel.Entities.Remove(oldEntity);
                    }

                    if (networkEntitiesViewModel.Entities.Contains(oldEntity))
                    {
                        networkEntitiesViewModel.Entities.Remove(oldEntity);
                    }

                    if (terminalViewModel.Entities.Contains(oldEntity))
                    {
                        terminalViewModel.Entities.Remove(oldEntity);
                    }

                    foreach (EntitiesByType ebt in networkDisplayViewModel.TreeViewEntities)
                    {
                        if (ebt.TypeName.Equals(oldEntity.Type.Name))
                        {
                            if (ebt.Entities.Contains(oldEntity))
                            {
                                ebt.Entities.Remove(oldEntity);
                            }
                        }
                    }
                }
            }
        }

        public BindableBase CurrentViewModel
        {
            get { return currentViewModel; }
            set
            {
                SetProperty(ref currentViewModel, value);
            }
        }

        private void OnNav(string destination)
        {
            switch (destination)
            {
                case "NetworkEntities":
                    CurrentViewModel = networkEntitiesViewModel;
                    break;
                case "NetworkDisplay":
                    CurrentViewModel = networkDisplayViewModel;
                    break;
                case "MeasurementGraph":
                    CurrentViewModel = measurementGraphViewModel;
                    break;
                case "Terminal":
                    CurrentViewModel = terminalViewModel;
                    break;
            }
        }

        private void createListener()
        {
            var tcp = new TcpListener(IPAddress.Any, 25565);
            tcp.Start();

            var listeningThread = new Thread(() =>
            {
                while (true)
                {
                    var tcpClient = tcp.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(param =>
                    {
                        //Prijem poruke
                        NetworkStream stream = tcpClient.GetStream();
                        string incomming;
                        byte[] bytes = new byte[1024];
                        int i = stream.Read(bytes, 0, bytes.Length);
                        //Primljena poruka je sacuvana u incomming stringu
                        incomming = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                        //Ukoliko je primljena poruka pitanje koliko objekata ima u sistemu -> odgovor
                        if (incomming.Equals("Need object count"))
                        {
                            //Response
                            /* Umesto sto se ovde salje count.ToString(), potrebno je poslati 
                             * duzinu liste koja sadrzi sve objekte pod monitoringom, odnosno
                             * njihov ukupan broj (NE BROJATI OD NULE, VEC POSLATI UKUPAN BROJ)
                             * */

                            Byte[] data = System.Text.Encoding.ASCII.GetBytes(networkEntitiesViewModel.Entities.Count.ToString());

                            stream.Write(data, 0, data.Length);
                        }
                        else
                        {
                            //U suprotnom, server je poslao promenu stanja nekog objekta u sistemu
                            Console.WriteLine(incomming); //Na primer: "Entitet_1:272"

                            //################ IMPLEMENTACIJA ####################
                            // Obraditi poruku kako bi se dobile informacije o izmeni
                            // Azuriranje potrebnih stvari u aplikaciji

                            string incommingEntityId = incomming.Substring(0, incomming.IndexOf(':'));
                            double newValue = double.Parse(incomming.Substring(incomming.IndexOf(':') + 1));

                            for (int idx = 0; idx < networkEntitiesViewModel.Entities.Count; idx++)
                            {
                                string currentEntityId = $"Entitet_{idx}";
                                if (currentEntityId == incommingEntityId)
                                {
                                    networkEntitiesViewModel.Entities[idx].Value = newValue;

                                    using (StreamWriter writer = File.AppendText("Log.txt"))
                                    {
                                        DateTime dateTime = DateTime.Now;
                                        writer.WriteLine($"{dateTime.ToString("yyyy-MM-dd HH:mm:ss")}: {networkEntitiesViewModel.Entities[idx].Id}, {newValue}");
                                    }

                                    networkDisplayViewModel.UpdateEntityOnCanvas(networkEntitiesViewModel.Entities[idx]);
                                    measurementGraphViewModel.OnShow();

                                    break;
                                }
                            }
                        }
                    }, null);
                }
            });

            listeningThread.IsBackground = true;
            listeningThread.Start();
        }

        private void CloseWindow(Window MainWindow)
        {
            if(MainWindow != null)
            {
                MainWindow.Close();
            }
            
        }
    }
}

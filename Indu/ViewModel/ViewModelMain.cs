using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Indu.ViewModel
{
    class ViewModelMain : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        Server.Server serv = new Server.Server();
        Server.Handler hand = new Server.Handler();
        
        
        public string Name
        {
            get
            {
                if (hand.IDset)
                {
                    return hand.getName(serv.Token);
                }
                else
                {
                    return "log in";

                }
            }
        }

      public ICommand LogInCommand
        {
            get
            {
                return new RelayCommand(o =>
                {                   
                    
                    System.Diagnostics.Process.Start("https://login.eveonline.com/oauth/authorize/?response_type=code&redirect_uri=https://localhost:6166/&client_id=2523a6cfe3b74756b95ff162c02675e8&scope=characterIndustryJobsRead&state=dadada");
                    serv.Start("https://localhost:6166/");

                    
                });
            }
        }

        public ICommand AktuCommand
        {
            get
            {
                return new RelayCommand(o=>
                {
                    hand.GetCharacterID(serv.Token);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
                );
            }
        }

    }
}

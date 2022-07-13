using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Chess;

namespace ChessApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void OnStartUp(object sender, StartupEventArgs a)
        {
            new GameWindow().Show();
        }
    }
}

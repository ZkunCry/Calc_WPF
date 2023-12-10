using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CalculatorTRPO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DotNetEnv.Env.Load();
            var m = Environment.GetEnvironmentVariable("PASSWORD");
            var services = new ServiceCollection();
            var database = new DataBase();
            /*var json = new RAM();*/
            /*var file = new FIle();*/
            services.AddSingleton<IMemory, DataBase>();
            /*services.AddSingleton<IMemory, RAM>();*/

            var provider = services.BuildServiceProvider();
            
            InitializeComponent();
            DataContext = new CalcViewModel(provider.GetRequiredService<IMemory>());
        }
    }
}

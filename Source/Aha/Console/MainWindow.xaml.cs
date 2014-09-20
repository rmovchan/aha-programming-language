using System;
using System.Collections.Generic;
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
using System.Reflection;
using Aha.API;
using Aha.API.Application;
using Aha.API.Jobs;
using Aha.Base;
using Aha.Core;
using Aha.Engine_;

namespace Console
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        delegate void func_Output(string text);

        struct BehaviorParams<tpar_Event> : icomp_BehaviorParams<tpar_Event>
        {
            private func_Output field_output;
            private string field_settings;
            private icomp_Engine<tpar_Event> field_engine;

            public bool attr_settings(out IahaArray<char> result) { result = new AhaString(field_settings); return true; }
            public bool attr_password(out IahaArray<char> result) { result = new AhaString(""); return true; } //TODO
            public bool fattr_output(IahaArray<char> text, out opaque_Job<tpar_Event> result) 
            {
                func_Output output = field_output;
                result = new opaque_Job<tpar_Event>()
                {
                    title = "output",
                    execute = delegate() { output(new string(text.get())); }
                };  
                return true; 
            }
            public bool attr_engine(out icomp_Engine<tpar_Event> result) { result = field_engine; return true; }

            public BehaviorParams(func_Output output, string settings, icomp_Engine<tpar_Event> engine)
            {
                field_output = output;
                field_settings = settings;
                field_engine = engine;
            }
        }

        private Assembly assembly;
        private object app;
        private Type eventType;
        private object b;
        private Type appType;
        private Type engType;
        private object eng;
        private bool running;
        private string title;
        private Queue<string> messages = new Queue<string>();
        private void output(string text) { messages.Enqueue(text); }
        private System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

        private void PopulateApplications()
        {
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Aha! Factor").OpenSubKey("Aha! for .NET").OpenSubKey("ConsoleApp");
            string[] names = key.GetSubKeyNames();
            foreach (string name in names)
            {
                //ToolStripDropDownItem item = new ToolStripDropDownButton(name, null, item_Click); 
                //toolStripMenuItem1.DropDownItems.Add(item);
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".dll"; // Default file extension
            dlg.Filter = "Assemblies (.dll)|*.dll"; // Filter files by extension 

            if ((bool)dlg.ShowDialog())
            {
                eng = null;
                assembly = Assembly.LoadFrom(dlg.FileName);
                try
                {
                    eventType = assembly.GetType("Aha.API.Application.opaque_Event");
                    Type exportType = assembly.GetType("Aha.API.Application.export");
                    object export = Activator.CreateInstance(exportType);
                    app = exportType.GetField("value").GetValue(export);
                    appType = typeof(imod_Application<>).MakeGenericType(new Type[] { eventType });
                    engType = typeof(comp_Engine<>).MakeGenericType(new Type[] { eventType });
                    eng = Activator.CreateInstance(engType);
                    // get application's title
                    object[] args = new object[1];
                    appType.InvokeMember(
                        "attr_Title",
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod,
                        null,
                        app,
                        args);
                    title = new string(((IahaArray<char>)args[0]).get());
                    status.Content = "Enter application settings for '" + title + "'";
                    parameters.IsReadOnly = false;
                    parameters.Focus();
                    start.IsEnabled = true;
                }
                catch (System.Exception)
                {
                    status.Content = "Error: assembly doesn't contain an Aha! application";
                }
            }

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            status.Content = "Running application '" + title + "'";
            input.IsReadOnly = false;
            input.Focus();
            messageLog.Clear();
            running = true;
            // create behavior parameters
            Type bpType = typeof(BehaviorParams<>).MakeGenericType(new Type[] { eventType });
            func_Output local_output = output;
            b = Activator.CreateInstance(bpType, new object[] { local_output, parameters.Text, eng });
            // get application's behavior
            object[] args = new object[2];
            args[0] = b;
            appType.InvokeMember
                (
                    "fattr_Behavior",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod,
                    null,
                    app,
                    args
                );
            engType.InvokeMember
                (
                    "StartExternal",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod,
                    null,
                    eng,
                    new object[] { args[1] }
                );
            start.IsEnabled = false;
            open.IsEnabled = false;
            stop.IsEnabled = true;
            parameters.IsReadOnly = true;
            dispatcherTimer.Start();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
        }

        private void parameters_TextInput(object sender, TextCompositionEventArgs e)
        {
        }

        private void TextBox_TextInput_1(object sender, TextCompositionEventArgs e)
        {
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void stop_Click(object sender, RoutedEventArgs e)
        {
            engType.InvokeMember
                (
                    "StopExternal",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod,
                    null,
                    eng,
                    new object[] { }
                );
        }

        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (eng != null)
            {
                engType.InvokeMember
                    (
                        "StopExternal",
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod,
                        null,
                        eng,
                        new object[] { }
                    );
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            while (messages.Count > 0) { messageLog.AppendText(messages.Dequeue()); messageLog.ScrollToEnd(); }
            //if (running) while (eng.Trace().Count > 0) listBox2.Items.Add(DateTime.Now.TimeOfDay.ToString() + ": " + eng.Trace().Dequeue());
            bool term = (Boolean)engType.InvokeMember
                (
                    "Terminated",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod,
                    null,
                    eng,
                    new object[] { }
                );
            if (running && term)
            {
                dispatcherTimer.Stop();
                status.Content = "Terminated";
                input.IsReadOnly = true;
                running = false;
                start.IsEnabled = true;
                open.IsEnabled = true;
                stop.IsEnabled = false;
            }
        }

        private void Window_Initialized_1(object sender, EventArgs e)
        {
            dispatcherTimer.Tick += new EventHandler(timer1_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 20);
        }

        private void input_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                object[] args = new object[2];
                args[0] = new AhaString(((TextBox)sender).Text);
                if ((bool)appType.InvokeMember
                        (
                            "fattr_Receive",
                            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod,
                            null,
                            app,
                            args
                        )
                    )
                    engType.InvokeMember
                        (
                            "HandleExternal",
                            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod,
                            null,
                            eng,
                            new object[] { args[1] }
                        );
                ((TextBox)sender).Clear();
            }
        }

    }
}

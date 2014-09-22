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

        struct TraceRec
        {
            public DateTime time;
            public string message;
        }

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
        private Queue<string> messages = new Queue<string>();
        private Queue<TraceRec> trace = new Queue<TraceRec>();
        private bool queueOvfl;
        private bool appSuspended;
        private void output(string text) 
        {
            if (messages.Count == 100 && !queueOvfl)
            {
                queueOvfl = true;
                engType.InvokeMember
                    (
                        "Suspend",
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod,
                        null,
                        eng,
                        new object[] { }
                    );
            }
            messages.Enqueue(text);
        }
        private void addTrace(string text)
        {
            if (trace.Count == 100 && !queueOvfl)
            {
                queueOvfl = true;
                engType.InvokeMember
                    (
                        "Suspend",
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod,
                        null,
                        eng,
                        new object[] { }
                    );
            }
            trace.Enqueue(new TraceRec() { time = DateTime.Now, message = text });
        }
        private System.Windows.Threading.DispatcherTimer outputTimer = new System.Windows.Threading.DispatcherTimer();

        private void PopulateApplications()
        {
            try
            {
                Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Aha! Factor").OpenSubKey("Aha! for .NET").OpenSubKey("ConsoleApp");
                string[] names = registryKey.GetSubKeyNames();
                foreach (string name in names)
                {
                    applicationBox.Items.Add(name);
                }
            }
            catch (System.Exception)
            { }
            applicationBox.Items.IsLiveSorting = true;
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadDll(string path)
        {
            try
            {
                assembly = Assembly.LoadFrom(path);
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
                string title = new string(((IahaArray<char>)args[0]).get());
                status.Content = "Enter application settings";
                applicationBox.Text = title;
                parameters.IsReadOnly = false;
                parameters.Focus();
                start.IsEnabled = true;
                register.IsEnabled = true;
                messageLog.Clear();
                traceView.Items.Clear();
                clearTrace.IsEnabled = false;
                saveTrace.IsEnabled = false;
            }
            catch (System.Exception)
            {
                status.Content = "Error: location doesn't contain an Aha! application";
                app = null;
                appType = null;
                eng = null;
                engType = null;
                //title = "";
                applicationBox.Text = "";
                start.IsEnabled = false;
                register.IsEnabled = false;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = ""; // Default file name
            dlg.DefaultExt = ".dll"; // Default file extension
            dlg.Filter = "Assemblies (.dll)|*.dll"; // Filter files by extension 

            if ((bool)dlg.ShowDialog())
            {
                eng = null;
                LoadDll(dlg.FileName);
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            status.Content = "Running";
            input.IsReadOnly = false;
            input.Focus();
            messageLog.Clear();
            //traceView.Items.Clear();
            running = true;
            // create behavior parameters
            Type bpType = typeof(BehaviorParams<>).MakeGenericType(new Type[] { eventType });
            func_Output local_output = output;
            func_Trace local_trace = addTrace;
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
                    new object[] { args[1], local_trace }
                );
            start.IsEnabled = false;
            open.IsEnabled = false;
            suspend.IsEnabled = true;
            applicationBox.IsEnabled = false;
            appSuspended = false;
            stop.IsEnabled = true;
            parameters.IsReadOnly = true;
            outputTimer.Start();
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
            outputTimer.Stop();
            while (messages.Count > 0) 
            { 
                messageLog.AppendText(messages.Dequeue()); 
                messageLog.ScrollToEnd(); 
            }
            while (trace.Count > 0)
            {
                TraceRec rec = trace.Dequeue();
                traceView.Items.Add(rec.time.ToLongTimeString() + ": " + rec.message);
            }
            if (queueOvfl && !appSuspended)
            {
                engType.InvokeMember
                    (
                        "Resume",
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod,
                        null,
                        eng,
                        new object[] { }
                    );
            }
            queueOvfl = false;
            if (traceView.Items.Count > 0)
            {
                clearTrace.IsEnabled = true;
                saveTrace.IsEnabled = true;
            }
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
                running = false;
                status.Content = "Terminated";
                input.IsReadOnly = true;
                start.IsEnabled = true;
                open.IsEnabled = true;
                applicationBox.IsEnabled = true;
                suspend.IsEnabled = false;
                resume.IsEnabled = false;
                stop.IsEnabled = false;
                parameters.IsReadOnly = false;
            }
            else
                outputTimer.Start();
        }

        private void Window_Initialized_1(object sender, EventArgs e)
        {
            outputTimer.Tick += new EventHandler(timer1_Tick);
            outputTimer.Interval = new TimeSpan(0, 0, 0, 0, 20);
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

        private void suspend_Click(object sender, RoutedEventArgs e)
        {
            engType.InvokeMember
                (
                    "Suspend",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod,
                    null,
                    eng,
                    new object[] { }
                );
            appSuspended = true;
            suspend.IsEnabled = false;
            resume.IsEnabled = true;
            status.Content = "Suspended";
        }

        private void resume_Click(object sender, RoutedEventArgs e)
        {
            engType.InvokeMember
                (
                    "Resume",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod,
                    null,
                    eng,
                    new object[] { }
                );
            appSuspended = false;
            suspend.IsEnabled = true;
            resume.IsEnabled = false;
            status.Content = "Running";
        }

        private void register_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\\Software\\Aha! Factor\\Aha! for .NET\\ConsoleApp\\" + applicationBox.Text, "Path", assembly.Location);
            Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\\Software\\Aha! Factor\\Aha! for .NET\\ConsoleApp\\" + applicationBox.Text, "Settings", parameters.Text);
            Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\\Software\\Aha! Factor\\Aha! for .NET\\ConsoleApp\\" + applicationBox.Text, "WindowState", (int)this.WindowState);
            Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\\Software\\Aha! Factor\\Aha! for .NET\\ConsoleApp\\" + applicationBox.Text, "WindowHeight", this.Height);
            Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\\Software\\Aha! Factor\\Aha! for .NET\\ConsoleApp\\" + applicationBox.Text, "WindowWidth", this.Width);
            if (applicationBox.SelectedIndex < 0)
            {
                applicationBox.Items.Add(applicationBox.Text);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateApplications();
        }

        private void applicationBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (applicationBox.SelectedIndex >= 0)
            {
                string path = (string)Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\\Software\\Aha! Factor\\Aha! for .NET\\ConsoleApp\\" + applicationBox.Items[applicationBox.SelectedIndex], "Path", "");
                LoadDll(path);
                parameters.Text = (string)Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\\Software\\Aha! Factor\\Aha! for .NET\\ConsoleApp\\" + applicationBox.Items[applicationBox.SelectedIndex], "Settings", "");
                this.WindowState = (System.Windows.WindowState)Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\\Software\\Aha! Factor\\Aha! for .NET\\ConsoleApp\\" + applicationBox.Items[applicationBox.SelectedIndex], "WindowState", System.Windows.WindowState.Normal);
                this.Height = Convert.ToDouble((string)Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\\Software\\Aha! Factor\\Aha! for .NET\\ConsoleApp\\" + applicationBox.Items[applicationBox.SelectedIndex], "WindowHeight", this.Height));
                this.Width = Convert.ToDouble((string)Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\\Software\\Aha! Factor\\Aha! for .NET\\ConsoleApp\\" + applicationBox.Items[applicationBox.SelectedIndex], "WindowWidth", this.Width));
            }
        }

        private void clearTrace_Click(object sender, RoutedEventArgs e)
        {
            traceView.Items.Clear();
            clearTrace.IsEnabled = false;
            saveTrace.IsEnabled = false;
        }

        private void saveTrace_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Trace"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension 

            if ((bool)dlg.ShowDialog())
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(dlg.FileName))
                {
                    foreach (string line in traceView.Items)
                    {
                        file.WriteLine(line);
                    }
                }
            }
        }

    }
}

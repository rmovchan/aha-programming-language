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
using Aha.Package.API;
using Aha.Package.API.Application;
using Aha.Package.API.Jobs;
using Aha.Package.Base;
using Aha.Core;
using Aha.Engine;

namespace Console
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        delegate void MyOutput(string text);

        struct TraceRec
        {
            public DateTime time;
            public string message;
        }

        struct BehaviorParams<tpar_Event>: icom_BehaviorParams<tpar_Event>
        {
            private MyOutput field_output;
            private string field_settings;
            private string field_password;
            private icomp_Engine<tpar_Event> field_engine;

            public bool attr_settings(out IahaArray<char> result) { result = new AhaString(field_settings); return true; }
            //public bool attr_password(out IahaArray<char> result) { result = new AhaString(field_password); return true; } //TODO
            public bool fattr_output(IahaArray<char> text, out opaque_Job<tpar_Event> result) 
            {
                MyOutput output = field_output;
                result = new opaque_Job<tpar_Event>()
                {
                    title = "output",
                    execute = delegate() { output(new string(text.get())); }
                };  
                return true; 
            }
            public bool attr_engine(out icomp_Engine<tpar_Event> result) { result = field_engine; return true; }

            public BehaviorParams(MyOutput output, string settings, string password, icomp_Engine<tpar_Event> engine)
            {
                field_output = output;
                field_settings = settings;
                field_password = password;
                field_engine = engine;
            }
        }

        private Microsoft.Win32.RegistryKey registryKey;
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
            if (messages.Count == 200 && !queueOvfl)
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
            if (trace.Count == 200 && !queueOvfl)
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
                registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software", true).OpenSubKey("Aha! Factor", true).OpenSubKey("Aha! Factor for .NET", true).OpenSubKey("ConsoleApp", true);
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
                eventType = assembly.GetType("Aha.Package.API.Application.opaque_Event");
                Type exportType = assembly.GetType("Aha.Package.API.Application.module_Application");
                app = Activator.CreateInstance(exportType);
                //app = exportType.GetField("value").GetValue(export);
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
                status.Content = "Enter " + title + " settings and select command Start";
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
            //status.Content = "Enter application's password and press Enter";
            //password.Visibility = Visibility.Visible;
            //passwordLabel.Visibility = Visibility.Visible;
            //password.Focus();
            //passwordLabel.Visibility = Visibility.Hidden;
            //password.Visibility = Visibility.Hidden;
            // create behavior parameters
            Type bpType = typeof(BehaviorParams<>).MakeGenericType(new Type[] { eventType });
            MyOutput local_output = output;
            func_Trace local_trace = addTrace;
            b = Activator.CreateInstance(bpType, new object[] { local_output, parameters.Text, password.Password, eng });
            // get application's behavior
            object[] args = new object[2];
            args[0] = b;
            try
            {
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
            }
            catch (System.Exception)
            {
                status.Content = "Error starting application";
                return;
            }
            start.IsEnabled = false;
            open.IsEnabled = false;
            suspend.IsEnabled = true;
            applicationBox.IsEnabled = false;
            appSuspended = false;
            stop.IsEnabled = true;
            parameters.IsReadOnly = true;
            messageLog.Clear();
            //traceView.Items.Clear();
            status.Content = "Running";
            running = true;
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
                messageLog.AppendText(messages.Dequeue() + "\n"); 
                messageLog.ScrollToEnd(); 
            }
            while (trace.Count > 0)
            {
                TraceRec rec = trace.Dequeue();
                traceView.Items.Add(rec.time.ToString("HH:mm:ss.ffff") + ": " + rec.message);
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
            if
                ((Boolean)engType.InvokeMember
                    (
                        "Enabled",
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod,
                        null,
                        eng,
                        new object[] { }
                    )
                )
            {
                if (input.Visibility != Visibility.Visible)
                {
                    input.Visibility = Visibility.Visible;
                    inputLabel.Visibility = Visibility.Visible;
                    input.Focus();
                }
            }
            else
            {
                input.Visibility = Visibility.Hidden;
                inputLabel.Visibility = Visibility.Hidden;
            }
            if (running && term)
            {
                running = false;
                status.Content = "Terminated";
                start.IsEnabled = true;
                open.IsEnabled = true;
                applicationBox.IsEnabled = true;
                suspend.IsEnabled = false;
                resume.IsEnabled = false;
                stop.IsEnabled = false;
                parameters.IsReadOnly = false;
                parameters.Focus();
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
                engType.InvokeMember
                    (
                        "HandleInput",
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod,
                        null,
                        eng,
                        new object[] { new AhaString(((TextBox)sender).Text) }
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
            Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\\Software\\Aha! Factor\\Aha! Factor for .NET\\ConsoleApp\\" + applicationBox.Text, "Path", assembly.Location);
            Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\\Software\\Aha! Factor\\Aha! Factor for .NET\\ConsoleApp\\" + applicationBox.Text, "Settings", parameters.Text);
            Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\\Software\\Aha! Factor\\Aha! Factor for .NET\\ConsoleApp\\" + applicationBox.Text, "WindowState", (int)this.WindowState);
            Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\\Software\\Aha! Factor\\Aha! Factor for .NET\\ConsoleApp\\" + applicationBox.Text, "WindowHeight", this.Height);
            Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\\Software\\Aha! Factor\\Aha! Factor for .NET\\ConsoleApp\\" + applicationBox.Text, "WindowWidth", this.Width);
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
            int idx = applicationBox.SelectedIndex;
            unregister.IsEnabled = idx >= 0;
            if (idx >= 0)
            {
                string path = (string)Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\\Software\\Aha! Factor\\Aha! Factor for .NET\\ConsoleApp\\" + applicationBox.Items[idx], "Path", "");
                LoadDll(path);
                parameters.Text = (string)Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\\Software\\Aha! Factor\\Aha! Factor for .NET\\ConsoleApp\\" + applicationBox.Items[idx], "Settings", "");
                this.WindowState = (System.Windows.WindowState)Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\\Software\\Aha! Factor\\Aha! Factor for .NET\\ConsoleApp\\" + applicationBox.Items[idx], "WindowState", this.WindowState);
                this.Height = Convert.ToDouble((string)Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\\Software\\Aha! Factor\\Aha! Factor for .NET\\ConsoleApp\\" + applicationBox.Items[idx], "WindowHeight", this.Height));
                this.Width = Convert.ToDouble((string)Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\\Software\\Aha! Factor\\Aha! Factor for .NET\\ConsoleApp\\" + applicationBox.Items[idx], "WindowWidth", this.Width));
            }
            else
                parameters.Text = "";
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

        private void unregister_Click(object sender, RoutedEventArgs e)
        {
            int idx = applicationBox.SelectedIndex;
            if (idx >= 0)
            {
                registryKey.DeleteSubKey((string)applicationBox.Items[idx], false);
                applicationBox.Items.RemoveAt(idx);
                applicationBox.SelectedIndex = -1;
                parameters.Text = "";
                register.IsEnabled = false;
                start.IsEnabled = false;
                stop.IsEnabled = false;
                suspend.IsEnabled = false;
                resume.IsEnabled = false;
                unregister.IsEnabled = false;
            }
        }

        private void password_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Return)
            //{
            //    passwordLabel.Visibility = Visibility.Hidden;
            //    password.Visibility = Visibility.Hidden;
            //    // create behavior parameters
            //    Type bpType = typeof(BehaviorParams<>).MakeGenericType(new Type[] { eventType });
            //    func_Output local_output = output;
            //    func_Trace local_trace = addTrace;
            //    b = Activator.CreateInstance(bpType, new object[] { local_output, parameters.Text, password.Password, eng });
            //    // get application's behavior
            //    object[] args = new object[2];
            //    args[0] = b;
            //    try
            //    {
            //        appType.InvokeMember
            //            (
            //                "fattr_Behavior",
            //                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod,
            //                null,
            //                app,
            //                args
            //            );
            //        engType.InvokeMember
            //            (
            //                "StartExternal",
            //                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod,
            //                null,
            //                eng,
            //                new object[] { args[1], local_trace }
            //            );
            //    }
            //    catch (System.Exception)
            //    {
            //        status.Content = "Error starting application";
            //        return;
            //    }
            //    start.IsEnabled = false;
            //    open.IsEnabled = false;
            //    suspend.IsEnabled = true;
            //    applicationBox.IsEnabled = false;
            //    appSuspended = false;
            //    stop.IsEnabled = true;
            //    parameters.IsReadOnly = true;
            //    messageLog.Clear();
            //    //traceView.Items.Clear();
            //    status.Content = "Running";
            //    running = true;
            //    outputTimer.Start();
            //}
        }

    }
}

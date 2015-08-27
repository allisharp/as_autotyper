using AlliSharp;
using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace as_autotyper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IHotKeyOwner
    {
        public FileLibrary<Typer> typers = null;
        public Typer edittedtyper = null;
        public HotKeyManager hotKeyManager = null;
        private char[] specialCharsList = new char[] { '{', '}', '(', ')', '+', '^', '%' };

        public MainWindow()
        {
            InitializeComponent();
        }


        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            tabAddNew.Visibility = Visibility.Visible;
            tabDefault.Visibility = Visibility.Collapsed;
            tbTextToType.Text = "<Text to type>";
            cbFKey.SelectedIndex = 0;
            cbPressEnter.IsChecked = true;
            cbIsActive.IsChecked = true;
            cbSendKeys.IsChecked = false;
            btnDone.Visibility = Visibility.Visible;
            btnEditDone.Visibility = Visibility.Collapsed;
            btnEditDelete.Visibility = Visibility.Collapsed;
        }

        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            foreach (Typer typer in lbTypers.Items)
            {
                if (typer.FKey == cbFKey.SelectedIndex + 1)
                {
                    new AlliSharp.AlliSharpMessageBox(this.Title, "The F Key chosen is already in use by another typer.");
                    return;
                }
            }
            if (tbTextToType.Text.Contains("{F" + (cbFKey.SelectedIndex + 1).ToString() + "}"))
            {
                if (cbSendKeys.IsChecked.Value)
                {
                    new AlliSharp.AlliSharpMessageBox(this.Title, "Please do not try to cause infinite loops with the auto-typer. Thanks, allisharp.com ;)");
                    return;
                }
            }

            Typer nt = new Typer(tbTextToType.Text, cbFKey.SelectedIndex + 1, cbSendKeys.IsChecked.Value, cbPressEnter.IsChecked.Value, cbIsActive.IsChecked.Value);
            if (cbIsActive.IsChecked.Value) nt.hotKeyId = hotKeyManager.RegisterFKey(cbFKey.SelectedIndex + 1);

            lbTypers.Items.Add(nt);

            tabAddNew.Visibility = Visibility.Collapsed;
            tabDefault.Visibility = Visibility.Visible;
            //throw new Exception("asdasd");
        }

        private void btnEditDone_Click(object sender, RoutedEventArgs e)
        {
            if (edittedtyper != null)
            {
                foreach (Typer typer in lbTypers.Items)
                {
                    if (typer != edittedtyper)
                    {
                        if (typer.FKey == cbFKey.SelectedIndex + 1)
                        {
                            new AlliSharp.AlliSharpMessageBox(this.Title, "The F Key chosen is already in use by another typer.");
                            return;
                        }
                    }
                }
                if (tbTextToType.Text.Contains("{F" + (cbFKey.SelectedIndex + 1).ToString() + "}"))
                {
                    if (cbSendKeys.IsChecked.Value)
                    {
                        new AlliSharp.AlliSharpMessageBox(this.Title, "Please do not try to cause infinite loops. If you really must, I guess you can use two or more typers together. Thanks, allisharp.com ;)");
                        return;
                    }
                }
                edittedtyper.Text = tbTextToType.Text;
                int oldfkey = edittedtyper.FKey;
                edittedtyper.FKey = cbFKey.SelectedIndex + 1;
                if (edittedtyper.FKey != oldfkey)
                {
                    if (edittedtyper.IsActive) hotKeyManager.UnregisterFKey(edittedtyper.hotKeyId);
                    if (cbIsActive.IsChecked.Value) edittedtyper.hotKeyId = hotKeyManager.RegisterFKey(edittedtyper.FKey);
                }
                else
                {
                    if (edittedtyper.IsActive != cbIsActive.IsChecked.Value)
                    {
                        if (cbIsActive.IsChecked.Value)
                        {
                            edittedtyper.hotKeyId = hotKeyManager.RegisterFKey(cbFKey.SelectedIndex + 1);
                        }
                        else
                        {
                            hotKeyManager.UnregisterFKey(edittedtyper.hotKeyId);
                        }
                    }
                }
                edittedtyper.PressEnter = cbPressEnter.IsChecked.Value;
                edittedtyper.IsActive = cbIsActive.IsChecked.Value;
                edittedtyper.SendKeys = cbSendKeys.IsChecked.Value;
                tabAddNew.Visibility = Visibility.Collapsed;
                tabDefault.Visibility = Visibility.Visible;
            }
            //throw new Exception("asdasd");
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            tabAddNew.Visibility = Visibility.Collapsed;
            tabDefault.Visibility = Visibility.Visible;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (edittedtyper != null)
            {
                if (lbTypers.Items.Contains(edittedtyper))
                {
                    if (edittedtyper.IsActive)
                    {
                        hotKeyManager.UnregisterFKey(edittedtyper.hotKeyId);
                    }
                    lbTypers.Items.Remove(edittedtyper);
                }
            }
            tabAddNew.Visibility = Visibility.Collapsed;
            tabDefault.Visibility = Visibility.Visible;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (lbTypers.SelectedItem != null)
            {
                if (lbTypers.SelectedItem as Typer != null)
                {
                    Typer typer = lbTypers.SelectedItem as Typer;
                    edittedtyper = typer;
                    tabAddNew.Visibility = Visibility.Collapsed;
                    tabDefault.Visibility = Visibility.Visible;
                    tbTextToType.Text = typer.Text;
                    cbFKey.SelectedIndex = typer.FKey - 1;
                    cbPressEnter.IsChecked = typer.PressEnter;
                    cbIsActive.IsChecked = typer.IsActive;
                    cbSendKeys.IsChecked = typer.SendKeys;
                    tabAddNew.Visibility = Visibility.Visible;
                    tabDefault.Visibility = Visibility.Collapsed;
                    btnDone.Visibility = Visibility.Collapsed;
                    btnEditDone.Visibility = Visibility.Visible;
                    btnEditDelete.Visibility = Visibility.Visible;
                }
            }

        }

        private void lbTypers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool enabled = false;
            if (lbTypers.SelectedItem != null)
            {
                if (lbTypers.SelectedItem as Typer != null)
                {
                    enabled = true;
                }
            }
            btnEditTyper.IsEnabled = enabled;
                
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            lbTypers.SelectedItem = null;
        }

        private void lbTypers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btnEdit_Click(sender, null);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();


            typers.Clear();
            foreach (Typer typer in lbTypers.Items)
            {
                if (typer.IsActive)
                {
                    hotKeyManager.UnregisterFKey(typer.hotKeyId);
                }
                typers.Add(typer);
            }
            typers.SaveLibrary();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (typers == null) //first load
            {
                typers = new FileLibrary<Typer>("typers");
                typers.LoadLibrary();
                foreach (Typer typer in typers)
                {
                    lbTypers.Items.Add(typer);
                }

                hotKeyManager = new HotKeyManager(this, new WindowInteropHelper(this).Handle);     

                foreach (Typer typer in lbTypers.Items)
                {
                    if (typer.IsActive)
                    {
                        typer.hotKeyId = hotKeyManager.RegisterFKey(typer.FKey);
                    }
                }

                if (ApplicationDeployment.IsNetworkDeployed)
                {
                    this.Title = this.Title + " " + ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(4);
                }
            }
        }

        public void HotKeyPressed(int id)
        {
            foreach (Typer typer in lbTypers.Items)
            {
                if (typer.hotKeyId.Equals(id))
                {
                    if (typer.IsActive)
                    {
                        try
                        {
                            if (typer.SendKeys)
                            {
                                SendKeys.SendWait(typer.PressEnter ? typer.Text + "{ENTER}" : typer.Text);
                            }
                            else
                            {
                                foreach (char c in typer.Text)
                                {
                                    if (specialCharsList.Contains(c))
                                    {
                                        SendKeys.SendWait("{" + c.ToString() + "}");
                                    }
                                    else
                                    {
                                        SendKeys.SendWait(c.ToString());
                                    }
                                }
                                if (typer.PressEnter) SendKeys.SendWait("{ENTER}");
                            }
                        }
                        catch
                        {
                            typer.IsActive = false;
                            hotKeyManager.UnregisterFKey(typer.hotKeyId);

                            Run run1 = new Run("The text for typer [F" + typer.FKey.ToString() + "] caused an error with SendKeys. ");
                            Run run2 = new Run("This typer has automatically been set to inactive. ");
                            Run run3 = new Run("To fix this error, set SendKeys to false. \n\n");
                            Run run4 = new Run("To learn more about commands and the proper syntax for SendKeys, check out: ");
                            Run run5 = new Run("https://msdn.microsoft.com/en-us/library/system.windows.forms.sendkeys.send%28v=vs.110%29.aspx");
                            Hyperlink hyperlink = new Hyperlink(run5)
                            {
                                NavigateUri = new Uri("https://msdn.microsoft.com/en-us/library/system.windows.forms.sendkeys.send%28v=vs.110%29.aspx")
                            };

                            Run run6 = new Run(".");
                            
                            hyperlink.RequestNavigate += new System.Windows.Navigation.RequestNavigateEventHandler(hyperlink_RequestNavigate);
                            Inline[] ilc = new Inline[] { run1, run2, run3, run4, hyperlink, run6 };

                            new AlliSharpMessageBox(this.Title, ilc);
                        }                        
                    }
                    break;
                }
            }
        }

        private void hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }

        private void btnGoToAlliSharpCom_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http:\\allisharp.com");
        }
    }
}

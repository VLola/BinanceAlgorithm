using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Binance.Net.Enums;
using Newtonsoft.Json;
using BinanceAlgorithm.Resourses;
using BinanceAlgorithm.Resourses.EmaResourses;
using System.Data;
using System.Windows.Data;
using System.ComponentModel;

namespace BinanceAlgorithm
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<EmaCompare> ema_list = new List<EmaCompare>();
        public List<ListKlines> LIST_KLINES = new List<ListKlines>();
        public GridViewColumnHeader _lastHeaderClicked = null;
        public ListSortDirection _lastDirection = ListSortDirection.Descending;
        public ICollectionView dataView;
        public double x_old;
        public double y_old;
        public double scale;
        public List<History> history = new List<History>();
        public MainWindow()
        {
            InitializeComponent();
            ErrorWatcher();
            FilesList();
            Clients();
            Exit.Visibility = Visibility.Hidden;
            LoadButtonsCompare();
            HistoryList.ItemsSource = history;
            dataView = CollectionViewSource.GetDefaultView(HistoryList.ItemsSource);
            order_open.Text = "0,5";
            order_sl.Text = "0,1";
            order_tp.Text = "0,1";

            MouseWheel += WindowChart_MouseWheel;
            MouseMove += WindowChart_MouseMove;
        }

        #region - Login -
        // ------------------------------------------------------- Start Client Block -------------------------------------------
        private void Clients()
        {
            try
            {
                string path = System.IO.Path.Combine(Environment.CurrentDirectory, "clients");
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                List<string> filesDir = (from a in Directory.GetFiles(path) select System.IO.Path.GetFileNameWithoutExtension(a)).ToList();
                if (filesDir.Count > 0)
                {
                    ClientList file_list = new ClientList(filesDir);
                    BoxName.ItemsSource = file_list.BoxNameContent;
                    BoxName.SelectedItem = file_list.BoxNameContent[0];
                }
            }
            catch (Exception e)
            {
                ErrorText.Add($"Clients {e.Message}");
            }
        }
        private void Button_Save(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = client_name.Text;
                string api = api_key.Text;
                string key = secret_key.Text;
                if (name != "" && api != "" && key != "")
                {
                    string path = System.IO.Path.Combine(Environment.CurrentDirectory, "clients");
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    if (!File.Exists(path + "/" + client_name.Text))
                    {
                        client_name.Text = "";
                        api_key.Text = "";
                        secret_key.Text = "";
                        Client client = new Client(name, api, key);
                        string json = JsonConvert.SerializeObject(client);
                        File.WriteAllText(path + "/" + name, json);
                        Clients();
                    }
                }
            }
            catch (Exception c)
            {
                ErrorText.Add($"Button_Save {c.Message}");
            }
        }
        // ------------------------------------------------------- End Client Block ---------------------------------------------
        // ------------------------------------------------------- Start Login Block -------------------------------------------
        private void Button_Login(object sender, RoutedEventArgs e)
        {
            try
            {
                string api = api_key.Text;
                string key = secret_key.Text;
                if (api != "" && key != "")
                {
                    client_name.Text = "";
                    api_key.Text = "";
                    secret_key.Text = "";
                    Socket.Connect(api, key);
                    Login_Click();
                }
                else if (BoxName.Text != "")
                {
                    string path = System.IO.Path.Combine(Environment.CurrentDirectory, "clients");
                    string json = File.ReadAllText(path + "\\" + BoxName.Text);
                    Client client = JsonConvert.DeserializeObject<Client>(json);
                    Socket.Connect(client.ApiKey, client.SecretKey);
                    Login_Click();
                }
            }
            catch (Exception c)
            {
                ErrorText.Add($"Button_Login {c.Message}");
            }
        }
        private void Login_Click()
        {
            api_key.Visibility = Visibility.Hidden;
            secret_key.Visibility = Visibility.Hidden;
            client_name.Visibility = Visibility.Hidden;
            BoxName.Visibility = Visibility.Hidden;
            Save.Visibility = Visibility.Hidden;
            Login.Visibility = Visibility.Hidden;
            label1.Visibility = Visibility.Hidden;
            label2.Visibility = Visibility.Hidden;
            label3.Visibility = Visibility.Hidden;
            Exit.Visibility = Visibility.Visible;
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            api_key.Visibility = Visibility.Visible;
            secret_key.Visibility = Visibility.Visible;
            client_name.Visibility = Visibility.Visible;
            BoxName.Visibility = Visibility.Visible;
            Save.Visibility = Visibility.Visible;
            Login.Visibility = Visibility.Visible;
            label1.Visibility = Visibility.Visible;
            label2.Visibility = Visibility.Visible;
            label3.Visibility = Visibility.Visible;
            Exit.Visibility = Visibility.Hidden;
        }
        // ------------------------------------------------------- End Login Block ---------------------------------------------
        #endregion

        #region - Error -
        // ------------------------------------------------------- Start Error Text Block --------------------------------------
        private void ErrorWatcher()
        {
            try
            {
                FileSystemWatcher error_watcher = new FileSystemWatcher();
                error_watcher.Path = ErrorText.Directory();
                error_watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                error_watcher.Changed += new FileSystemEventHandler(OnChanged);
                error_watcher.Filter = ErrorText.Patch();
                error_watcher.EnableRaisingEvents = true;
            }
            catch (Exception e)
            {
                ErrorText.Add($"ErrorWatcher {e.Message}");
            }
        }
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            Dispatcher.Invoke(new Action(() => { error_log.Text = File.ReadAllText(ErrorText.FullPatch()); }));
        }
        private void Button_ClearErrors(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(ErrorText.FullPatch(), "");
        }
        // ------------------------------------------------------- End Error Text Block ----------------------------------------
        #endregion

        #region - CheckTextBox -
        // ------------------------------------------------------- Start Digit Check TextBox -----------------------------------
        private void digit_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !((Char.IsDigit(e.Text, 0) || ((e.Text == System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0].ToString()) && (DS_Count(((TextBox)sender).Text) < 1))));
        }
        public int DS_Count(string s)
        {
            string substr = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0].ToString();
            int count = (s.Length - s.Replace(substr, "").Length) / substr.Length;
            return count;
        }
        // ------------------------------------------------------- End Digit Check TextBox --------------------------------------
        #endregion

        #region - HistoryFile -
        // ------------------------------------------------------- Start History File ----------------------------------------------
        public void StartHistoryFile()
        {
            try
            {
                List<string> sort = new List<string>();

                var result = Socket.futures.ExchangeData.GetPricesAsync().Result;
                if (!result.Success) ErrorText.Add("Error GetKlinesAsync");
                else
                {
                    foreach (var it in result.Data.ToList())
                    {
                        sort.Add(it.Symbol);
                    }
                    sort.Sort();

                    int count = Convert.ToInt32(klines_end.Text);
                    if (current_time.IsChecked.Value == true)
                    {
                        foreach (var symbol in sort)
                        {
                            Klines(symbol, klines_count: count);
                        }
                        WriteToFile(LIST_KLINES);
                    }
                    else
                    {
                        DateTime TIME = data_picker.SelectedDate.Value.Date;
                        if (StartTime.IsChecked.Value == true)
                        {
                            TIME = TIME.AddHours(Double.Parse(start_time_h.Text));
                            TIME = TIME.AddMinutes(Double.Parse(start_time_m.Text));
                        }
                        else
                        {
                            TIME = TIME.AddHours(Double.Parse(end_time_h.Text));
                            TIME = TIME.AddMinutes(Double.Parse(end_time_m.Text));
                        }
                        foreach (var symbol in sort)
                        {
                            if (StartTime.IsChecked.Value == true) Klines(symbol, start_time: TIME, klines_count: count);
                            else Klines(symbol, end_time: TIME, klines_count: count);
                        }
                        WriteToFile(LIST_KLINES);
                    }
                }
            }
            catch (Exception e)
            {
                ErrorText.Add($"StartHistoryFile {e.Message}");
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            StartHistoryFile();
        }

        // ------------------------------------------------------- End History File -------------------------------------------------
        #endregion

        #region - Klines -
        // ------------------------------------------------------- Start Klines Block --------------------------------------------
        public int KLINE_START;
        public int KLINE_END;

        private void klines_start_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (klines_start.Text.Length > 0 && klines_start.Text.Length < 4)
            {
                KLINE_START = Convert.ToInt32(klines_start.Text);
            }
        }

        private void klines_end_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (klines_end.Text.Length > 0 && klines_end.Text.Length < 4)
            {
                KLINE_END = Convert.ToInt32(klines_end.Text);
            }
        }
        public void Klines(string SYMBOL, DateTime? start_time = null, DateTime? end_time = null, int? klines_count = null)
        {
            try
            {

                var result = Socket.futures.ExchangeData.GetKlinesAsync(symbol: SYMBOL, interval: KlineInterval.OneMinute, startTime: start_time, endTime: end_time, limit: klines_count).Result;
                if (!result.Success) ErrorText.Add("Error GetKlinesAsync");
                else
                {
                    List<Kline> list = new List<Kline>();
                    foreach (var it in result.Data.ToList())
                    {
                        list.Insert(0, new Kline(it.OpenTime, it.OpenPrice, it.HighPrice, it.LowPrice, it.ClosePrice, it.CloseTime));                                 // список монет с ценами
                    }
                    LIST_KLINES.Add(new ListKlines(SYMBOL, list));
                }
            }

            catch (Exception e)
            {
                ErrorText.Add($"Klines {e.Message}");
            }
        }
        public void WriteToFile(List<ListKlines> list)
        {
            try
            {
                int size = list[0].listKlines.Count();
                string klines = size.ToString();
                DateTime time_start = list[0].listKlines[0].OpenTime;
                DateTime time_end = list[0].listKlines[size - 1].OpenTime;
                string path_date_start = $"{time_start.Year}.{time_start.Month}.{time_start.Day}_{time_start.Hour}.{time_start.Minute}-";
                string path_date_end = $"{time_end.Year}.{time_end.Month}.{time_end.Day}_{time_end.Hour}.{time_end.Minute}-";
                string path = System.IO.Path.Combine(Environment.CurrentDirectory, "");
                string path_full = @"\times\" + path_date_start + path_date_end + klines + ".txt";
                string json = JsonConvert.SerializeObject(list);
                File.AppendAllText(path + path_full, json);
                ErrorText.Add("Готово");
                LIST_KLINES.Clear();
                FilesList();
            }
            catch (Exception e)
            {
                ErrorText.Add($"WriteToFile {e.Message}");
            }
        }

        // ------------------------------------------------------- Start Texts List --------------------------------------------
        public void FilesList()
        {
            try
            {
                string path = System.IO.Path.Combine(Environment.CurrentDirectory, "times");
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                List<string> list = new List<string>();
                List<string> filesDir = (from a in Directory.GetFiles(path) select System.IO.Path.GetFileNameWithoutExtension(a)).ToList();
                if (filesDir.Count > 0)
                {
                    boxList file_list = new boxList(filesDir);
                    cmbTest1.ItemsSource = file_list.cmbContentFileNames;
                    cmbTest1.SelectedItem = file_list.cmbContentFileNames[0];
                }
            }
            catch (Exception e)
            {
                ErrorText.Add($"FilesList {e.Message}");
            }
        }
        // ------------------------------------------------------- End Texts List ----------------------------------------------
        // ------------------------------------------------------- End Klines Block ----------------------------------------------
        #endregion

        #region - Save Ema -

        public string symbol_ema;
        private void SaveEma(int period)
        {
            try
            {
                string path_ema = System.IO.Path.Combine(Environment.CurrentDirectory, "ema");
                if (!Directory.Exists(path_ema)) Directory.CreateDirectory(path_ema);
                if (!File.Exists(path_ema + "/" + period.ToString()))
                {
                    string path = System.IO.Path.Combine(Environment.CurrentDirectory, "");
                    string json = File.ReadAllText(path + @"\times\" + cmbTest1.Text + ".txt");
                    var list = JsonConvert.DeserializeObject<List<ListKlines>>(json);

                    List<ListEma> list_ema = new List<ListEma>();
                    int size = list[0].listKlines.Count;
                    for (int a = 0; a < list.Count; a++)
                    {
                        symbol_ema = list[a].symbol;
                        List<decimal> list_average = new List<decimal>();
                        for (int i = 0; i < size; i++)
                        {
                            decimal sum = 0m;
                            if (size - i > period)
                            {                                                                                                                   // Изменено условие
                                for (int j = 0; j < period; j++)
                                {
                                    decimal average_kline = (list[a].listKlines[i + j].High + list[a].listKlines[i + j].Low) / 2;
                                    sum += average_kline;
                                }
                                list_average.Add(sum / period);
                            }
                            else
                            {
                                for (int j = 0; j < (size - i); j++)
                                {
                                    decimal average_kline = (list[a].listKlines[i + j].High + list[a].listKlines[i + j].Low) / 2;
                                    sum += average_kline;
                                }
                                list_average.Add(sum / (size - i));
                            }
                        }

                        ListEma ema = new ListEma(symbol_ema, list_average);
                        list_ema.Add(ema);
                    }
                    ObjectListEma object_ema = new ObjectListEma(period, list_ema);
                    string json_object = JsonConvert.SerializeObject(object_ema);
                    File.WriteAllText(path_ema + "/" + period.ToString(), json_object);
                    LoadButtonsCompare();
                }
                else ErrorText.Add($"SaveEma file exist!");
            }
            catch (Exception e)
            {
                ErrorText.Add($"SaveEma {e.Message}");
            }
        }
        private void LoadButtonsCompare()
        {
            try
            {
                string path = System.IO.Path.Combine(Environment.CurrentDirectory, "ema");
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                List<string> filesDir = (from a in Directory.GetFiles(path) select System.IO.Path.GetFileNameWithoutExtension(a)).ToList();
                if (filesDir.Count > 0)
                {
                    EmaCompare file_list = new EmaCompare(filesDir);
                    compare_1.ItemsSource = file_list.Compare1;
                    compare_1.SelectedItem = file_list.Compare1[0];
                    compare_2.ItemsSource = file_list.Compare2;
                    compare_2.SelectedItem = file_list.Compare2[0];
                }
            }
            catch (Exception e)
            {
                ErrorText.Add($"LoadButtonsCompare {e.Message}");
            }

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (price_change.Text != "") SaveEma(Convert.ToInt32(price_change.Text));
                else ErrorText.Add("Specify price change!");
            }
            catch (Exception c)
            {
                ErrorText.Add($"Button_Click {c.Message}");
            }
        }
        #endregion

        #region - Ema Compare -

        private void EmaCompare(int compare_1, int compare_2)
        {
            try
            {
                history.Clear();
                dataView.SortDescriptions.Clear();

                //-------------------------------------------------------------------------------------
                string path_ema = System.IO.Path.Combine(Environment.CurrentDirectory, "ema");
                string json1 = File.ReadAllText(path_ema + "\\" + compare_1.ToString());
                string json2 = File.ReadAllText(path_ema + "\\" + compare_2.ToString());
                var list_ema_long = JsonConvert.DeserializeObject<ObjectListEma>(json1);
                var list_ema_short = JsonConvert.DeserializeObject<ObjectListEma>(json2);
                List<ListEma> list_result = new List<ListEma>();

                for (int i = 0; i < list_ema_long.Ema.Count; i++) list_result.Add(ResultPercentEma.ResultListEma(list_ema_long.Ema[i], list_ema_short.Ema[i]));
                //-----------------------------------------------------------------------------------
                decimal start = Convert.ToDecimal(order_open.Text);
                decimal tp = Convert.ToDecimal(order_tp.Text);
                decimal sl = Convert.ToDecimal(order_sl.Text);

                for(int i = 0;i < list_result.Count; i++) history.Add(ResultPercentEma.ResultHistory(list_result[i], list_ema_short.Ema[i], start, tp, sl));

                HistoryList.Items.Refresh();
            }
            catch (Exception e)
            {
                ErrorText.Add($"EmaCompare {e.Message}");
            }
        }
        private void LoadTableEmaCompare(object sender, RoutedEventArgs e)
        {
            try
            {
                if (compare_1.Text != "" && compare_2.Text != "" && order_open.Text != "" && order_sl.Text != "" && order_tp.Text != "") EmaCompare(Convert.ToInt32(compare_1.Text), Convert.ToInt32(compare_2.Text));
                else ErrorText.Add("Fill in all the data!");
            }
            catch (Exception c)
            {
                ErrorText.Add($"LoadTableEmaCompare {c.Message}");
            }
        }

        #endregion

        #region - Load Candlestick -
        private void listView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string path = System.IO.Path.Combine(Environment.CurrentDirectory, "");
                string json = File.ReadAllText(path + @"\times\" + cmbTest1.Text + ".txt");
                var list = JsonConvert.DeserializeObject<List<ListKlines>>(json);

                History item = (History)(sender as ListView).SelectedItem;
                if (item != null)
                {
                    int count = 0;
                    foreach (var it in list)
                    {
                        if (item.Sumbol == it.symbol)
                        {
                            string path_ema = System.IO.Path.Combine(Environment.CurrentDirectory, "ema");
                            string json1 = File.ReadAllText(path_ema + "\\" + compare_1.Text);
                            string json2 = File.ReadAllText(path_ema + "\\" + compare_2.Text);
                            var list_ema_long = JsonConvert.DeserializeObject<ObjectListEma>(json1);
                            var list_ema_short = JsonConvert.DeserializeObject<ObjectListEma>(json2);
                            Chart1.DataContext = new Candlestick(history[count].movement_history, it, list_ema_long.Ema[count].list, list_ema_short.Ema[count].list);
                        }
                        count++;
                    }
                }
            }
            catch (Exception c)
            {
                ErrorText.Add(c.Message);
            }

        }
        #endregion

        #region - Sorted -
        void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Descending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    var columnBinding = headerClicked.Column.DisplayMemberBinding as Binding;
                    var sortBy = columnBinding?.Path.Path ?? headerClicked.Column.Header as string;

                    Sort(sortBy, direction);

                    if (direction == ListSortDirection.Ascending)
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowUp"] as DataTemplate;
                    }
                    else
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowDown"] as DataTemplate;
                    }

                    // Remove arrow from previously sorted header
                    if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
                    {
                        _lastHeaderClicked.Column.HeaderTemplate = null;
                    }

                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }
            }
        }

        private void Sort(string sortBy, ListSortDirection direction)
        {
            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }
        #endregion

        #region - Chart Events -
        private void WindowChart_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                double x = 0;
                if (e.GetPosition(this).X - x_old > 0) x = 20;
                else if (e.GetPosition(this).X - x_old < 0) x = -20;
                if (x_old != 0)
                {
                    double width = Chart1.Width;
                    //if (Chart1.Width + x >= 1300 && Chart1.Width + x <= 3500)
                    Chart1.Width = width + x;
                    
                }

                double y = 0;
                if (e.GetPosition(this).Y - y_old > 0) y = 20;
                else if (e.GetPosition(this).Y - y_old < 0) y = -20;
                if (y_old != 0)
                {
                    double margin_top = Chart1.Margin.Top + y;
                    double margin_bottom = Chart1.Margin.Bottom - y;
                    Chart1.Margin = new Thickness(0, margin_top, 0 , margin_bottom);
                }
            }
            x_old = e.GetPosition(this).X;
            y_old = e.GetPosition(this).Y;
        }

        private void WindowChart_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (player1Scale.ScaleY + Convert.ToDouble(e.Delta) / 2000 > 0) player1Scale.ScaleY = player1Scale.ScaleY + Convert.ToDouble(e.Delta) / 2000;
        }
        #endregion
    }
}

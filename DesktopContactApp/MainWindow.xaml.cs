using DesktopContactApp.Classes;
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

namespace DesktopContactApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Contact> resultContact = new List<Contact>();

        public MainWindow()
        {
            InitializeComponent();
            List<Contact> resultContact = new List<Contact>();

            ReadDatabase();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NewContactWindow contactWindow = new NewContactWindow();
            contactWindow.ShowDialog();

            ReadDatabase();
        }

        void ReadDatabase()
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.databasePath))
            {
                conn.CreateTable<Contact>();
                resultContact = (conn.Table<Contact>().ToList()).OrderBy(c => c.Name).ToList();

                var variable = from c2 in resultContact
                               orderby c2.Name
                               select c2;

            }

            if (resultContact != null)
            {
                //foreach (var ContactItem in resultContact)
                //{
                //    contactListView.Items.Add(new ListViewItem()
                //    {
                //        Content = ContactItem
                //    });
                //}
                contactListView.ItemsSource = resultContact;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox searchTextBox = sender as TextBox;

            var filterList = resultContact.Where(c => c.Name.ToLower().Contains(searchTextBox.Text.ToLower()) || c.Phone.Contains(searchTextBox.Text)).ToList();

            var filterList2 = (from c2 in resultContact
                              where c2.Name.ToLower().Contains(searchTextBox.Text.ToLower())
                              orderby c2.Email
                              select c2).ToList();

            contactListView.ItemsSource = filterList2;
        }

        private void contactListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Contact selectedContact = (Contact)contactListView.SelectedItem;
            if (selectedContact != null)
            {
                DetailsContactWindow DetailscontactWindow = new DetailsContactWindow(selectedContact);
                DetailscontactWindow.ShowDialog();
            ReadDatabase();

            }
        }
    }
}

using System.ComponentModel;
using Xamarin.Forms;
using TestingSample.ViewModels;

namespace TestingSample.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}
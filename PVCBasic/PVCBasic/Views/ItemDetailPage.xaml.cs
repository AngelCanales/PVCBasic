using System.ComponentModel;
using Xamarin.Forms;
using PVCBasic.ViewModels;

namespace PVCBasic.Views
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
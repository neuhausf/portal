namespace Portal.View.Common
{
    using Windows.UI.Xaml.Controls;

    public class ListGroupStyleSelector : GroupStyleSelector
    {
        protected override GroupStyle SelectGroupStyleCore(object group, uint level)
        {
            return (GroupStyle)App.Current.Resources["ListViewGroupStyle"];
        }
    }

}
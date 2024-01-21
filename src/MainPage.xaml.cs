namespace SpaceShooter
{

    public partial class MainPage
    {
        public MainPage()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception e)
            {
                Super.DisplayException(this, e);
            }
        }

    }
}
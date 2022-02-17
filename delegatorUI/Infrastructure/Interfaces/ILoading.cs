namespace delegatorUI.Infrastructure.Interfaces
{
    public interface ILoading
    {
        public int LoadingOpacity { get; set; }

        public int LoadingZIndex { get; set; }

        public void StartLoading()
        {
            LoadingOpacity = 1;
            LoadingZIndex = 20;
        }

        public void EndLoading()
        {
            LoadingOpacity = 0;
            LoadingZIndex = -20;
        }
    }
}

namespace delegatorUI.Infrastructure.Interfaces
{
    public interface ILoading
    {
        public int LoadingOpacity { get; set; }

        public int LoadingZIndex { get; set; }

        private void StartLoading()
        {
            LoadingOpacity = 1;
            LoadingZIndex = 20;
        }

        private void EndLoading()
        {
            LoadingOpacity = 0;
            LoadingZIndex = -20;
        }
    }
}

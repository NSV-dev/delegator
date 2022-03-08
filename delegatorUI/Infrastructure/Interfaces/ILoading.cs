namespace delegatorUI.Infrastructure.Interfaces
{
    public interface ILoading
    {
        public int LoadingZIndex { get; set; }

        public void StartLoading()
        {
            LoadingZIndex = 20;
        }

        public void EndLoading()
        {
            LoadingZIndex = -20;
        }
    }
}

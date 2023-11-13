namespace UseCases.Document.MAUI.Models
{
    public struct UseCaseOption
    {
        public UseCaseOption(string title, Func<Task> action)
        {
            Title = title;
            NavigationAction = action;
        }

        public string Title { get; private set; }

        public Func<Task> NavigationAction { get; private set; }
    }
}

namespace RedmineBot.Helpers
{
    public enum IssueStatus : byte
    {
        New = 1,
        InWork,
        Done,
        OnPause,
        Closed
    }

    public enum Tracker : byte
    {
        Task = 5,
        SubTask = 10
    }

    public enum Activity : byte
    {
        Development = 9
    }
}

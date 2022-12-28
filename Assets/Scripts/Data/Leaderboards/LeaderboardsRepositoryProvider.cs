public static class LeaderboardsRepositoryProvider
{
    private static LeaderboardsRepository _repositoryInstance;

    public static LeaderboardsRepository Get()
    {
        if (_repositoryInstance == null)
        {
            _repositoryInstance = new();
        }

        return _repositoryInstance;
    }
}

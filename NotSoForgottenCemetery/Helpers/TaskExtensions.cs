namespace Cemetery
{
    public static class TaskExtensions
    {
        public static async void FireAndForgetSafeAsync(this Task task)
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}

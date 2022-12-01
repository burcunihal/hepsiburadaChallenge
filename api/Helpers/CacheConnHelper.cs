using StackExchange.Redis;
using StackExchange.Redis;


namespace api.Helpers
{
    public class CacheConnHelper
    {
        static CacheConnHelper()
        {
            CacheConnHelper.lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect("localhost:6379");
            });
        }
        private static Lazy<ConnectionMultiplexer> lazyConnection;
        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }

    }
}
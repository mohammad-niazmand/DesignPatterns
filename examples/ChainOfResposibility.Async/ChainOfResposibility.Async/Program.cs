using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ChainOfResposibility.Async
{
    enum LogMethod { Console, Debug, Trace, File, Db, None }

    abstract class AbstractLogger
    {
        public LogMethod Method { get; protected set; }
        public abstract void Log(string s);
        public AbstractLogger()
        {
            var logMethod = ConfigurationManager.AppSettings["LogMethod"].ToString();

            LogMethod lm;
            if (Enum.TryParse<LogMethod>(logMethod, out lm))
                this.Method = lm;
            else
                this.Method = LogMethod.None;
        }
    }
    class Logger : AbstractLogger
    {
        public override void Log(string s)
        {
            if (this.Method == LogMethod.Console)
                Console.WriteLine(s);
            else if (this.Method == LogMethod.Debug)
                Debug.WriteLine(s);
            else if (this.Method == LogMethod.Trace)
                Trace.WriteLine(s);
        }
        private static Logger instance;
        private Logger()
        { }
        static Logger()
        {
            instance = new Logger();
        }
        public static Logger GetInstance()
        {
            return instance;
        }
    }
    abstract class AbstractConfig
    {
        public string ConnectionString { get; protected set; }
        public string NetAddress { get; protected set; }
        public LogMethod LogMethod { get; protected set; }
        public string GetUrlForId(int id)
        {
            return String.Format(this.NetAddress + "/home/customer/{0}", id);
        }
    }
    class Config : AbstractConfig
    {
        private static Config instance;
        private Config()
        {
            var logger = ServiceLocator.GetLoggerService();
            logger.Log("Reading configuration");

            this.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnStr"].ToString();
            this.NetAddress = ConfigurationManager.AppSettings["NetAddress"].ToString();

            var logMethod = ConfigurationManager.AppSettings["LogMethod"].ToString();

            LogMethod lm;
            if (Enum.TryParse<LogMethod>(logMethod, out lm))
                this.LogMethod = lm;
            else
                this.LogMethod = LogMethod.None;
        }
        static Config()
        {
            instance = new Config();
        }
        public static Config GetInstance()
        {
            return instance;
        }
    }
    class ServiceLocator
    {
        public static AbstractConfig GetConfigService()
        {
            var a = Config.GetInstance();
            return (AbstractConfig)a;
        }
        public static AbstractLogger GetLoggerService()
        {
            return Logger.GetInstance();
        }
    }
    class Customer
    {
        public int ID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public Customer(int id, string p1, string p2)
        {
            this.ID = id;
            this.Firstname = p1;
            this.Lastname = p2;
        }
        public Customer() : this(0, "", "") { }
        public Customer(int id) : this(id, "", "") { }
        public Customer(string p1, string p2) : this(0, p1, p2) { }
    }
    abstract class Handler
    {
        protected Handler successor;
        public virtual async Task<Customer> GetCustomer(int id)
        {
            return null;
        }
        public Handler(Handler successor)
        {
            this.successor = successor;
        }
    }
    class DefaultHandler : Handler
    {
        public DefaultHandler()
            : base(null)
        { }
        public override async Task<Customer> GetCustomer(int id)
        {
            return new Customer();
        }
    }
    class CacheHandler : Handler
    {
        private ConcurrentDictionary<int, Customer> cache;
        public CacheHandler(Handler successor)
            : base(successor)
        {
            cache = new ConcurrentDictionary<int, Customer>();
        }
        public override async Task<Customer> GetCustomer(int id)
        {
            var logger = ServiceLocator.GetLoggerService();
            logger.Log("Checking cache for Customer(" + id.ToString() + ") ...");

            if (cache.ContainsKey(id))
            {
                logger.Log("Request handled by CacheHandler");
                return cache[id];
            }
            else
            {
                logger.Log("Cache is empty. Refering request to the next handler.");
                var result = await successor.GetCustomer(id);

                logger.Log("Updating cache ...");
                if (cache.TryAdd(id, result))
                    logger.Log("Cache updated.");
                else
                    logger.Log("Updating cache failed.");

                return result;
            }
        }
    }
    class DbHandler : Handler
    {
        public DbHandler(Handler successor) : base(successor) { }
        public override async Task<Customer> GetCustomer(int id)
        {
            Customer result = null;

            var config = ServiceLocator.GetConfigService();
            var logger = ServiceLocator.GetLoggerService();
            logger.Log("Checking database ...");

            try
            {
                using (var con = new SqlConnection(config.ConnectionString))
                {
                    con.Open();

                    var cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT Firstname, Lastname FROM Customer WHERE ID = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    var reader = await cmd.ExecuteReaderAsync();

                    if (reader.Read())
                    {
                        logger.Log("Data was retrieved from database.");
                        result = new Customer(id, reader[0].ToString(), reader[1].ToString());
                    }

                    reader.Close();

                    if (result == null)
                    {
                        logger.Log("Database is empty. Refering the request to the next handler.");
                        result = await successor.GetCustomer(id);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Log("Checking database encountered with error '" + e.Message + "'");
            }
            return result;
        }
    }
    class NetHandler : Handler
    {
        public NetHandler(Handler successor) : base(successor) { }
        public override async Task<Customer> GetCustomer(int id)
        {
            var logger = ServiceLocator.GetLoggerService();

            Customer result = null;
            try
            {
                logger.Log("Connecting the REST service ...");

                var config = ServiceLocator.GetConfigService();
                var wc = new WebClient();
                var uri = new Uri(config.GetUrlForId(id));
                string json = await wc.DownloadStringTaskAsync(uri);

                result = DeserializeJson(id, json);

                logger.Log("Data obtained from REST service. Updating database ...");
                await UpdateDb(result);
                logger.Log("Database updated.");
            }
            catch (Exception e)
            {
                logger.Log("NetHandler encountered with error.\n" + e.Message);
                result = successor.GetCustomer(id).Result;
            }

            return result;
        }
        private Customer DeserializeJson(int id, string json)
        {
            var result = new Customer(id);
            var reader = new JsonTextReader(new StringReader(json));
            var p1Read = false;
            var p2Read = false;
            var allRead = false;

            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        if (String.Compare(reader.Value.ToString(), "Firstname", true) == 0)
                            p1Read = true;
                        else if (String.Compare(reader.Value.ToString(), "Lastname", true) == 0)
                            p2Read = true;
                    }
                    if (reader.TokenType == JsonToken.String)
                    {
                        if (!allRead)
                        {
                            if (p1Read)
                            {
                                result.Firstname = reader.Value.ToString();
                                p1Read = false;
                            }
                            else if (p2Read)
                            {
                                result.Lastname = reader.Value.ToString();
                                allRead = true;
                            }
                        }
                    }
                }
            }
            return result;
        }
        private async Task UpdateDb(Customer data)
        {
            var config = ServiceLocator.GetConfigService();

            using (var con = new SqlConnection(config.ConnectionString))
            {
                con.Open();

                var cmd = new SqlCommand();

                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"INSERT INTO Customer(ID, Firstname, Lastname) VALUES (@id, @firstname, @lastname);";
                cmd.Parameters.AddWithValue("@id", data.ID);
                cmd.Parameters.AddWithValue("@firstname", data.Firstname);
                cmd.Parameters.AddWithValue("@lastname", data.Lastname);

                await cmd.ExecuteNonQueryAsync();

                return;
            }
        }
    }
    class Test
    {
        static void ShowCustomer(Customer customer)
        {
            Console.WriteLine();
            Console.WriteLine("Customer ID: {0}", customer.ID);
            Console.WriteLine("Customer Firstname: {0}", customer.Firstname);
            Console.WriteLine("Customer Lastname: {0}", customer.Lastname);
        }
        static void Main()
        {
            try
            {
                var h1 = new DefaultHandler();
                var h2 = new NetHandler(h1);
                var h3 = new DbHandler(h2);
                var h4 = new CacheHandler(h3);

                Customer customer;

                customer = h4.GetCustomer(1).Result;
                ShowCustomer(customer);
                customer = h4.GetCustomer(1).Result;
                ShowCustomer(customer);
                customer = h4.GetCustomer(1).Result;
                ShowCustomer(customer);
                customer = h4.GetCustomer(1).Result;
                ShowCustomer(customer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message);
            }
            //h4.GetCustomer(10);
            //h4.GetCustomer(10);

            Console.ReadKey();
        }
    }
    /* OUTPUT
    Checking cache for Customer(1) ...
    Cache is empty. Refering request to the next handler.
    Reading configuration
    Checking database ...
    Database is empty. Refering the request to the next handler.
    Connecting the REST service ...
    Data obtained from REST service. Updating database ...
    Database updated.
    Updating cache ...
    Cache updated.

    Customer ID: 1
    Customer Firstname: Hamed
    Customer Lastname: Karimi
    Checking cache for Customer(1) ...
    Request handled by CacheHandler

    Customer ID: 1
    Customer Firstname: Hamed
    Customer Lastname: Karimi
    Checking cache for Customer(1) ...
    Request handled by CacheHandler

    Customer ID: 1
    Customer Firstname: Hamed
    Customer Lastname: Karimi
    Checking cache for Customer(1) ...
    Request handled by CacheHandler

    Customer ID: 1
    Customer Firstname: Hamed
    Customer Lastname: Karimi 
    */
}

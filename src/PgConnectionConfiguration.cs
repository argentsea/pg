using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Npgsql;
using ArgentSea;
using System.Collections;

namespace ArgentSea.Pg
{
    /// <summary>
    /// This class represents is a (non-sharded) database connection.
    /// Note that the SecurityKey must match a defined key in the DataSecurityOptions; likewise, a DataResilienceKey (if defined) must match as key in the DataResilienceOptions array.
    /// If the DataResilienceKey is not defined, a default data resilience strategy will be used.
    /// </summary>
    public class PgConnectionConfiguration : IConnectionConfiguration
    {

        private readonly NpgsqlConnectionStringBuilder csb = new NpgsqlConnectionStringBuilder();

        public void SetSecurity(SecurityConfiguration security)
        {
            if (security.WindowsAuth)
            {
                this.csb.IntegratedSecurity = true;
            }
            else
            {
                this.csb.Username = security.UserName;
                this.csb.Password = security.Password;
                this.csb.IntegratedSecurity = false;
            }
        }

		public string ConnectionDescription
		{
			get => $"database {this.csb.Database} on host {this.csb.Host}, port {csb.Port}";
		}

		public string GetConnectionString()
            => this.csb.ToString();

        /// <summary>
        /// The optional application name parameter to be sent to the backend during connection initiation.
        /// </summary>
        public string ApplicationName
        {
            get => this.csb.ApplicationName;
            set => this.csb.ApplicationName = value; 
        }
        /// <summary>
        /// The minimum number of usages an SQL statement is used before it's automatically prepared. Defaults to 5
        /// </summary>
        public int AutoPrepareMinUsages
        {
            get => this.csb.AutoPrepareMinUsages;
            set => this.csb.AutoPrepareMinUsages = value;
        }
        /// <summary>
        /// Whether to check the certificate revocation list during authentication. False by default.
        /// </summary>
        public bool CheckCertificateRevocation
        {
            get => this.csb.CheckCertificateRevocation;
            set => this.csb.CheckCertificateRevocation = value;
        }
        /// <summary>
        /// Gets or sets the client_encoding parameter.
        /// </summary>
        public string ClientEncoding
        {
            get => this.csb.ClientEncoding;
            set => this.csb.ClientEncoding = value;
        }
        /// <summary>
        /// The time to wait (in seconds) while trying to execute a command before terminating the attempt and generating an error. Defaults to 30 seconds.
        /// </summary>
        public int CommandTimeout
        {
            get => this.csb.CommandTimeout;
            set => this.csb.CommandTimeout = value;
        }
        /// <summary>
        /// The time to wait before closing idle connections in the pool if the count of all connections exceeds MinPoolSize.
        /// </summary>
        public int ConnectionIdleLifetime
        {
            get => this.csb.ConnectionIdleLifetime;
            set => this.csb.ConnectionIdleLifetime = value;
        }
        /// <summary>
        /// How many seconds the pool waits before attempting to prune idle connections that are beyond idle lifetime.
        /// </summary>
        public int ConnectionPruningInterval
        {
            get => this.csb.ConnectionPruningInterval;
            set => this.csb.ConnectionPruningInterval = value;
        }
        /// <summary>
        /// Makes MaxValue and MinValue timestamps and dates readable as infinity and negative infinity.
        /// </summary>
        public bool ConvertInfinityDateTime
        {
            get => this.csb.ConvertInfinityDateTime;
            set => this.csb.ConvertInfinityDateTime = value;
        }
        /// <summary>
        /// The PostgreSQL database to connect to.
        /// </summary>
        public string Database
        {
            get => this.csb.Database;
            set => this.csb.Database = value;
        }
        /// <summary>
        /// Gets or sets the .NET encoding that will be used to encode/decode PostgreSQL string data.
        /// </summary>
        public string Encoding
        {
            get => this.csb.Encoding;
            set => this.csb.Encoding = value;
        }
        /// <summary>
        /// Whether to enlist in an ambient TransactionScope.
        /// </summary>
        public bool Enlist
        {
            get => this.csb.Enlist;
            set => this.csb.Enlist = value;
        }
        /// <summary>
        /// The hostname or IP address of the PostgreSQL server to connect to.
        /// </summary>
        public string Host
        {
            get => this.csb.Host;
            set => this.csb.Host = value;
        }
        /// <summary>
        /// The Kerberos realm to be used for authentication
        /// </summary>
        public bool IncludeRealm
        {
            get => this.csb.IncludeRealm;
            set => this.csb.IncludeRealm = value;
        }
        /// <summary>
        /// The time to wait (in seconds) while trying to execute a an internal command before terminating the attempt and generating an error.
        /// </summary>
        public int InternalCommandTimeout
        {
            get => this.csb.InternalCommandTimeout;
            set => this.csb.InternalCommandTimeout = value;
        }
        /// <summary>
        /// The number of seconds of connection inactivity before Npgsql sends a keepalive query. Set to 0 (the default) to disable.
        /// </summary>
        public int KeepAlive
        {
            get => this.csb.KeepAlive;
            set => this.csb.KeepAlive = value;
        }
        /// <summary>
        /// The Kerberos service name to be used for authentication.
        /// </summary>
        public string KerberosServiceName
        {
            get => this.csb.KerberosServiceName;
            set => this.csb.KerberosServiceName = value;
        }
        /// <summary>
        /// The maximum number SQL statements that can be automatically prepared at any given point. Beyond this number the least-recently-used statement will be recycled. Zero (the default) disables automatic preparation.
        /// </summary>
        public int MaxAutoPrepare
        {
            get => this.csb.MaxAutoPrepare;
            set => this.csb.MaxAutoPrepare = value;
        }
        /// <summary>
        /// The maximum connection pool size.
        /// </summary>
        public int MaxPoolSize
        {
            get => this.csb.MaxPoolSize;
            set => this.csb.MaxPoolSize = value;
        }
        /// <summary>
        /// The minimum connection pool size.
        /// </summary>
        public int MinPoolSize
        {
            get => this.csb.MinPoolSize;
            set => this.csb.MinPoolSize = value;
        }
        /// <summary>
        /// If set to true, a pool connection's state won't be reset when it is closed (improves performance). Do not specify this unless you know what you're doing.
        /// </summary>
        public bool NoResetOnClose
        {
            get => this.csb.NoResetOnClose;
            set => this.csb.NoResetOnClose = value;
        }
        /// <summary>
        /// Gets or sets a Boolean value that indicates if security-sensitive information, such as the password, is not returned as part of the connection if the connection is open or has ever been in an open state.
        /// </summary>
        public bool PersistSecurityInfo
        {
            get => this.csb.PersistSecurityInfo;
            set => this.csb.PersistSecurityInfo = value;
        }
        /// <summary>
        /// Whether connection pooling should be used.
        /// </summary>
        public bool Pooling
        {
            get => this.csb.Pooling;
            set => this.csb.Pooling = value;
        }
        /// <summary>
        /// The TCP/IP port of the PostgreSQL server.
        /// </summary>
        public int Port
        {
            get => this.csb.Port;
            set => this.csb.Port = value;
        }
        /// <summary>
        /// Determines the size of the internal buffer Npgsql uses when reading. Increasing may improve performance if transferring large values from the database.
        /// </summary>
        public int ReadBufferSize
        {
            get => this.csb.ReadBufferSize;
            set => this.csb.ReadBufferSize = value;
        }
        /// <summary>
        /// Gets or sets the schema search path.
        /// </summary>
        public string SearchPath
        {
            get => this.csb.SearchPath;
            set => this.csb.SearchPath = value;
        }
        /// <summary>
        /// A compatibility mode for special PostgreSQL server types.
        /// </summary>
        public ServerCompatibilityMode ServerCompatibilityMode
        {
            get => this.csb.ServerCompatibilityMode;
            set => this.csb.ServerCompatibilityMode = value;
        }
        /// <summary>
        /// Determines the size of socket read buffer.
        /// </summary>
        public int SocketReceiveBufferSize
        {
            get => this.csb.SocketReceiveBufferSize;
            set => this.csb.SocketReceiveBufferSize = value;
        }
        /// <summary>
        /// Determines the size of socket send buffer.
        /// </summary>
        public int SocketSendBufferSize
        {
            get => this.csb.SocketSendBufferSize;
            set => this.csb.SocketSendBufferSize = value;
        }
        /// <summary>
        /// Controls whether SSL is required, disabled or preferred, depending on server support.
        /// </summary>
        public SslMode SslMode {
            get => this.csb.SslMode;
            set => this.csb.SslMode = value;
        }

        /// <summary>
        /// The interval, in milliseconds, between when successive keep-alive packets are sent if no acknowledgement is received. Defaults to the value of TcpKeepAliveTime. TcpKeepAliveTime must be non-zero as well. Supported only on Windows.
        /// </summary>
        public int TcpKeepAliveInterval
        {
            get => this.csb.TcpKeepAliveInterval;
            set => this.csb.TcpKeepAliveInterval = value;
        }
        /// <summary>
        /// The number of seconds of connection inactivity before a TCP keepalive query is sent. Use of this option is discouraged, use KeepAlive instead if possible. Set to 0 (the default) to disable. Supported only on Windows.
        /// </summary>
        public int TcpKeepAliveTime
        {
            get => this.csb.TcpKeepAliveTime;
            set => this.csb.TcpKeepAliveTime = value;
        }
        /// <summary>
        /// The time to wait (in seconds) while trying to establish a connection before terminating the attempt and generating an error. Defaults to 15 seconds.
        /// </summary>
        public int Timeout
        {
            get => this.csb.Timeout;
            set => this.csb.Timeout = value;
        }
        /// <summary>
        /// Whether to trust the server certificate without validating it.
        /// </summary>
        public bool TrustServerCertificate
        {
            get => this.csb.TrustServerCertificate;
            set => this.csb.TrustServerCertificate = value;
        }

        /// <summary>
        /// Writes connection performance information to performance counters.
        /// </summary>
        public bool UsePerfCounters
        {
            get => this.csb.UsePerfCounters;
            set => this.csb.UsePerfCounters = value;
        }
        /// <summary>
        /// Npgsql uses its own internal implementation of TLS/SSL. Turn this on to use .NET SslStream instead.
        /// </summary>
        public bool UseSslStream
        {
            get => this.csb.UseSslStream;
            set => this.csb.UseSslStream = value;
        }

        /// <summary>
        /// Determines the size of the internal buffer Npgsql uses when writing. Increasing may improve performance if transferring large values to the database.
        /// </summary>
        public int WriteBufferSize
        {
            get => this.csb.WriteBufferSize;
            set => this.csb.WriteBufferSize = value;
        }

        /// <summary>
        /// Adds an item to the configuration
        /// </summary>
        /// <param name="item"></param>
        public void Add(KeyValuePair<string, object> item)
        {
            this.csb.Add(item);
        }
        /// <summary>
        /// Determines whether the configuration contains a specific key-value pair.
        /// </summary>
        public bool Contains(KeyValuePair<string, object> item)
        {
            return this.csb.Contains(item);
        }
        /// <summary>
        /// Determines whether the configuration contains a specific key.
        /// </summary>
        public bool ContainsKey(string key)
        {
            return this.csb.ContainsKey(key);
        }


        /// <summary>
        /// Removes the entry from the configuration instance.
        /// </summary>
        public void Remove(KeyValuePair<string, object> item)
        {
            this.csb.Remove(item);
        }

        /// <summary>
        /// Removes the entry from the configuration instance.
        /// </summary>
        public void Remove(string key)
        {
            this.csb.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return this.csb.TryGetValue(key, out value);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Npgsql;
using ArgentSea;
using System.Collections;
using Microsoft.Extensions.Options;

namespace ArgentSea.Pg
{
    /// <summary>
    /// This class represents is a (non-sharded) database connection.
    /// Note that the SecurityKey must match a defined key in the DataSecurityOptions; likewise, a ResilienceKey (if defined) must match as key in the DataResilienceOptions array.
    /// If the ResilienceKey is not defined, a default data resilience strategy will be used.
    /// </summary>
    public class PgConnectionConfiguration :  DataConnectionConfigurationBase
    {

        private readonly NpgsqlConnectionStringBuilder _csb = new NpgsqlConnectionStringBuilder();
        private string _connectionString = null;

		public override string ConnectionDescription
		{
			get => $"database {this._csb.Database} on host {this._csb.Host}, port {_csb.Port}";
		}

		public override string GetConnectionString()
        {
            if (hasConnectionPropertyChanged && string.IsNullOrEmpty(_connectionString))
            {
                var security = base.GetSecurityConfiguration();
                if (!(security is null))
                {
                    if (security.WindowsAuth)
                    {
                        this._csb.IntegratedSecurity = true;
                    }
                    else
                    {
                        this._csb.Username = security.UserName;
                        this._csb.Password = security.Password;
                        this._csb.IntegratedSecurity = false;
                    }
                }
                _connectionString = _csb.ToString();
                hasConnectionPropertyChanged = false;
            }
            return _connectionString;
        }

        /// <summary>
        /// The optional application name parameter to be sent to the backend during connection initiation.
        /// </summary>
        public string ApplicationName
        {
            get => this._csb.ApplicationName;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.ApplicationName = value;
            }
        }
        /// <summary>
        /// The minimum number of usages an SQL statement is used before it's automatically prepared. Defaults to 5
        /// </summary>
        public int AutoPrepareMinUsages
        {
            get => this._csb.AutoPrepareMinUsages;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.AutoPrepareMinUsages = value;
            }
        }
        /// <summary>
        /// Whether to check the certificate revocation list during authentication. False by default.
        /// </summary>
        public bool CheckCertificateRevocation
        {
            get => this._csb.CheckCertificateRevocation;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.CheckCertificateRevocation = value;
            }
        }
        /// <summary>
        /// Gets or sets the client_encoding parameter.
        /// </summary>
        public string ClientEncoding
        {
            get => this._csb.ClientEncoding;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.ClientEncoding = value;
            }
        }
        /// <summary>
        /// The time to wait (in seconds) while trying to execute a command before terminating the attempt and generating an error. Defaults to 30 seconds.
        /// </summary>
        public int CommandTimeout
        {
            get => this._csb.CommandTimeout;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.CommandTimeout = value;
            }
        }
        /// <summary>
        /// The time to wait before closing idle connections in the pool if the count of all connections exceeds MinPoolSize.
        /// </summary>
        public int ConnectionIdleLifetime
        {
            get => this._csb.ConnectionIdleLifetime;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.ConnectionIdleLifetime = value;
            }
        }
        /// <summary>
        /// How many seconds the pool waits before attempting to prune idle connections that are beyond idle lifetime.
        /// </summary>
        public int ConnectionPruningInterval
        {
            get => this._csb.ConnectionPruningInterval;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.ConnectionPruningInterval = value;
            }
        }
        /// <summary>
        /// Makes MaxValue and MinValue timestamps and dates readable as infinity and negative infinity.
        /// </summary>
        public bool ConvertInfinityDateTime
        {
            get => this._csb.ConvertInfinityDateTime;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.ConvertInfinityDateTime = value;
            }
        }
        /// <summary>
        /// The PostgreSQL database to connect to.
        /// </summary>
        public string Database
        {
            get => this._csb.Database;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.Database = value;
            }
        }
        /// <summary>
        /// Gets or sets the .NET encoding that will be used to encode/decode PostgreSQL string data.
        /// </summary>
        public string Encoding
        {
            get => this._csb.Encoding;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.Encoding = value;
            }
        }
        /// <summary>
        /// Whether to enlist in an ambient TransactionScope.
        /// </summary>
        public bool Enlist
        {
            get => this._csb.Enlist;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.Enlist = value;
            }
        }
        /// <summary>
        /// The hostname or IP address of the PostgreSQL server to connect to.
        /// </summary>
        public string Host
        {
            get => this._csb.Host;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.Host = value;
            }
        }
        /// <summary>
        /// The Kerberos realm to be used for authentication
        /// </summary>
        public bool IncludeRealm
        {
            get => this._csb.IncludeRealm;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.IncludeRealm = value;
            }
        }
        /// <summary>
        /// The time to wait (in seconds) while trying to execute a an internal command before terminating the attempt and generating an error.
        /// </summary>
        public int InternalCommandTimeout
        {
            get => this._csb.InternalCommandTimeout;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.InternalCommandTimeout = value;
            }
        }
        /// <summary>
        /// The number of seconds of connection inactivity before Npgsql sends a keepalive query. Set to 0 (the default) to disable.
        /// </summary>
        public int KeepAlive
        {
            get => this._csb.KeepAlive;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.KeepAlive = value;
            }
        }
        /// <summary>
        /// The Kerberos service name to be used for authentication.
        /// </summary>
        public string KerberosServiceName
        {
            get => this._csb.KerberosServiceName;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.KerberosServiceName = value;
            }
        }
        /// <summary>
        /// The maximum number SQL statements that can be automatically prepared at any given point. Beyond this number the least-recently-used statement will be recycled. Zero (the default) disables automatic preparation.
        /// </summary>
        public int MaxAutoPrepare
        {
            get => this._csb.MaxAutoPrepare;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.MaxAutoPrepare = value;
            }
        }
        /// <summary>
        /// The maximum connection pool size.
        /// </summary>
        public int MaxPoolSize
        {
            get => this._csb.MaxPoolSize;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.MaxPoolSize = value;
            }
        }
        /// <summary>
        /// The minimum connection pool size.
        /// </summary>
        public int MinPoolSize
        {
            get => this._csb.MinPoolSize;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.MinPoolSize = value;
            }
        }
        /// <summary>
        /// If set to true, a pool connection's state won't be reset when it is closed (improves performance). Do not specify this unless you know what you're doing.
        /// </summary>
        public bool NoResetOnClose
        {
            get => this._csb.NoResetOnClose;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.NoResetOnClose = value;
            }
        }
        /// <summary>
        /// Gets or sets a Boolean value that indicates if security-sensitive information, such as the password, is not returned as part of the connection if the connection is open or has ever been in an open state.
        /// </summary>
        public bool PersistSecurityInfo
        {
            get => this._csb.PersistSecurityInfo;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.PersistSecurityInfo = value;
            }
        }
        /// <summary>
        /// Whether connection pooling should be used.
        /// </summary>
        public bool Pooling
        {
            get => this._csb.Pooling;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.Pooling = value;
            }
        }
        /// <summary>
        /// The TCP/IP port of the PostgreSQL server.
        /// </summary>
        public int Port
        {
            get => this._csb.Port;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.Port = value;
            }
        }
        /// <summary>
        /// Determines the size of the internal buffer Npgsql uses when reading. Increasing may improve performance if transferring large values from the database.
        /// </summary>
        public int ReadBufferSize
        {
            get => this._csb.ReadBufferSize;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.ReadBufferSize = value;
            }
        }
        /// <summary>
        /// Gets or sets the schema search path.
        /// </summary>
        public string SearchPath
        {
            get => this._csb.SearchPath;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.SearchPath = value;
            }
        }
        /// <summary>
        /// A compatibility mode for special PostgreSQL server types.
        /// </summary>
        public ServerCompatibilityMode ServerCompatibilityMode
        {
            get => this._csb.ServerCompatibilityMode;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.ServerCompatibilityMode = value;
            }
        }
        /// <summary>
        /// Determines the size of socket read buffer.
        /// </summary>
        public int SocketReceiveBufferSize
        {
            get => this._csb.SocketReceiveBufferSize;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.SocketReceiveBufferSize = value;
            }
        }
        /// <summary>
        /// Determines the size of socket send buffer.
        /// </summary>
        public int SocketSendBufferSize
        {
            get => this._csb.SocketSendBufferSize;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.SocketSendBufferSize = value;
            }
        }
        /// <summary>
        /// Controls whether SSL is required, disabled or preferred, depending on server support.
        /// </summary>
        public SslMode SslMode {
            get => this._csb.SslMode;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.SslMode = value;
            }
        }

        /// <summary>
        /// The interval, in milliseconds, between when successive keep-alive packets are sent if no acknowledgement is received. Defaults to the value of TcpKeepAliveTime. TcpKeepAliveTime must be non-zero as well. Supported only on Windows.
        /// </summary>
        public int TcpKeepAliveInterval
        {
            get => this._csb.TcpKeepAliveInterval;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.TcpKeepAliveInterval = value;
            }
        }
        /// <summary>
        /// The number of seconds of connection inactivity before a TCP keepalive query is sent. Use of this option is discouraged, use KeepAlive instead if possible. Set to 0 (the default) to disable. Supported only on Windows.
        /// </summary>
        public int TcpKeepAliveTime
        {
            get => this._csb.TcpKeepAliveTime;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.TcpKeepAliveTime = value;
            }
        }
        /// <summary>
        /// The time to wait (in seconds) while trying to establish a connection before terminating the attempt and generating an error. Defaults to 15 seconds.
        /// </summary>
        public int Timeout
        {
            get => this._csb.Timeout;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.Timeout = value;
            }
        }
        /// <summary>
        /// Whether to trust the server certificate without validating it.
        /// </summary>
        public bool TrustServerCertificate
        {
            get => this._csb.TrustServerCertificate;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.TrustServerCertificate = value;
            }
        }

        /// <summary>
        /// Writes connection performance information to performance counters.
        /// </summary>
        public bool UsePerfCounters
        {
            get => this._csb.UsePerfCounters;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.UsePerfCounters = value;
            }
        }
        /// <summary>
        /// Npgsql uses its own internal implementation of TLS/SSL. Turn this on to use .NET SslStream instead.
        /// </summary>
        public bool UseSslStream
        {
            get => this._csb.UseSslStream;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.UseSslStream = value;
            }
        }

        /// <summary>
        /// Determines the size of the internal buffer Npgsql uses when writing. Increasing may improve performance if transferring large values to the database.
        /// </summary>
        public int WriteBufferSize
        {
            get => this._csb.WriteBufferSize;
            set
            {
                hasConnectionPropertyChanged = true;
                this._csb.WriteBufferSize = value;
            }
        }

        /// <summary>
        /// Adds an item to the configuration
        /// </summary>
        /// <param name="item"></param>
        public void Add(KeyValuePair<string, object> item)
        {
            this._csb.Add(item);
        }
        /// <summary>
        /// Determines whether the configuration contains a specific key-value pair.
        /// </summary>
        public bool Contains(KeyValuePair<string, object> item)
        {
            return this._csb.Contains(item);
        }
        /// <summary>
        /// Determines whether the configuration contains a specific key.
        /// </summary>
        public bool ContainsKey(string key)
        {
            return this._csb.ContainsKey(key);
        }


        /// <summary>
        /// Removes the entry from the configuration instance.
        /// </summary>
        public void Remove(KeyValuePair<string, object> item)
        {
            this._csb.Remove(item);
        }

        /// <summary>
        /// Removes the entry from the configuration instance.
        /// </summary>
        public void Remove(string key)
        {
            this._csb.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return this._csb.TryGetValue(key, out value);
        }
    }
}

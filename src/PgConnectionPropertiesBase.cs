// © John Hicks. All rights reserved. Licensed under the MIT license.
// See the LICENSE file in the repository root for more information.

using Npgsql;

namespace ArgentSea.Pg
{
    public abstract class PgConnectionPropertiesBase : DataConnectionConfigurationBase
    {

        private string _applicationName = null;
        private int? _autoPrepareMinUsages = null;
        private bool? _checkCertificateRevocation = null;
        private string _clientEncoding = null;
        private int? _commandTimeout = null;
        private int? _connectionIdleLifetime = null;
        private int? _connectionPruningInterval = null;
        private bool? _convertInfinityDateTime = null;
        private string _database = null;
        private string _encoding = null;
        private bool? _enlist = null;
        private string _host = null;
        private bool? _includeRealm = null;
        private int? _internalCommandTimeout = null;
        private int? _keepAlive = null;
        private string _kerberosServiceName = null;
        private bool? _loadTableComposites = null;
        private int? _maxAutoPrepare = null;
        private int? _maxPoolSize = null;
        private int? _minPoolSize = null;
        private bool? _noResetOnClose = null;
        private bool? _persistSecurityInfo = null;
        private bool? _pooling = null;
        private int? _port = null;
        private int? _readBufferSize = null;
        private string _searchPath = null;
        private ServerCompatibilityMode? _serverCompatibilityMode = null;
        private int? _socketReceiveBufferSize = null;
        private int? _socketSendBufferSize = null;
        private SslMode? _sslMode = null;
        private bool? _tcpKeepAlive = null;
        private int? _tcpKeepAliveInterval = null;
        private int? _tcpKeepAliveTime = null;
        private int? _timeout = null;
        private string _timezone = null;
        private bool? _trustServerCertificate = null;
        private bool? _usePerfCounters = null;
        private bool? _useSslStream = null;
        private int? _writeBufferSize = null;

        /// <summary>
        /// The optional application name parameter to be sent to the backend during connection initiation.
        /// </summary>
        public string ApplicationName
        {
            get { return _applicationName; }
            set
            {
                if (_applicationName != value)
                {
                    _applicationName = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// The minimum number of usages an SQL statement is used before it's automatically prepared. Defaults to 5
        /// </summary>
        public int? AutoPrepareMinUsages
        {
            get { return _autoPrepareMinUsages; }
            set
            {
                if (_autoPrepareMinUsages != value)
                {
                    _autoPrepareMinUsages = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Whether to check the certificate revocation list during authentication. False by default.
        /// </summary>
        public bool? CheckCertificateRevocation
        {
            get { return _checkCertificateRevocation; }
            set
            {
                if (_checkCertificateRevocation != value)
                {
                    _checkCertificateRevocation = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the client_encoding parameter.
        /// </summary>
        public string ClientEncoding
        {
            get { return _clientEncoding; }
            set
            {
                if (_clientEncoding != value)
                {
                    _clientEncoding = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// The time to wait (in seconds) while trying to execute a command before terminating the attempt and generating an error. Defaults to 30 seconds.
        /// </summary>
        public int? CommandTimeout
        {
            get { return _commandTimeout; }
            set
            {
                if (_commandTimeout != value)
                {
                    _commandTimeout = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// The time to wait before closing idle connections in the pool if the count of all connections exceeds MinPoolSize.
        /// </summary>
        public int? ConnectionIdleLifetime
        {
            get { return _connectionIdleLifetime; }
            set
            {
                if (_connectionIdleLifetime != value)
                {
                    _connectionIdleLifetime = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// How many seconds the pool waits before attempting to prune idle connections that are beyond idle lifetime.
        /// </summary>
        public int? ConnectionPruningInterval
        {
            get { return _connectionPruningInterval; }
            set
            {
                if (_connectionPruningInterval != value)
                {
                    _connectionPruningInterval = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Makes MaxValue and MinValue timestamps and dates readable as infinity and negative infinity.
        /// </summary>
        public bool? ConvertInfinityDateTime
        {
            get { return _convertInfinityDateTime; }
            set
            {
                if (_convertInfinityDateTime != value)
                {
                    _convertInfinityDateTime = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// The PostgreSQL database to connect to.
        /// </summary>
        public string Database
        {
            get { return _database; }
            set
            {
                if (_database != value)
                {
                    _database = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the .NET encoding that will be used to encode/decode PostgreSQL string data.
        /// </summary>
        public string Encoding
        {
            get { return _encoding; }
            set
            {
                if (_encoding != value)
                {
                    _encoding = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Whether to enlist in an ambient TransactionScope.
        /// </summary>
        public bool? Enlist
        {
            get { return _enlist; }
            set
            {
                if (_enlist != value)
                {
                    _enlist = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// The hostname or IP address of the PostgreSQL server to connect to.
        /// </summary>
        public string Host
        {
            get { return _host; }
            set
            {
                if (_host != value)
                {
                    _host = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// The Kerberos realm to be used for authentication
        /// </summary>
        public bool? IncludeRealm
        {
            get { return _includeRealm; }
            set
            {
                if (_includeRealm != value)
                {
                    _includeRealm = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// The time to wait (in seconds) while trying to execute a an internal command before terminating the attempt and generating an error.
        /// </summary>
        public int? InternalCommandTimeout
        {
            get { return _internalCommandTimeout; }
            set
            {
                if (_internalCommandTimeout != value)
                {
                    _internalCommandTimeout = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// The number of seconds of connection inactivity before Npgsql sends a keepalive query. Set to 0 (the default) to disable.
        /// </summary>
        public int? KeepAlive
        {
            get { return _keepAlive; }
            set
            {
                if (_keepAlive != value)
                {
                    _keepAlive = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// The Kerberos service name to be used for authentication.
        /// </summary>
        public string KerberosServiceName
        {
            get { return _kerberosServiceName; }
            set
            {
                if (_kerberosServiceName != value)
                {
                    _kerberosServiceName = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Load table composite type definitions, and not just free-standing composite types.
        /// </summary>
        public bool? LoadTableComposites
        {
            get { return _loadTableComposites; }
            set
            {
                if (_loadTableComposites != value)
                {
                    _loadTableComposites = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// The maximum number SQL statements that can be automatically prepared at any given point. Beyond this number the least-recently-used statement will be recycled. Zero (the default) disables automatic preparation.
        /// </summary>
        public int? MaxAutoPrepare
        {
            get { return _maxAutoPrepare; }
            set
            {
                if (_maxAutoPrepare != value)
                {
                    _maxAutoPrepare = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// The maximum connection pool size.
        /// </summary>
        public int? MaxPoolSize
        {
            get { return _maxPoolSize; }
            set
            {
                if (_maxPoolSize != value)
                {
                    _maxPoolSize = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// The minimum connection pool size.
        /// </summary>
        public int? MinPoolSize
        {
            get { return _minPoolSize; }
            set
            {
                if(_minPoolSize != value)
                {
                    _minPoolSize = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// If set to true, a pool connection's state won't be reset when it is closed (improves performance). Do not specify this unless you know what you're doing.
        /// </summary>
        public bool? NoResetOnClose
        {
            get { return _noResetOnClose; }
            set
            {
                if (_noResetOnClose != value)
                {
                    _noResetOnClose = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a Boolean value that indicates if security-sensitive information, such as the password, is not returned as part of the connection if the connection is open or has ever been in an open state.
        /// </summary>
        public bool? PersistSecurityInfo
        {
            get { return _persistSecurityInfo; }
            set
            {
                if (_persistSecurityInfo != value)
                {
                    _persistSecurityInfo = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Whether connection pooling should be used.
        /// </summary>
        public bool? Pooling
        {
            get { return _pooling; }
            set
            {
                if (_pooling != value)
                {
                    _pooling = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// The TCP/IP port of the PostgreSQL server.
        /// </summary>
        public int? Port
        {
            get { return _port; }
            set
            {
                if (_port != value)
                {
                    _port = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Determines the size of the internal buffer Npgsql uses when reading. Increasing may improve performance if transferring large values from the database.
        /// </summary>
        public int? ReadBufferSize
        {
            get { return _readBufferSize; }
            set
            {
                if (_readBufferSize != value)
                {
                    _readBufferSize = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the schema search path.
        /// </summary>
        public string SearchPath
        {
            get { return _searchPath; }
            set
            {
                if (_searchPath != value)
                {
                    _searchPath = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// A compatibility mode for special PostgreSQL server types.
        /// </summary>
        public ServerCompatibilityMode? ServerCompatibilityMode
        {
            get { return _serverCompatibilityMode; }
            set
            {
                if (_serverCompatibilityMode != value)
                {
                    _serverCompatibilityMode = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Determines the size of socket read buffer.
        /// </summary>
        public int? SocketReceiveBufferSize
        {
            get { return _socketReceiveBufferSize; }
            set
            {
                if (_socketReceiveBufferSize != value)
                {
                    _socketReceiveBufferSize = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Determines the size of socket send buffer.
        /// </summary>
        public int? SocketSendBufferSize
        {
            get { return _socketSendBufferSize; }
            set
            {
                if (_socketSendBufferSize != value)
                {
                    _socketSendBufferSize = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Controls whether SSL is required, disabled or preferred, depending on server support.
        /// </summary>
        public SslMode? SslMode
        {
            get { return _sslMode; }
            set
            {
                if (_sslMode != value)
                {
                    _sslMode = value;
                    RaisePropertyChanged();

                }
            }
        }

        /// <summary>
        /// Whether to use TCP keepalive with system defaults if overrides isn't specified.
        /// </summary>
        public bool? TcpKeepAlive
        {
            get { return _tcpKeepAlive; }
            set
            {
                if (_tcpKeepAlive != value)
                {
                    _tcpKeepAlive = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// The interval, in milliseconds, between when successive keep-alive packets are sent if no acknowledgement is received. Defaults to the value of TcpKeepAliveTime. TcpKeepAliveTime must be non-zero as well. Supported only on Windows.
        /// </summary>
        public int? TcpKeepAliveInterval
        {
            get { return _tcpKeepAliveInterval; }
            set
            {
                if (_tcpKeepAliveInterval != value)
                {
                    _tcpKeepAliveInterval = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// The number of seconds of connection inactivity before a TCP keepalive query is sent. Use of this option is discouraged, use KeepAlive instead if possible. Set to 0 (the default) to disable. Supported only on Windows.
        /// </summary>
        public int? TcpKeepAliveTime
        {
            get { return _tcpKeepAliveTime; }
            set
            {
                if (_tcpKeepAliveTime != value)
                {
                    _tcpKeepAliveTime = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// The time to wait (in seconds) while trying to establish a connection before terminating the attempt and generating an error. Defaults to 15 seconds.
        /// </summary>
        public int? Timeout
        {
            get { return _timeout; }
            set
            {
                if (_timeout != value)
                {
                    _timeout = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the session timezone, PGTZ environment variable can be used instead.
        /// </summary>
        public string Timezone
        {
            get { return _timezone; }
            set
            {
                if (_timezone != value)
                {
                    _timezone = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Whether to trust the server certificate without validating it.
        /// </summary>
        public bool? TrustServerCertificate
        {
            get { return _trustServerCertificate; }
            set
            {
                if (_trustServerCertificate != value)
                {
                    _trustServerCertificate = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Writes connection performance information to performance counters.
        /// </summary>
        public bool? UsePerfCounters
        {
            get { return _usePerfCounters; }
            set
            {
                if (_usePerfCounters != value)
                {
                    _usePerfCounters = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Npgsql uses its own internal implementation of TLS/SSL. Turn this on to use .NET SslStream instead.
        /// </summary>
        public bool? UseSslStream
        {
            get { return _useSslStream; }
            set
            {
                if (_useSslStream != value)
                {
                    _useSslStream = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Determines the size of the internal buffer Npgsql uses when writing. Increasing may improve performance if transferring large values to the database.
        /// </summary>
        public int? WriteBufferSize
        {
            get { return _writeBufferSize; }
            set
            {
                if (_writeBufferSize != value)
                {
                    _writeBufferSize = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}

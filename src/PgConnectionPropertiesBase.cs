// © John Hicks. All rights reserved. Licensed under the MIT license.
// See the LICENSE file in the repository root for more information.

using Npgsql;

namespace ArgentSea.Pg
{
    public abstract class PgConnectionPropertiesBase : DataConnectionConfigurationBase
    {
        /// <summary>
        /// The optional application name parameter to be sent to the backend during connection initiation.
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// The minimum number of usages an SQL statement is used before it's automatically prepared. Defaults to 5
        /// </summary>
        public int? AutoPrepareMinUsages { get; set; }

        /// <summary>
        /// Whether to check the certificate revocation list during authentication. False by default.
        /// </summary>
        public bool? CheckCertificateRevocation { get; set; }

        /// <summary>
        /// Gets or sets the client_encoding parameter.
        /// </summary>
        public string ClientEncoding { get; set; }

        /// <summary>
        /// The time to wait (in seconds) while trying to execute a command before terminating the attempt and generating an error. Defaults to 30 seconds.
        /// </summary>
        public int? CommandTimeout { get; set; }

        /// <summary>
        /// The time to wait before closing idle connections in the pool if the count of all connections exceeds MinPoolSize.
        /// </summary>
        public int? ConnectionIdleLifetime { get; set; }

        /// <summary>
        /// How many seconds the pool waits before attempting to prune idle connections that are beyond idle lifetime.
        /// </summary>
        public int? ConnectionPruningInterval { get; set; }

        /// <summary>
        /// Makes MaxValue and MinValue timestamps and dates readable as infinity and negative infinity.
        /// </summary>
        public bool? ConvertInfinityDateTime { get; set; }

        /// <summary>
        /// The PostgreSQL database to connect to.
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Gets or sets the .NET encoding that will be used to encode/decode PostgreSQL string data.
        /// </summary>
        public string Encoding { get; set; }

        /// <summary>
        /// Whether to enlist in an ambient TransactionScope.
        /// </summary>
        public bool? Enlist { get; set; }

        /// <summary>
        /// The hostname or IP address of the PostgreSQL server to connect to.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// The Kerberos realm to be used for authentication
        /// </summary>
        public bool? IncludeRealm { get; set; }

        /// <summary>
        /// The time to wait (in seconds) while trying to execute a an internal command before terminating the attempt and generating an error.
        /// </summary>
        public int? InternalCommandTimeout { get; set; }

        /// <summary>
        /// The number of seconds of connection inactivity before Npgsql sends a keepalive query. Set to 0 (the default) to disable.
        /// </summary>
        public int? KeepAlive { get; set; }

        /// <summary>
        /// The Kerberos service name to be used for authentication.
        /// </summary>
        public string KerberosServiceName { get; set; }

        /// <summary>
        /// Load table composite type definitions, and not just free-standing composite types.
        /// </summary>
        public bool? LoadTableComposites { get; set; }

        /// <summary>
        /// The maximum number SQL statements that can be automatically prepared at any given point. Beyond this number the least-recently-used statement will be recycled. Zero (the default) disables automatic preparation.
        /// </summary>
        public int? MaxAutoPrepare { get; set; }

        /// <summary>
        /// The maximum connection pool size.
        /// </summary>
        public int? MaxPoolSize { get; set; }

        /// <summary>
        /// The minimum connection pool size.
        /// </summary>
        public int? MinPoolSize { get; set; }

        /// <summary>
        /// If set to true, a pool connection's state won't be reset when it is closed (improves performance). Do not specify this unless you know what you're doing.
        /// </summary>
        public bool? NoResetOnClose { get; set; }

        /// <summary>
        /// Gets or sets a Boolean value that indicates if security-sensitive information, such as the password, is not returned as part of the connection if the connection is open or has ever been in an open state.
        /// </summary>
        public bool? PersistSecurityInfo { get; set; }

        /// <summary>
        /// Whether connection pooling should be used.
        /// </summary>
        public bool? Pooling { get; set; }

        /// <summary>
        /// The TCP/IP port of the PostgreSQL server.
        /// </summary>
        public int? Port { get; set; }

        /// <summary>
        /// Determines the size of the internal buffer Npgsql uses when reading. Increasing may improve performance if transferring large values from the database.
        /// </summary>
        public int? ReadBufferSize { get; set; }

        /// <summary>
        /// Gets or sets the schema search path.
        /// </summary>
        public string SearchPath { get; set; }

        /// <summary>
        /// A compatibility mode for special PostgreSQL server types.
        /// </summary>
        public ServerCompatibilityMode? ServerCompatibilityMode { get; set; }

        /// <summary>
        /// Determines the size of socket read buffer.
        /// </summary>
        public int? SocketReceiveBufferSize { get; set; }

        /// <summary>
        /// Determines the size of socket send buffer.
        /// </summary>
        public int? SocketSendBufferSize { get; set; }

        /// <summary>
        /// Controls whether SSL is required, disabled or preferred, depending on server support.
        /// </summary>
        public SslMode? SslMode { get; set; }

        /// <summary>
        /// Whether to use TCP keepalive with system defaults if overrides isn't specified.
        /// </summary>
        public bool? TcpKeepAlive { get; set; }

        /// <summary>
        /// The interval, in milliseconds, between when successive keep-alive packets are sent if no acknowledgement is received. Defaults to the value of TcpKeepAliveTime. TcpKeepAliveTime must be non-zero as well. Supported only on Windows.
        /// </summary>
        public int? TcpKeepAliveInterval { get; set; }

        /// <summary>
        /// The number of seconds of connection inactivity before a TCP keepalive query is sent. Use of this option is discouraged, use KeepAlive instead if possible. Set to 0 (the default) to disable. Supported only on Windows.
        /// </summary>
        public int? TcpKeepAliveTime { get; set; }

        /// <summary>
        /// The time to wait (in seconds) while trying to establish a connection before terminating the attempt and generating an error. Defaults to 15 seconds.
        /// </summary>
        public int? Timeout { get; set; }

        /// <summary>
        /// Gets or sets the session timezone, PGTZ environment variable can be used instead.
        /// </summary>
        public string Timezone { get; set; }

        /// <summary>
        /// Whether to trust the server certificate without validating it.
        /// </summary>
        public bool? TrustServerCertificate { get; set; }

        /// <summary>
        /// Writes connection performance information to performance counters.
        /// </summary>
        public bool? UsePerfCounters { get; set; }

        /// <summary>
        /// Npgsql uses its own internal implementation of TLS/SSL. Turn this on to use .NET SslStream instead.
        /// </summary>
        public bool? UseSslStream { get; set; }

        /// <summary>
        /// Determines the size of the internal buffer Npgsql uses when writing. Increasing may improve performance if transferring large values to the database.
        /// </summary>
        public int? WriteBufferSize { get; set; }
    }
}

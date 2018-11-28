// © John Hicks. All rights reserved. Licensed under the MIT license.
// See the LICENSE file in the repository root for more information.

using System.Collections.Generic;
using Npgsql;
using System.ComponentModel;

namespace ArgentSea.Pg
{
    /// <summary>
    /// This class represents is a (non-sharded) database connection.
    /// Note that the SecurityKey must match a defined key in the DataSecurityOptions; likewise, a ResilienceKey (if defined) must match as key in the DataResilienceOptions array.
    /// If the ResilienceKey is not defined, a default data resilience strategy will be used.
    /// </summary>
    public class PgConnectionConfiguration : PgConnectionPropertiesBase, IDataConnection
    {

        private readonly NpgsqlConnectionStringBuilder _csb = new NpgsqlConnectionStringBuilder();
        private string _connectionString = null;
        private PgConnectionPropertiesBase _globalProperties = null;
        private PgConnectionPropertiesBase _shardSetProperties = null;
        private PgConnectionPropertiesBase _shardProperties = null;

        private const int DefaultConnectTimeout = 2;

        private void HandlePropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            _connectionString = null;
        }

        private void SetProperties(DataConnectionConfigurationBase properties)
        {
            if (!(properties.Password is null))
            {
                _csb.Password = properties.Password;
            }
            if (!(properties.UserName is null))
            {
                _csb.Username = properties.UserName;
            }
            if (!(properties.WindowsAuth is null))
            {
                _csb.IntegratedSecurity = properties.WindowsAuth.Value;
            }
            var props = (PgConnectionPropertiesBase)properties;
            if (!(props.ApplicationName is null))
            {
                _csb.ApplicationName = props.ApplicationName;
            }
            if (!(props.AutoPrepareMinUsages is null))
            {
                _csb.AutoPrepareMinUsages = props.AutoPrepareMinUsages.Value;
            }
            if (!(props.CheckCertificateRevocation is null))
            {
                _csb.CheckCertificateRevocation = props.CheckCertificateRevocation.Value;
            }
            if (!(props.ClientEncoding is null))
            {
                _csb.ClientEncoding = props.ClientEncoding;
            }
            if (!(props.CommandTimeout is null))
            {
                _csb.CommandTimeout = props.CommandTimeout.Value;
            }
            if (!(props.ConnectionIdleLifetime is null))
            {
                _csb.CommandTimeout = props.ConnectionIdleLifetime.Value;
            }
            if (!(props.ConnectionPruningInterval is null))
            {
                _csb.ConnectionPruningInterval = props.ConnectionPruningInterval.Value;
            }
            if (!(props.ConvertInfinityDateTime is null))
            {
                _csb.ConvertInfinityDateTime = props.ConvertInfinityDateTime.Value;
            }
            if (!(props.Database is null))
            {
                _csb.Database = props.Database;
            }
            if (!(props.Encoding is null))
            {
                _csb.Encoding = props.Encoding;
            }
            if (!(props.Enlist is null))
            {
                _csb.Enlist = props.Enlist.Value;
            }
            if (!(props.Host is null))
            {
                _csb.Host = props.Host;
            }
            if (!(props.IncludeRealm is null))
            {
                _csb.IncludeRealm = props.IncludeRealm.Value;
            }
            if (!(props.InternalCommandTimeout is null))
            {
                _csb.InternalCommandTimeout = props.InternalCommandTimeout.Value;
            }
            if (!(props.KeepAlive is null))
            {
                _csb.KeepAlive = props.KeepAlive.Value;
            }
            if (!(props.KerberosServiceName is null))
            {
                _csb.KerberosServiceName = props.KerberosServiceName;
            }
            if (!(props.LoadTableComposites is null))
            {
                _csb.LoadTableComposites = props.LoadTableComposites.Value;
            }
            if (!(props.MaxAutoPrepare is null))
            {
                _csb.MaxAutoPrepare = props.MaxAutoPrepare.Value;
            }
            if (!(props.MaxPoolSize is null))
            {
                _csb.MaxPoolSize = props.MaxPoolSize.Value;
            }
            if (!(props.MinPoolSize is null))
            {
                _csb.MinPoolSize = props.MinPoolSize.Value;
            }
            if (!(props.NoResetOnClose is null))
            {
                _csb.NoResetOnClose = props.NoResetOnClose.Value;
            }
            if (!(props.PersistSecurityInfo is null))
            {
                _csb.PersistSecurityInfo = props.PersistSecurityInfo.Value;
            }
            if (!(props.Pooling is null))
            {
                _csb.Pooling = props.Pooling.Value;
            }
            if (!(props.Port is null))
            {
                _csb.Port = props.Port.Value;
            }
            if (!(props.ReadBufferSize is null))
            {
                _csb.ReadBufferSize = props.ReadBufferSize.Value;
            }
            if (!(props.SearchPath is null))
            {
                _csb.SearchPath = props.SearchPath;
            }
            if (!(props.ServerCompatibilityMode is null))
            {
                _csb.ServerCompatibilityMode = props.ServerCompatibilityMode.Value;
            }
            if (!(props.SocketReceiveBufferSize is null))
            {
                _csb.SocketReceiveBufferSize = props.SocketReceiveBufferSize.Value;
            }
            if (!(props.SocketSendBufferSize is null))
            {
                _csb.SocketSendBufferSize = props.SocketSendBufferSize.Value;
            }
            if (!(props.SslMode is null))
            {
                _csb.SslMode = props.SslMode.Value;
            }
            if (!(props.TcpKeepAlive is null))
            {
                _csb.TcpKeepAlive = props.TcpKeepAlive.Value;
            }
            if (!(props.Timeout is null))
            {
                _csb.Timeout = props.Timeout.Value;
            }
            if (!(props.Timezone is null))
            {
                _csb.Timezone = props.Timezone;
            }
            if (!(props.TrustServerCertificate is null))
            {
                _csb.TrustServerCertificate = props.TrustServerCertificate.Value;
            }
            if (!(props.UsePerfCounters is null))
            {
                _csb.UsePerfCounters = props.UsePerfCounters.Value;
            }
            if (!(props.UseSslStream is null))
            {
                _csb.UseSslStream = props.UseSslStream.Value;
            }
            if (!(props.WriteBufferSize is null))
            {
                _csb.WriteBufferSize = props.WriteBufferSize.Value;
            }
        }

        public string GetConnectionString()
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                if (!(_globalProperties is null))
                {
                    SetProperties(_globalProperties);
                }
                if (!(_shardSetProperties is null))
                {
                    SetProperties(_shardSetProperties);
                }
                if (!(_shardProperties is null))
                {
                    SetProperties(_shardProperties);
                }
                SetProperties(this);
                _connectionString = _csb.ToString();
            }
            return _connectionString;
        }

        public void SetAmbientConfiguration(DataConnectionConfigurationBase globalProperties, DataConnectionConfigurationBase shardSetProperties, DataConnectionConfigurationBase shardProperties)
        {
            _globalProperties = globalProperties as PgConnectionPropertiesBase;
            _shardSetProperties = shardSetProperties as PgConnectionPropertiesBase;
            _shardProperties = shardProperties as PgConnectionPropertiesBase;

            if (!(_globalProperties is null))
            {
                _globalProperties.PropertyChanged += HandlePropertyChanged;
            }
            if (!(_shardSetProperties is null))
            {
                _shardSetProperties.PropertyChanged += HandlePropertyChanged;
            }
            if (!(_shardProperties is null))
            {
                _shardProperties.PropertyChanged += HandlePropertyChanged;
            }
        }


        public string ConnectionDescription
		{
			get => $"database {this._csb.Database} on host {this._csb.Host}, port {_csb.Port}";
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

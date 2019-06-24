// © John Hicks. All rights reserved. Licensed under the MIT license.
// See the LICENSE file in the repository root for more information.

using System.Collections.Generic;
using Npgsql;
using System.ComponentModel;
using Microsoft.Extensions.Logging;

namespace ArgentSea.Pg
{
    /// <summary>
    /// This class represents is a (non-sharded) database connection.
    /// Note that the SecurityKey must match a defined key in the DataSecurityOptions; likewise, a ResilienceKey (if defined) must match as key in the DataResilienceOptions array.
    /// If the ResilienceKey is not defined, a default data resilience strategy will be used.
    /// </summary>
    public class PgConnectionConfiguration : PgConnectionPropertiesBase, IDataConnection
    {

        private string _connectionString = null;
        private PgConnectionPropertiesBase _globalProperties = null;
        private PgConnectionPropertiesBase _shardSetProperties = null;
        private PgConnectionPropertiesBase _readWriteProperties = null;
        private PgConnectionPropertiesBase _shardProperties = null;
        private string _connectionDescription = null;

        private const int DefaultConnectTimeout = 2;

        public PgConnectionConfiguration()
        {
        }

        private void HandlePropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            _connectionString = null;
        }

        private void SetProperties(NpgsqlConnectionStringBuilder csb, DataConnectionConfigurationBase properties)
        {
            if (!(properties.Password is null))
            {
                csb.Password = properties.Password;
            }
            if (!(properties.UserName is null))
            {
                csb.Username = properties.UserName;
            }
            if (!(properties.WindowsAuth is null))
            {
                csb.IntegratedSecurity = properties.WindowsAuth.Value;
            }
            var props = (PgConnectionPropertiesBase)properties;
            if (!(props.ApplicationName is null))
            {
                csb.ApplicationName = props.ApplicationName;
            }
            if (!(props.AutoPrepareMinUsages is null))
            {
                csb.AutoPrepareMinUsages = props.AutoPrepareMinUsages.Value;
            }
            if (!(props.CheckCertificateRevocation is null))
            {
                csb.CheckCertificateRevocation = props.CheckCertificateRevocation.Value;
            }
            if (!(props.ClientEncoding is null))
            {
                csb.ClientEncoding = props.ClientEncoding;
            }
            if (!(props.CommandTimeout is null))
            {
                csb.CommandTimeout = props.CommandTimeout.Value;
            }
            if (!(props.ConnectionIdleLifetime is null))
            {
                csb.CommandTimeout = props.ConnectionIdleLifetime.Value;
            }
            if (!(props.ConnectionPruningInterval is null))
            {
                csb.ConnectionPruningInterval = props.ConnectionPruningInterval.Value;
            }
            if (!(props.ConvertInfinityDateTime is null))
            {
                csb.ConvertInfinityDateTime = props.ConvertInfinityDateTime.Value;
            }
            if (!(props.Database is null))
            {
                csb.Database = props.Database;
            }
            if (!(props.Encoding is null))
            {
                csb.Encoding = props.Encoding;
            }
            if (!(props.Enlist is null))
            {
                csb.Enlist = props.Enlist.Value;
            }
            if (!(props.Host is null))
            {
                csb.Host = props.Host;
            }
            if (!(props.IncludeRealm is null))
            {
                csb.IncludeRealm = props.IncludeRealm.Value;
            }
            if (!(props.InternalCommandTimeout is null))
            {
                csb.InternalCommandTimeout = props.InternalCommandTimeout.Value;
            }
            if (!(props.KeepAlive is null))
            {
                csb.KeepAlive = props.KeepAlive.Value;
            }
            if (!(props.KerberosServiceName is null))
            {
                csb.KerberosServiceName = props.KerberosServiceName;
            }
            if (!(props.LoadTableComposites is null))
            {
                csb.LoadTableComposites = props.LoadTableComposites.Value;
            }
            if (!(props.MaxAutoPrepare is null))
            {
                csb.MaxAutoPrepare = props.MaxAutoPrepare.Value;
            }
            if (!(props.MaxPoolSize is null))
            {
                csb.MaxPoolSize = props.MaxPoolSize.Value;
            }
            if (!(props.MinPoolSize is null))
            {
                csb.MinPoolSize = props.MinPoolSize.Value;
            }
            if (!(props.NoResetOnClose is null))
            {
                csb.NoResetOnClose = props.NoResetOnClose.Value;
            }
            if (!(props.PersistSecurityInfo is null))
            {
                csb.PersistSecurityInfo = props.PersistSecurityInfo.Value;
            }
            if (!(props.Pooling is null))
            {
                csb.Pooling = props.Pooling.Value;
            }
            if (!(props.Port is null))
            {
                csb.Port = props.Port.Value;
            }
            if (!(props.ReadBufferSize is null))
            {
                csb.ReadBufferSize = props.ReadBufferSize.Value;
            }
            if (!(props.SearchPath is null))
            {
                csb.SearchPath = props.SearchPath;
            }
            if (!(props.ServerCompatibilityMode is null))
            {
                csb.ServerCompatibilityMode = props.ServerCompatibilityMode.Value;
            }
            if (!(props.SocketReceiveBufferSize is null))
            {
                csb.SocketReceiveBufferSize = props.SocketReceiveBufferSize.Value;
            }
            if (!(props.SocketSendBufferSize is null))
            {
                csb.SocketSendBufferSize = props.SocketSendBufferSize.Value;
            }
            if (!(props.SslMode is null))
            {
                csb.SslMode = props.SslMode.Value;
            }
            if (!(props.TcpKeepAlive is null))
            {
                csb.TcpKeepAlive = props.TcpKeepAlive.Value;
            }
            if (!(props.Timeout is null))
            {
                csb.Timeout = props.Timeout.Value;
            }
            if (!(props.Timezone is null))
            {
                csb.Timezone = props.Timezone;
            }
            if (!(props.TrustServerCertificate is null))
            {
                csb.TrustServerCertificate = props.TrustServerCertificate.Value;
            }
            if (!(props.UsePerfCounters is null))
            {
                csb.UsePerfCounters = props.UsePerfCounters.Value;
            }
            if (!(props.UseSslStream is null))
            {
                csb.UseSslStream = props.UseSslStream.Value;
            }
            if (!(props.WriteBufferSize is null))
            {
                csb.WriteBufferSize = props.WriteBufferSize.Value;
            }
        }

        public string GetConnectionString(ILogger logger)
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                var csb = new NpgsqlConnectionStringBuilder();
                if (!(_globalProperties is null))
                {
                    SetProperties(csb, _globalProperties);
                }
                if (!(_shardSetProperties is null))
                {
                    SetProperties(csb, _shardSetProperties);
                }
                if (!(_readWriteProperties is null))
                {
                    SetProperties(csb, _readWriteProperties);
                }
                if (!(_shardProperties is null))
                {
                    SetProperties(csb, _shardProperties);
                }
                SetProperties(csb, this);
                _connectionString = csb.ToString();
                _connectionDescription = $"database {csb.Database} on server {csb.Host}";
                var logCS = _connectionString;
                var pwd = csb.Password;
                if (!string.IsNullOrEmpty(pwd))
                {
                    logCS = logCS.Replace(pwd, "********");
                }
                logger?.SqlConnectionStringBuilt(logCS);
            }
            return _connectionString;
        }

        public void SetAmbientConfiguration(DataConnectionConfigurationBase globalProperties, DataConnectionConfigurationBase shardSetProperties, DataConnectionConfigurationBase readWriteProperties, DataConnectionConfigurationBase shardProperties)
        {
            _globalProperties = globalProperties as PgConnectionPropertiesBase;
            _shardSetProperties = shardSetProperties as PgConnectionPropertiesBase;
            _readWriteProperties = readWriteProperties as PgConnectionPropertiesBase;
            _shardProperties = shardProperties as PgConnectionPropertiesBase;

            if (!(_globalProperties is null))
            {
                _globalProperties.PropertyChanged += HandlePropertyChanged;
            }
            if (!(_shardSetProperties is null))
            {
                _shardSetProperties.PropertyChanged += HandlePropertyChanged;
            }
            if (!(_readWriteProperties is null))
            {
                _readWriteProperties.PropertyChanged += HandlePropertyChanged;
            }
            if (!(_shardProperties is null))
            {
                _shardProperties.PropertyChanged += HandlePropertyChanged;
            }
        }


        public string ConnectionDescription
        {
            get
            {
                if (string.IsNullOrEmpty(this._connectionDescription))
                {
                    string database = _globalProperties?.Database;
                    string host = _globalProperties?.Host;
                    if (!string.IsNullOrEmpty(_shardSetProperties?.Database))
                    {
                        database = _shardSetProperties.Database;
                    }
                    if (!string.IsNullOrEmpty(_shardSetProperties?.Host))
                    {
                        host = _shardSetProperties.Host;
                    }

                    if (!string.IsNullOrEmpty(_readWriteProperties?.Database))
                    {
                        database = _readWriteProperties.Database;
                    }
                    if (!string.IsNullOrEmpty(_readWriteProperties?.Host))
                    {
                        host = _readWriteProperties.Host;
                    }

                    if (!string.IsNullOrEmpty(_shardProperties?.Database))
                    {
                        database = _shardProperties.Database;
                    }
                    if (!string.IsNullOrEmpty(_shardProperties?.Host))
                    {
                        host = _shardProperties.Host;
                    }

                    if (!string.IsNullOrEmpty(this.Database))
                    {
                        database = this.Database;
                    }
                    if (!string.IsNullOrEmpty(this.Host))
                    {
                        host = this.Host;
                    }
                    _connectionDescription = $"database {database} on server {host}";
                }
                return this._connectionDescription;
            }
        }
    }
}

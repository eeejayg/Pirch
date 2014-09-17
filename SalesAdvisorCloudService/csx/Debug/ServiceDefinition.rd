<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="SalesAdvisorCloudService" generation="1" functional="0" release="0" Id="ffabf8e8-a8ed-4e1b-b53f-da995d86bedc" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="SalesAdvisorCloudServiceGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="BackendWebRole:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/LB:BackendWebRole:Endpoint1" />
          </inToChannel>
        </inPort>
        <inPort name="BackendWebRole:Https" protocol="https">
          <inToChannel>
            <lBChannelMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/LB:BackendWebRole:Https" />
          </inToChannel>
        </inPort>
        <inPort name="SalesAdvisorWebRole:HttpIn" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/LB:SalesAdvisorWebRole:HttpIn" />
          </inToChannel>
        </inPort>
        <inPort name="SalesAdvisorWebRole:HttpsIn" protocol="https">
          <inToChannel>
            <lBChannelMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/LB:SalesAdvisorWebRole:HttpsIn" />
          </inToChannel>
        </inPort>
        <inPort name="SalesAdvisorWebRole:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" protocol="tcp">
          <inToChannel>
            <lBChannelMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/LB:SalesAdvisorWebRole:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="BackendWebRole:DevProtectedQueueConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapBackendWebRole:DevProtectedQueueConnectionString" />
          </maps>
        </aCS>
        <aCS name="BackendWebRole:DevProtectedQueueName" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapBackendWebRole:DevProtectedQueueName" />
          </maps>
        </aCS>
        <aCS name="BackendWebRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapBackendWebRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="BackendWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapBackendWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </maps>
        </aCS>
        <aCS name="BackendWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapBackendWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </maps>
        </aCS>
        <aCS name="BackendWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapBackendWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </maps>
        </aCS>
        <aCS name="BackendWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapBackendWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </maps>
        </aCS>
        <aCS name="BackendWebRole:ProtectedQueueConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapBackendWebRole:ProtectedQueueConnectionString" />
          </maps>
        </aCS>
        <aCS name="BackendWebRole:ProtectedQueueName" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapBackendWebRole:ProtectedQueueName" />
          </maps>
        </aCS>
        <aCS name="BackendWebRoleInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapBackendWebRoleInstances" />
          </maps>
        </aCS>
        <aCS name="Certificate|BackendWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapCertificate|BackendWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </maps>
        </aCS>
        <aCS name="Certificate|SalesAdvisorWebRole:ArtefactSelfSigned" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapCertificate|SalesAdvisorWebRole:ArtefactSelfSigned" />
          </maps>
        </aCS>
        <aCS name="Certificate|SalesAdvisorWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapCertificate|SalesAdvisorWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </maps>
        </aCS>
        <aCS name="Certificate|SalesAdvisorWorkerRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapCertificate|SalesAdvisorWorkerRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </maps>
        </aCS>
        <aCS name="SalesAdvisorWebRole:CDNDomain" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapSalesAdvisorWebRole:CDNDomain" />
          </maps>
        </aCS>
        <aCS name="SalesAdvisorWebRole:Environment" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapSalesAdvisorWebRole:Environment" />
          </maps>
        </aCS>
        <aCS name="SalesAdvisorWebRole:FLEndpointServer" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapSalesAdvisorWebRole:FLEndpointServer" />
          </maps>
        </aCS>
        <aCS name="SalesAdvisorWebRole:MessageQueueConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapSalesAdvisorWebRole:MessageQueueConnectionString" />
          </maps>
        </aCS>
        <aCS name="SalesAdvisorWebRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapSalesAdvisorWebRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="SalesAdvisorWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapSalesAdvisorWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </maps>
        </aCS>
        <aCS name="SalesAdvisorWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapSalesAdvisorWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </maps>
        </aCS>
        <aCS name="SalesAdvisorWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapSalesAdvisorWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </maps>
        </aCS>
        <aCS name="SalesAdvisorWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapSalesAdvisorWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </maps>
        </aCS>
        <aCS name="SalesAdvisorWebRole:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapSalesAdvisorWebRole:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" />
          </maps>
        </aCS>
        <aCS name="SalesAdvisorWebRole:SADBConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapSalesAdvisorWebRole:SADBConnectionString" />
          </maps>
        </aCS>
        <aCS name="SalesAdvisorWebRoleInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapSalesAdvisorWebRoleInstances" />
          </maps>
        </aCS>
        <aCS name="SalesAdvisorWorkerRole:DevProtectedQueueConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapSalesAdvisorWorkerRole:DevProtectedQueueConnectionString" />
          </maps>
        </aCS>
        <aCS name="SalesAdvisorWorkerRole:DevProtectedQueueName" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapSalesAdvisorWorkerRole:DevProtectedQueueName" />
          </maps>
        </aCS>
        <aCS name="SalesAdvisorWorkerRole:DevQueueConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapSalesAdvisorWorkerRole:DevQueueConnectionString" />
          </maps>
        </aCS>
        <aCS name="SalesAdvisorWorkerRole:DevQueueName" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapSalesAdvisorWorkerRole:DevQueueName" />
          </maps>
        </aCS>
        <aCS name="SalesAdvisorWorkerRole:FLEndpointServer" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapSalesAdvisorWorkerRole:FLEndpointServer" />
          </maps>
        </aCS>
        <aCS name="SalesAdvisorWorkerRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapSalesAdvisorWorkerRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="SalesAdvisorWorkerRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapSalesAdvisorWorkerRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </maps>
        </aCS>
        <aCS name="SalesAdvisorWorkerRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapSalesAdvisorWorkerRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </maps>
        </aCS>
        <aCS name="SalesAdvisorWorkerRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapSalesAdvisorWorkerRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </maps>
        </aCS>
        <aCS name="SalesAdvisorWorkerRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapSalesAdvisorWorkerRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </maps>
        </aCS>
        <aCS name="SalesAdvisorWorkerRole:PrivateQueueConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapSalesAdvisorWorkerRole:PrivateQueueConnectionString" />
          </maps>
        </aCS>
        <aCS name="SalesAdvisorWorkerRole:PrivateQueueName" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapSalesAdvisorWorkerRole:PrivateQueueName" />
          </maps>
        </aCS>
        <aCS name="SalesAdvisorWorkerRole:ProductDBConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapSalesAdvisorWorkerRole:ProductDBConnectionString" />
          </maps>
        </aCS>
        <aCS name="SalesAdvisorWorkerRole:ProtectedQueueConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapSalesAdvisorWorkerRole:ProtectedQueueConnectionString" />
          </maps>
        </aCS>
        <aCS name="SalesAdvisorWorkerRole:ProtectedQueueName" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapSalesAdvisorWorkerRole:ProtectedQueueName" />
          </maps>
        </aCS>
        <aCS name="SalesAdvisorWorkerRole:SADBConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapSalesAdvisorWorkerRole:SADBConnectionString" />
          </maps>
        </aCS>
        <aCS name="SalesAdvisorWorkerRoleInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/MapSalesAdvisorWorkerRoleInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:BackendWebRole:Endpoint1">
          <toPorts>
            <inPortMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/BackendWebRole/Endpoint1" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:BackendWebRole:Https">
          <toPorts>
            <inPortMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/BackendWebRole/Https" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:SalesAdvisorWebRole:HttpIn">
          <toPorts>
            <inPortMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWebRole/HttpIn" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:SalesAdvisorWebRole:HttpsIn">
          <toPorts>
            <inPortMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWebRole/HttpsIn" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:SalesAdvisorWebRole:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput">
          <toPorts>
            <inPortMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWebRole/Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </toPorts>
        </lBChannel>
        <sFSwitchChannel name="SW:BackendWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp">
          <toPorts>
            <inPortMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/BackendWebRole/Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
          </toPorts>
        </sFSwitchChannel>
        <sFSwitchChannel name="SW:SalesAdvisorWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp">
          <toPorts>
            <inPortMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWebRole/Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
          </toPorts>
        </sFSwitchChannel>
        <sFSwitchChannel name="SW:SalesAdvisorWorkerRole:GetDataEndpoint">
          <toPorts>
            <inPortMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWorkerRole/GetDataEndpoint" />
          </toPorts>
        </sFSwitchChannel>
        <sFSwitchChannel name="SW:SalesAdvisorWorkerRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp">
          <toPorts>
            <inPortMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWorkerRole/Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
          </toPorts>
        </sFSwitchChannel>
      </channels>
      <maps>
        <map name="MapBackendWebRole:DevProtectedQueueConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/BackendWebRole/DevProtectedQueueConnectionString" />
          </setting>
        </map>
        <map name="MapBackendWebRole:DevProtectedQueueName" kind="Identity">
          <setting>
            <aCSMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/BackendWebRole/DevProtectedQueueName" />
          </setting>
        </map>
        <map name="MapBackendWebRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/BackendWebRole/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapBackendWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" kind="Identity">
          <setting>
            <aCSMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/BackendWebRole/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </setting>
        </map>
        <map name="MapBackendWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" kind="Identity">
          <setting>
            <aCSMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/BackendWebRole/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </setting>
        </map>
        <map name="MapBackendWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" kind="Identity">
          <setting>
            <aCSMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/BackendWebRole/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </setting>
        </map>
        <map name="MapBackendWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/BackendWebRole/Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </setting>
        </map>
        <map name="MapBackendWebRole:ProtectedQueueConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/BackendWebRole/ProtectedQueueConnectionString" />
          </setting>
        </map>
        <map name="MapBackendWebRole:ProtectedQueueName" kind="Identity">
          <setting>
            <aCSMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/BackendWebRole/ProtectedQueueName" />
          </setting>
        </map>
        <map name="MapBackendWebRoleInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/BackendWebRoleInstances" />
          </setting>
        </map>
        <map name="MapCertificate|BackendWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" kind="Identity">
          <certificate>
            <certificateMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/BackendWebRole/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </certificate>
        </map>
        <map name="MapCertificate|SalesAdvisorWebRole:ArtefactSelfSigned" kind="Identity">
          <certificate>
            <certificateMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWebRole/ArtefactSelfSigned" />
          </certificate>
        </map>
        <map name="MapCertificate|SalesAdvisorWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" kind="Identity">
          <certificate>
            <certificateMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWebRole/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </certificate>
        </map>
        <map name="MapCertificate|SalesAdvisorWorkerRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" kind="Identity">
          <certificate>
            <certificateMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWorkerRole/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </certificate>
        </map>
        <map name="MapSalesAdvisorWebRole:CDNDomain" kind="Identity">
          <setting>
            <aCSMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWebRole/CDNDomain" />
          </setting>
        </map>
        <map name="MapSalesAdvisorWebRole:Environment" kind="Identity">
          <setting>
            <aCSMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWebRole/Environment" />
          </setting>
        </map>
        <map name="MapSalesAdvisorWebRole:FLEndpointServer" kind="Identity">
          <setting>
            <aCSMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWebRole/FLEndpointServer" />
          </setting>
        </map>
        <map name="MapSalesAdvisorWebRole:MessageQueueConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWebRole/MessageQueueConnectionString" />
          </setting>
        </map>
        <map name="MapSalesAdvisorWebRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWebRole/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapSalesAdvisorWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" kind="Identity">
          <setting>
            <aCSMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWebRole/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </setting>
        </map>
        <map name="MapSalesAdvisorWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" kind="Identity">
          <setting>
            <aCSMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWebRole/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </setting>
        </map>
        <map name="MapSalesAdvisorWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" kind="Identity">
          <setting>
            <aCSMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWebRole/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </setting>
        </map>
        <map name="MapSalesAdvisorWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWebRole/Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </setting>
        </map>
        <map name="MapSalesAdvisorWebRole:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWebRole/Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" />
          </setting>
        </map>
        <map name="MapSalesAdvisorWebRole:SADBConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWebRole/SADBConnectionString" />
          </setting>
        </map>
        <map name="MapSalesAdvisorWebRoleInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWebRoleInstances" />
          </setting>
        </map>
        <map name="MapSalesAdvisorWorkerRole:DevProtectedQueueConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWorkerRole/DevProtectedQueueConnectionString" />
          </setting>
        </map>
        <map name="MapSalesAdvisorWorkerRole:DevProtectedQueueName" kind="Identity">
          <setting>
            <aCSMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWorkerRole/DevProtectedQueueName" />
          </setting>
        </map>
        <map name="MapSalesAdvisorWorkerRole:DevQueueConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWorkerRole/DevQueueConnectionString" />
          </setting>
        </map>
        <map name="MapSalesAdvisorWorkerRole:DevQueueName" kind="Identity">
          <setting>
            <aCSMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWorkerRole/DevQueueName" />
          </setting>
        </map>
        <map name="MapSalesAdvisorWorkerRole:FLEndpointServer" kind="Identity">
          <setting>
            <aCSMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWorkerRole/FLEndpointServer" />
          </setting>
        </map>
        <map name="MapSalesAdvisorWorkerRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWorkerRole/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapSalesAdvisorWorkerRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" kind="Identity">
          <setting>
            <aCSMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWorkerRole/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </setting>
        </map>
        <map name="MapSalesAdvisorWorkerRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" kind="Identity">
          <setting>
            <aCSMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWorkerRole/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </setting>
        </map>
        <map name="MapSalesAdvisorWorkerRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" kind="Identity">
          <setting>
            <aCSMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWorkerRole/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </setting>
        </map>
        <map name="MapSalesAdvisorWorkerRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWorkerRole/Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </setting>
        </map>
        <map name="MapSalesAdvisorWorkerRole:PrivateQueueConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWorkerRole/PrivateQueueConnectionString" />
          </setting>
        </map>
        <map name="MapSalesAdvisorWorkerRole:PrivateQueueName" kind="Identity">
          <setting>
            <aCSMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWorkerRole/PrivateQueueName" />
          </setting>
        </map>
        <map name="MapSalesAdvisorWorkerRole:ProductDBConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWorkerRole/ProductDBConnectionString" />
          </setting>
        </map>
        <map name="MapSalesAdvisorWorkerRole:ProtectedQueueConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWorkerRole/ProtectedQueueConnectionString" />
          </setting>
        </map>
        <map name="MapSalesAdvisorWorkerRole:ProtectedQueueName" kind="Identity">
          <setting>
            <aCSMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWorkerRole/ProtectedQueueName" />
          </setting>
        </map>
        <map name="MapSalesAdvisorWorkerRole:SADBConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWorkerRole/SADBConnectionString" />
          </setting>
        </map>
        <map name="MapSalesAdvisorWorkerRoleInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWorkerRoleInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="BackendWebRole" generation="1" functional="0" release="0" software="C:\All\pirch\SalesAdvisorCloudService\csx\Debug\roles\BackendWebRole" entryPoint="base\x86\WaHostBootstrapper.exe" parameters="base\x86\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="8080" />
              <inPort name="Https" protocol="https" portRanges="8081">
                <certificate>
                  <certificateMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/BackendWebRole/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
                </certificate>
              </inPort>
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp" portRanges="3389" />
              <outPort name="BackendWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SW:BackendWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
                </outToChannel>
              </outPort>
              <outPort name="SalesAdvisorWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SW:SalesAdvisorWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
                </outToChannel>
              </outPort>
              <outPort name="SalesAdvisorWorkerRole:GetDataEndpoint" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SW:SalesAdvisorWorkerRole:GetDataEndpoint" />
                </outToChannel>
              </outPort>
              <outPort name="SalesAdvisorWorkerRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SW:SalesAdvisorWorkerRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
                </outToChannel>
              </outPort>
            </componentports>
            <settings>
              <aCS name="DevProtectedQueueConnectionString" defaultValue="" />
              <aCS name="DevProtectedQueueName" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="" />
              <aCS name="ProtectedQueueConnectionString" defaultValue="" />
              <aCS name="ProtectedQueueName" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;BackendWebRole&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;BackendWebRole&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;e name=&quot;Https&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;SalesAdvisorWebRole&quot;&gt;&lt;e name=&quot;HttpIn&quot; /&gt;&lt;e name=&quot;HttpsIn&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;SalesAdvisorWorkerRole&quot;&gt;&lt;e name=&quot;GetDataEndpoint&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
            <storedcertificates>
              <storedCertificate name="Stored0Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" certificateStore="My" certificateLocation="System">
                <certificate>
                  <certificateMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/BackendWebRole/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
                </certificate>
              </storedCertificate>
            </storedcertificates>
            <certificates>
              <certificate name="Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
            </certificates>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/BackendWebRoleInstances" />
            <sCSPolicyUpdateDomainMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/BackendWebRoleUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/BackendWebRoleFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="SalesAdvisorWebRole" generation="1" functional="0" release="0" software="C:\All\pirch\SalesAdvisorCloudService\csx\Debug\roles\SalesAdvisorWebRole" entryPoint="base\x86\WaHostBootstrapper.exe" parameters="base\x86\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="HttpIn" protocol="http" portRanges="80" />
              <inPort name="HttpsIn" protocol="https" portRanges="443">
                <certificate>
                  <certificateMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWebRole/ArtefactSelfSigned" />
                </certificate>
              </inPort>
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" protocol="tcp" />
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp" portRanges="3389" />
              <outPort name="BackendWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SW:BackendWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
                </outToChannel>
              </outPort>
              <outPort name="SalesAdvisorWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SW:SalesAdvisorWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
                </outToChannel>
              </outPort>
              <outPort name="SalesAdvisorWorkerRole:GetDataEndpoint" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SW:SalesAdvisorWorkerRole:GetDataEndpoint" />
                </outToChannel>
              </outPort>
              <outPort name="SalesAdvisorWorkerRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SW:SalesAdvisorWorkerRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
                </outToChannel>
              </outPort>
            </componentports>
            <settings>
              <aCS name="CDNDomain" defaultValue="" />
              <aCS name="Environment" defaultValue="" />
              <aCS name="FLEndpointServer" defaultValue="" />
              <aCS name="MessageQueueConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" defaultValue="" />
              <aCS name="SADBConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;SalesAdvisorWebRole&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;BackendWebRole&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;e name=&quot;Https&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;SalesAdvisorWebRole&quot;&gt;&lt;e name=&quot;HttpIn&quot; /&gt;&lt;e name=&quot;HttpsIn&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;SalesAdvisorWorkerRole&quot;&gt;&lt;e name=&quot;GetDataEndpoint&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
            <storedcertificates>
              <storedCertificate name="Stored0ArtefactSelfSigned" certificateStore="CA" certificateLocation="System">
                <certificate>
                  <certificateMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWebRole/ArtefactSelfSigned" />
                </certificate>
              </storedCertificate>
              <storedCertificate name="Stored1Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" certificateStore="My" certificateLocation="System">
                <certificate>
                  <certificateMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWebRole/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
                </certificate>
              </storedCertificate>
            </storedcertificates>
            <certificates>
              <certificate name="ArtefactSelfSigned" />
              <certificate name="Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
            </certificates>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWebRoleInstances" />
            <sCSPolicyUpdateDomainMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWebRoleUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWebRoleFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="SalesAdvisorWorkerRole" generation="1" functional="0" release="0" software="C:\All\pirch\SalesAdvisorCloudService\csx\Debug\roles\SalesAdvisorWorkerRole" entryPoint="base\x86\WaHostBootstrapper.exe" parameters="base\x86\WaWorkerHost.exe " memIndex="1792" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="GetDataEndpoint" protocol="tcp" />
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp" portRanges="3389" />
              <outPort name="BackendWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SW:BackendWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
                </outToChannel>
              </outPort>
              <outPort name="SalesAdvisorWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SW:SalesAdvisorWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
                </outToChannel>
              </outPort>
              <outPort name="SalesAdvisorWorkerRole:GetDataEndpoint" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SW:SalesAdvisorWorkerRole:GetDataEndpoint" />
                </outToChannel>
              </outPort>
              <outPort name="SalesAdvisorWorkerRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SW:SalesAdvisorWorkerRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
                </outToChannel>
              </outPort>
            </componentports>
            <settings>
              <aCS name="DevProtectedQueueConnectionString" defaultValue="" />
              <aCS name="DevProtectedQueueName" defaultValue="" />
              <aCS name="DevQueueConnectionString" defaultValue="" />
              <aCS name="DevQueueName" defaultValue="" />
              <aCS name="FLEndpointServer" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="" />
              <aCS name="PrivateQueueConnectionString" defaultValue="" />
              <aCS name="PrivateQueueName" defaultValue="" />
              <aCS name="ProductDBConnectionString" defaultValue="" />
              <aCS name="ProtectedQueueConnectionString" defaultValue="" />
              <aCS name="ProtectedQueueName" defaultValue="" />
              <aCS name="SADBConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;SalesAdvisorWorkerRole&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;BackendWebRole&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;e name=&quot;Https&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;SalesAdvisorWebRole&quot;&gt;&lt;e name=&quot;HttpIn&quot; /&gt;&lt;e name=&quot;HttpsIn&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;SalesAdvisorWorkerRole&quot;&gt;&lt;e name=&quot;GetDataEndpoint&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
            <storedcertificates>
              <storedCertificate name="Stored0Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" certificateStore="My" certificateLocation="System">
                <certificate>
                  <certificateMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWorkerRole/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
                </certificate>
              </storedCertificate>
            </storedcertificates>
            <certificates>
              <certificate name="Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
            </certificates>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWorkerRoleInstances" />
            <sCSPolicyUpdateDomainMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWorkerRoleUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWorkerRoleFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="SalesAdvisorWebRoleUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyUpdateDomain name="BackendWebRoleUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyUpdateDomain name="SalesAdvisorWorkerRoleUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="BackendWebRoleFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyFaultDomain name="SalesAdvisorWebRoleFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyFaultDomain name="SalesAdvisorWorkerRoleFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="BackendWebRoleInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="SalesAdvisorWebRoleInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="SalesAdvisorWorkerRoleInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="227a05ba-acc0-475f-ae98-93e8c82a7577" ref="Microsoft.RedDog.Contract\ServiceContract\SalesAdvisorCloudServiceContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="20ca9e5a-69a0-41fd-a70f-50473bbaf615" ref="Microsoft.RedDog.Contract\Interface\BackendWebRole:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/BackendWebRole:Endpoint1" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="f86769f6-fff4-4569-be38-3dce5c212e19" ref="Microsoft.RedDog.Contract\Interface\BackendWebRole:Https@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/BackendWebRole:Https" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="f2e2b348-c8f7-4c8d-bbd0-a79341bdfe58" ref="Microsoft.RedDog.Contract\Interface\SalesAdvisorWebRole:HttpIn@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWebRole:HttpIn" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="c7689aab-44c1-44d3-915d-fe5db0b133b7" ref="Microsoft.RedDog.Contract\Interface\SalesAdvisorWebRole:HttpsIn@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWebRole:HttpsIn" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="e9a5d2d0-1f03-461f-9ae4-620a5cf104b6" ref="Microsoft.RedDog.Contract\Interface\SalesAdvisorWebRole:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/SalesAdvisorCloudService/SalesAdvisorCloudServiceGroup/SalesAdvisorWebRole:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>
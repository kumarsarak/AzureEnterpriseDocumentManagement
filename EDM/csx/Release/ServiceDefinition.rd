<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="EDM" generation="1" functional="0" release="0" Id="376a4af1-5d04-45f0-9145-feee99e3fe92" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="EDMGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="WebRole1:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/EDM/EDMGroup/LB:WebRole1:Endpoint1" />
          </inToChannel>
        </inPort>
        <inPort name="WebRole1:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" protocol="tcp">
          <inToChannel>
            <lBChannelMoniker name="/EDM/EDMGroup/LB:WebRole1:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="Certificate|WebRole1:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" defaultValue="">
          <maps>
            <mapMoniker name="/EDM/EDMGroup/MapCertificate|WebRole1:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </maps>
        </aCS>
        <aCS name="Certificate|WebRole1:Microsoft.WindowsAzure.Plugins.RemoteDebugger.TransportValidation" defaultValue="">
          <maps>
            <mapMoniker name="/EDM/EDMGroup/MapCertificate|WebRole1:Microsoft.WindowsAzure.Plugins.RemoteDebugger.TransportValidation" />
          </maps>
        </aCS>
        <aCS name="WebRole1:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/EDM/EDMGroup/MapWebRole1:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="WebRole1:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="">
          <maps>
            <mapMoniker name="/EDM/EDMGroup/MapWebRole1:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </maps>
        </aCS>
        <aCS name="WebRole1:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="">
          <maps>
            <mapMoniker name="/EDM/EDMGroup/MapWebRole1:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </maps>
        </aCS>
        <aCS name="WebRole1:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="">
          <maps>
            <mapMoniker name="/EDM/EDMGroup/MapWebRole1:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </maps>
        </aCS>
        <aCS name="WebRole1:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/EDM/EDMGroup/MapWebRole1:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </maps>
        </aCS>
        <aCS name="WebRole1:Microsoft.WindowsAzure.Plugins.RemoteDebugger.ClientThumbprint" defaultValue="">
          <maps>
            <mapMoniker name="/EDM/EDMGroup/MapWebRole1:Microsoft.WindowsAzure.Plugins.RemoteDebugger.ClientThumbprint" />
          </maps>
        </aCS>
        <aCS name="WebRole1:Microsoft.WindowsAzure.Plugins.RemoteDebugger.Connector.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/EDM/EDMGroup/MapWebRole1:Microsoft.WindowsAzure.Plugins.RemoteDebugger.Connector.Enabled" />
          </maps>
        </aCS>
        <aCS name="WebRole1:Microsoft.WindowsAzure.Plugins.RemoteDebugger.Connector.Version" defaultValue="">
          <maps>
            <mapMoniker name="/EDM/EDMGroup/MapWebRole1:Microsoft.WindowsAzure.Plugins.RemoteDebugger.Connector.Version" />
          </maps>
        </aCS>
        <aCS name="WebRole1:Microsoft.WindowsAzure.Plugins.RemoteDebugger.ServerThumbprint" defaultValue="">
          <maps>
            <mapMoniker name="/EDM/EDMGroup/MapWebRole1:Microsoft.WindowsAzure.Plugins.RemoteDebugger.ServerThumbprint" />
          </maps>
        </aCS>
        <aCS name="WebRole1:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/EDM/EDMGroup/MapWebRole1:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" />
          </maps>
        </aCS>
        <aCS name="WebRole1Instances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/EDM/EDMGroup/MapWebRole1Instances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <sFSwitchChannel name="IE:WebRole1:Microsoft.WindowsAzure.Plugins.RemoteDebugger.Connector">
          <toPorts>
            <inPortMoniker name="/EDM/EDMGroup/WebRole1/Microsoft.WindowsAzure.Plugins.RemoteDebugger.Connector" />
          </toPorts>
        </sFSwitchChannel>
        <sFSwitchChannel name="IE:WebRole1:Microsoft.WindowsAzure.Plugins.RemoteDebugger.Forwarder">
          <toPorts>
            <inPortMoniker name="/EDM/EDMGroup/WebRole1/Microsoft.WindowsAzure.Plugins.RemoteDebugger.Forwarder" />
          </toPorts>
        </sFSwitchChannel>
        <lBChannel name="LB:WebRole1:Endpoint1">
          <toPorts>
            <inPortMoniker name="/EDM/EDMGroup/WebRole1/Endpoint1" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:WebRole1:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput">
          <toPorts>
            <inPortMoniker name="/EDM/EDMGroup/WebRole1/Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </toPorts>
        </lBChannel>
        <sFSwitchChannel name="SW:WebRole1:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp">
          <toPorts>
            <inPortMoniker name="/EDM/EDMGroup/WebRole1/Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
          </toPorts>
        </sFSwitchChannel>
      </channels>
      <maps>
        <map name="MapCertificate|WebRole1:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" kind="Identity">
          <certificate>
            <certificateMoniker name="/EDM/EDMGroup/WebRole1/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </certificate>
        </map>
        <map name="MapCertificate|WebRole1:Microsoft.WindowsAzure.Plugins.RemoteDebugger.TransportValidation" kind="Identity">
          <certificate>
            <certificateMoniker name="/EDM/EDMGroup/WebRole1/Microsoft.WindowsAzure.Plugins.RemoteDebugger.TransportValidation" />
          </certificate>
        </map>
        <map name="MapWebRole1:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/EDM/EDMGroup/WebRole1/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapWebRole1:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" kind="Identity">
          <setting>
            <aCSMoniker name="/EDM/EDMGroup/WebRole1/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </setting>
        </map>
        <map name="MapWebRole1:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" kind="Identity">
          <setting>
            <aCSMoniker name="/EDM/EDMGroup/WebRole1/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </setting>
        </map>
        <map name="MapWebRole1:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" kind="Identity">
          <setting>
            <aCSMoniker name="/EDM/EDMGroup/WebRole1/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </setting>
        </map>
        <map name="MapWebRole1:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/EDM/EDMGroup/WebRole1/Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </setting>
        </map>
        <map name="MapWebRole1:Microsoft.WindowsAzure.Plugins.RemoteDebugger.ClientThumbprint" kind="Identity">
          <setting>
            <aCSMoniker name="/EDM/EDMGroup/WebRole1/Microsoft.WindowsAzure.Plugins.RemoteDebugger.ClientThumbprint" />
          </setting>
        </map>
        <map name="MapWebRole1:Microsoft.WindowsAzure.Plugins.RemoteDebugger.Connector.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/EDM/EDMGroup/WebRole1/Microsoft.WindowsAzure.Plugins.RemoteDebugger.Connector.Enabled" />
          </setting>
        </map>
        <map name="MapWebRole1:Microsoft.WindowsAzure.Plugins.RemoteDebugger.Connector.Version" kind="Identity">
          <setting>
            <aCSMoniker name="/EDM/EDMGroup/WebRole1/Microsoft.WindowsAzure.Plugins.RemoteDebugger.Connector.Version" />
          </setting>
        </map>
        <map name="MapWebRole1:Microsoft.WindowsAzure.Plugins.RemoteDebugger.ServerThumbprint" kind="Identity">
          <setting>
            <aCSMoniker name="/EDM/EDMGroup/WebRole1/Microsoft.WindowsAzure.Plugins.RemoteDebugger.ServerThumbprint" />
          </setting>
        </map>
        <map name="MapWebRole1:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/EDM/EDMGroup/WebRole1/Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" />
          </setting>
        </map>
        <map name="MapWebRole1Instances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/EDM/EDMGroup/WebRole1Instances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="WebRole1" generation="1" functional="0" release="0" software="C:\EDM\EDM\EDM\csx\Release\roles\WebRole1" entryPoint="base\x86\WaHostBootstrapper.exe" parameters="base\x86\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" protocol="tcp" />
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp" portRanges="3389" />
              <outPort name="WebRole1:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/EDM/EDMGroup/SW:WebRole1:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
                </outToChannel>
              </outPort>
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteDebugger.ClientThumbprint" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteDebugger.Connector.Enabled" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteDebugger.Connector.Version" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteDebugger.ServerThumbprint" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;WebRole1&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;WebRole1&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteDebugger.Connector&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteDebugger.Forwarder&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
            <storedcertificates>
              <storedCertificate name="Stored0Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" certificateStore="My" certificateLocation="System">
                <certificate>
                  <certificateMoniker name="/EDM/EDMGroup/WebRole1/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
                </certificate>
              </storedCertificate>
              <storedCertificate name="Stored1Microsoft.WindowsAzure.Plugins.RemoteDebugger.TransportValidation" certificateStore="My" certificateLocation="System">
                <certificate>
                  <certificateMoniker name="/EDM/EDMGroup/WebRole1/Microsoft.WindowsAzure.Plugins.RemoteDebugger.TransportValidation" />
                </certificate>
              </storedCertificate>
            </storedcertificates>
            <certificates>
              <certificate name="Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
              <certificate name="Microsoft.WindowsAzure.Plugins.RemoteDebugger.TransportValidation" />
            </certificates>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/EDM/EDMGroup/WebRole1Instances" />
            <sCSPolicyUpdateDomainMoniker name="/EDM/EDMGroup/WebRole1UpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/EDM/EDMGroup/WebRole1FaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="WebRole1UpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="WebRole1FaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="WebRole1Instances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="d3eeafc1-03af-4571-ae3a-394a9a4ed697" ref="Microsoft.RedDog.Contract\ServiceContract\EDMContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="da25b2f9-649c-4d74-98df-33fc365a8888" ref="Microsoft.RedDog.Contract\Interface\WebRole1:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/EDM/EDMGroup/WebRole1:Endpoint1" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="baf99814-d1dd-457d-be5d-cdb2b9f7dabd" ref="Microsoft.RedDog.Contract\Interface\WebRole1:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/EDM/EDMGroup/WebRole1:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>
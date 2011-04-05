using System;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Configuration;

namespace WcfService1
{
    /// <summary>
    /// WCF SOAP Inspector class
    /// by Kory Becker http://www.primaryobjects.com/CMS/Article121.aspx
    /// 
    /// SOAP XML worker method to view incoming and
    /// outgoing SOAP messages to the WCF service.
    /// </summary>
    public class ConsoleOutputMessageInspector : IDispatchMessageInspector
    {
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            // Make a copy of the SOAP packet for viewing.
            MessageBuffer buffer = request.CreateBufferedCopy(Int32.MaxValue);
            Message msgCopy = buffer.CreateMessage();

            request = buffer.CreateMessage();

            // Get the SOAP XML content.
            string strMessage = buffer.CreateMessage().ToString();

            // Get the SOAP XML body content.
            System.Xml.XmlDictionaryReader xrdr = msgCopy.GetReaderAtBodyContents();
            string bodyData = xrdr.ReadOuterXml();

            // Replace the body placeholder with the actual SOAP body.
            strMessage = strMessage.Replace("... stream ...", bodyData);

            // Display the SOAP XML.
            System.Diagnostics.Debug.WriteLine("Received:\n" + strMessage);

            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            // Make a copy of the SOAP packet for viewing.
            MessageBuffer buffer = reply.CreateBufferedCopy(Int32.MaxValue);
            reply = buffer.CreateMessage();

            // Display the SOAP XML.
            System.Diagnostics.Debug.WriteLine("Sending:\n" + buffer.CreateMessage().ToString());
        }
    }

    /// <summary>
    /// SOAP XML Inspector endpoint.
    /// </summary>
    public class ConsoleOutputBehavior : IEndpointBehavior
    {
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            throw new Exception("Behavior not supported on the consumer side!");
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            ConsoleOutputMessageInspector inspector = new ConsoleOutputMessageInspector();
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(inspector);
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }

    public class ConsoleOutputBehaviorExtensionElement : BehaviorExtensionElement
    {
        protected override object CreateBehavior()
        {
            return new ConsoleOutputBehavior();
        }

        public override Type BehaviorType
        {
            get
            {
                return typeof(ConsoleOutputBehavior);
            }
        }
    }

    /// <summary>
    /// SOAP XML Inspector attribute.
    /// Add the attribute to your WCF class definition, as follows:
    /// [ConsoleHeaderOutputBehavior]
    /// public class MyWCFService : IMyWCFService { ... }
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ConsoleHeaderOutputBehavior : Attribute, IServiceBehavior
    {
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            for (int i = 0; i < serviceHostBase.ChannelDispatchers.Count; i++)
            {
                ChannelDispatcher channelDispatcher = serviceHostBase.ChannelDispatchers[i] as ChannelDispatcher;
                if (channelDispatcher != null)
                {
                    foreach (EndpointDispatcher endpointDispatcher in channelDispatcher.Endpoints)
                    {
                        ConsoleOutputMessageInspector inspector = new ConsoleOutputMessageInspector();
                        endpointDispatcher.DispatchRuntime.MessageInspectors.Add(inspector);
                    }
                }
            }
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }
    }
}

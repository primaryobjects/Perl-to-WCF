using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService1
{
    [ConsoleHeaderOutputBehavior]
    public class Service1 : IService1
    {
        public string HelloWorld(string name)
        {
            return "Hello World, " + name;
        }

        public string HelloWorld2(NameData nameData)
        {
            return "Hello World, " + nameData.FirstName + " " + nameData.LastName;
        }
    }
}

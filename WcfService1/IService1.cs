using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService1
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        string HelloWorld(string name);

        [OperationContract]
        string HelloWorld2(NameData nameData);
    }

    [DataContract]
    public class NameData
    {
        [DataMember]
        public string FirstName;

        [DataMember]
        public string LastName;

        [DataMember]
        public int Age;

        public NameData(string firstName, string lastName, int age)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
        }
    }
}

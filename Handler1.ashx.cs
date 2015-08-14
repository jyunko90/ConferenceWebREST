using JsonServices;
using JsonServices.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConferenceRESTSystem
{
    public class Handler1 : JsonHandler
    {
        public Handler1()
        {
            this.service.Name = "JSONWebAPI";
            this.service.Description = "JSON API for android appliation";
            InterfaceConfiguration IConfig = new InterfaceConfiguration("RestAPI", typeof(IServiceAPI), typeof(ServiceAPI));
            this.service.Interfaces.Add(IConfig);
        }
    }
}
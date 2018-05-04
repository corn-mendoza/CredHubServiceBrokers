using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MSSqlSecureBroker.Models;
using Steeltoe.Security.DataProtection.CredHub;

namespace MSSqlSecureBroker.Controllers
{
    [Route("v2")]
    public class SecureBrokerController : Controller
    {
        private ILoggerFactory _loggerFactory;
        private ILogger<SecureBrokerController> _logger;
        private static ICredHubClient _credHub;

        //26e6b7d6-1b47-4d66-9a8b-e7b645d552cb
        //661f5db6-b9ad-4f89-bd3e-99e4c9fba426
        //ae0a2836-9940-40f2-b47f-0a64f60eaf35
        //99e86f84-2d22-4d89-a4e0-728281634c21

        public SecureBrokerController(ILogger<SecureBrokerController> logger, ILoggerFactory loggerFactory, IOptionsSnapshot<CredHubOptions> credHubOptions)
        {
            _loggerFactory = loggerFactory;
            _logger = logger;
            
            if (_credHub == null && Request?.Path.Value.Contains("Injected") != true)
            {
                // if a username and password were supplied, use that auth method, otherwise expect Diego to provide credentials on PCF
                try
                {
                    if (!string.IsNullOrEmpty(credHubOptions.Value.CredHubUser) && !string.IsNullOrEmpty(credHubOptions.Value.CredHubPassword))
                    {
                        _logger?.LogTrace("Getting CredHub UAA Client...");
                        _credHub = CredHubClient.CreateUAAClientAsync(credHubOptions.Value, _loggerFactory.CreateLogger<CredHubClient>()).Result;
                    }
                    else
                    {
                        _logger?.LogTrace("Getting CredHub mTLS Client...");
                        _credHub = CredHubClient.CreateMTLSClientAsync(credHubOptions.Value, _loggerFactory.CreateLogger<CredHubClient>()).Result;
                    }
                }
                catch (Exception e)
                {
                    _logger?.LogCritical(e, "Failed to initialize CredHubClient");
                }
            }
        }

        // GET v2/catalog
        [HttpGet("Catalog")]
        public Catalog Catalog()
        {
            var allServices = new Catalog();

            List<Service> svcs = new List<Service>();
            var svc1 = new Service() { Name = "MSSQLServer", Id = "0a78ac03-bf3b-42a5-a302-934b0b776324", Description = "MS SQL Server with CredHub Integration" };
            svc1.Plans.Add(new Plan() { Name = "standard", Description = "MS SQL Server connection", Id = "0f8a649e-9fd0-49d6-84dc-5aa65c31299a" });
            //svc1.Plans.Add(new Plan() { Name = "full", Description = "large", Id = "0f8a649e-9fd0-49d6-84dc-5aa65c31288a" });

            allServices.services.Add(svc1);

            //var svc2 = new Service() { Name = "Service 2", Id = "4dd1b266-3d33-43ba-b35d-b4b0a6cba733", Description = "Description 2" };
            //svc2.Plans.Add(new Plan() { Name = "basic 2", Description = "small", Id = "0f8a649e-9fd0-49d6-84dc-5aa65c31277a" });
            //svc2.Plans.Add(new Plan() { Name = "full 2", Description = "large", Id = "0f8a649e-9fd0-49d6-84dc-5aa65c31266a" });
            
            //allServices.services.Add(svc2);

            return (allServices);
        }

        // GET v2/lastOperation
        [HttpGet("LastOperation")]
        public LastOperation LastOperation([FromBody]string value)
        {
            return new LastOperation() { LastOperationState = "succeeded", Description = "succeeded" };
        }

        // PUT v2/provision
        [HttpPut("Provision")]
        public ProvisionedServiceSpec Provision([FromBody]string value)
        {
            if (_credHub != null)
            {
                dynamic provisionData = value;

            }

            return new ProvisionedServiceSpec();
        }

        // DELETE v2/deprovision
        [HttpDelete("Deprovision")]
        public DeprovisionServiceSpec Deprovision([FromBody]string value)
        {
            if (_credHub != null)
            {
                dynamic deprovisionData = value;

            }

            return new DeprovisionServiceSpec();
        }

        // PUT v2/bind
        [HttpPut("Bind")]
        public Binding Bind([FromBody]string value)
        {
            if (_credHub != null)
            {
                dynamic bindData = value;

            }

            return new Binding();
        }

        // DELETE v2/unbind
        [HttpDelete("Unbind")]
        public void Unbind([FromBody]string value)
        {
            if (_credHub != null)
            {
                dynamic unbindData = value;

            }
        }

        // PATCH v2/update
        [HttpPatch("Update")]
        public void Update([FromBody]string value)
        {
            if (_credHub != null)
            {
                dynamic updateData = value;

            }
        }
    }
}

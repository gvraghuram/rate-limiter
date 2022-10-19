using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using RateLimiter.DataAccess;
using RateLimiter.Model;
using RateLimiter.RulesEngine.Interfaces;
using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using RateLimiter.RulesEngine;
using RateLimiter.Extensions;

namespace RateLimiter.Tests
{
    [TestFixture]
    public class RateLimiterTest
    {
        private IRuleEngine engine;

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.AddRateLimitServices();
            services.AddSingleton<IStorageService, InMemoryService>();
            services.AddSingleton(typeof(IConfiguration), Configuration.GetConfiguration);
            var serviceProvider = services.BuildServiceProvider();
            var scope = serviceProvider.CreateScope();
            engine = scope.ServiceProvider.GetRequiredService<IRuleEngine>();
        }

        [TestCase(ClientRegions.EU)]
        public void TestEURuleInWindow_Successful(ClientRegions region)
        {

            //Arrange 
            var ip = IPAddress.Parse("127.0.0.1");
            var token = new ClientToken(ip);
            var request1 = new ClientRequest(token, region, DateTime.UtcNow);
            var request2 = new ClientRequest(token, region, DateTime.UtcNow.AddMilliseconds(100));

            //Act
            var validate1 = engine.ProcessRules(request1);
            var validate2 = engine.ProcessRules(request2);

            //Assert
            Assert.IsTrue(validate1);
            Assert.IsTrue(validate2);
        }

        [TestCase(ClientRegions.EU)]
        public void TestEURuleOutOfWindow_Successful(ClientRegions region)
        {

            //Arrange 
            var ip = IPAddress.Parse("127.0.0.1");
            var token = new ClientToken(ip);
            var request1 = new ClientRequest(token, region, DateTime.UtcNow);
            var request2 = new ClientRequest(token, region, DateTime.UtcNow.AddMilliseconds(5));

            //Act
            var validate1 = engine.ProcessRules(request1);
            var validate2 = engine.ProcessRules(request2);

            //Assert
            Assert.IsTrue(validate1);
            Assert.IsFalse(validate2);
        }

        [TestCase(ClientRegions.US)]
        [TestCase(ClientRegions.EU)]
        [TestCase(ClientRegions.Others)]
        public void TestUSOtherRuleInWindow_Successful(ClientRegions region)
        {

            //Arrange 
            var ip = IPAddress.Parse("127.0.0.1");
            var token = new ClientToken(ip);
            var request1 = new ClientRequest(token, region, DateTime.UtcNow);
            var request2 = new ClientRequest(token, region, DateTime.UtcNow.AddMilliseconds(100));

            //Act
            var validate1 = engine.ProcessRules(request1);
            var validate2 = engine.ProcessRules(request2);

            //Assert
            Assert.IsTrue(validate1);
            Assert.IsTrue(validate2);
        }

        [TestCase(ClientRegions.US)]
        [TestCase(ClientRegions.Others)]
        public void TestUSOtherRuleOutOfWindow_Successful(ClientRegions region)
        {
            //Arrange 
            var ip = IPAddress.Parse("127.0.0.1");
            var token = new ClientToken(ip);
            var request1 = new ClientRequest(token, region, DateTime.UtcNow);
            var request2 = new ClientRequest(token, region, DateTime.UtcNow.AddMilliseconds(5));

            //Act
            var validate1 = engine.ProcessRules(request1);
            var validate2 = engine.ProcessRules(request2);

            //Assert
            Assert.IsTrue(validate1);
            Assert.IsTrue(validate2);
        }

        [TestCase(ClientRegions.US)]
        [TestCase(ClientRegions.Others)]

        public void TestUSMaxRequestInWindows_Successfully(ClientRegions region)
        {
            //Arrage
            var ip = IPAddress.Parse("127.0.0.1");
            var token = new ClientToken(ip);
            var request = new ClientRequest(token, region, DateTime.UtcNow);
            var results = new List<bool>();

            //Act
            for (int i = 0; i < 10; i++)
            {
                results.Add(engine.ProcessRules(request));
            }


            //Assert
            Assert.IsTrue(results.All(x => x));
        }

        [TestCase(ClientRegions.US)]
        public void TestUSMaxRequestInWindows_ExceedingMax_Successfully(ClientRegions region)
        {
            //Arrage
            var ip = IPAddress.Parse("127.0.0.1");
            var token = new ClientToken(ip);
            var request = new ClientRequest(token, region, DateTime.UtcNow);

            //Act
            for (int i = 0; i < 10; i++)
            {
                engine.ProcessRules(request);
            }

            var result = engine.ProcessRules(request);

            //Assert
            Assert.IsFalse(result);
        }

        [TestCase(ClientRegions.Others)]
        public void OtherRegionsPassAllRules_Successfully(ClientRegions region)
        {
            //Arrange
            var ip = IPAddress.Parse("127.0.0.1");
            var token = new ClientToken(ip);
            var request = new ClientRequest(token, region, DateTime.UtcNow);
            bool result = true;

            //Act
            for (int i = 0; i < 100; i++)
            {
                request = new ClientRequest(token, region, DateTime.UtcNow.AddMilliseconds(i));
                result = result && engine.ProcessRules(request);
            }

            //Assert
            Assert.IsTrue(result);
        }
    }
}

using FluentAssertions;
using InsuranceClaimMicroservice.Controllers;
using InsuranceClaimMicroservice.Models;
using InsuranceClaimMicroservice.Repository;
using InsuranceClaimMicroservice.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;

namespace InsuranceClaimMicroservice.Tests
{
    [TestFixture]
    public class Tests
    {
        private Mock<IAuditRepository> _auditRepositoryStub;
        private Mock<IInitiateClaimService> _initiateClaimService;
        private Mock<IConfiguration> _configuration;
        [SetUp]
        public void Setup()
        {
            _auditRepositoryStub = new Mock<IAuditRepository>();
            _initiateClaimService = new Mock<IInitiateClaimService>();
            _configuration = new Mock<IConfiguration>();
        }

        [Test]
        public void GetAllInsurerDetail_WithExistingInsurerDetail_ReturnsAllInsurerDetail()
        {
            var expectedItems = new List<InsurerDetail>
           {
               new InsurerDetail { InsurerName = "Bajaj Insurance", InsurerPackageName = "L1", InsuranceAmountLimit = 200000, DisbursementDuration = 2},
               new InsurerDetail { InsurerName = "Tata Insurance", InsurerPackageName = "L2", InsuranceAmountLimit = 300000, DisbursementDuration = 1},
               new InsurerDetail { InsurerName = "Reliance Insurance", InsurerPackageName = "L3", InsuranceAmountLimit = 400000, DisbursementDuration = 1},
               new InsurerDetail { InsurerName = "LIC", InsurerPackageName = "L4", InsuranceAmountLimit = 250000, DisbursementDuration = 2},
               new InsurerDetail { InsurerName = "ICICI Insurance", InsurerPackageName = "L5", InsuranceAmountLimit = 280000, DisbursementDuration = 2}
           };
            _auditRepositoryStub.Setup(repo => repo.GetAllInsurers()).Returns(expectedItems);
            var controller = new AuditSeverityController(_auditRepositoryStub.Object, _initiateClaimService.Object, _configuration.Object);
            var response =  controller.GetAllInsurerDetail();
            var result = response as OkObjectResult;
            result.Value
                .Should()
                .BeEquivalentTo(expectedItems, options => options.ComparingByMembers<InsurerDetail>());           
        }

        [Test]
        public void GetInsurerByPackageName_WhenInsurerExists_ReturnsTheInsurerDetail()
        {
            var expectedItem = new InsurerDetail { InsurerName = "Bajaj Insurance", InsurerPackageName = "L1", InsuranceAmountLimit = 200000, DisbursementDuration = 2 };

            _auditRepositoryStub.Setup(repo => repo.GetInsurerByPackageName("L1")).Returns(expectedItem);
            var controller = new AuditSeverityController(_auditRepositoryStub.Object, _initiateClaimService.Object, _configuration.Object);
            var response = controller.GetInsurerByPackageName("L1");
            var result = response as OkObjectResult;
            result.Value.Should().BeEquivalentTo(expectedItem,
                options => options.ComparingByMembers<InsurerDetail>());
        }
        [Test]
        public void GetInsurerByPackageName_WhenInsurerDoesNotExists_ReturnsNotFound()
        {
            var controller = new AuditSeverityController(_auditRepositoryStub.Object, _initiateClaimService.Object, _configuration.Object);
            var response = controller.GetInsurerByPackageName("A1");
            response.Should().BeOfType<NotFoundResult>();
            (response as NotFoundResult).StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
        //[Test]
        //public void InitiateClaim_ClaimIsCorrect_ReturnsBalanceAmount()
        //{
        //    var initiateClaim = new InitiateClaim
        //    {
        //        Ailment = AilmentCategory.Orthopaedics,
        //        PatientId = 5,
        //        InsurerName = "LIC",
        //        TreatmentPackageName = "Package 1",
        //        PatientName = "Sushant"
        //    };

        //    var patient = new PatientDetail
        //    {
        //        Name = "Sushant",
        //        Id = 5,
        //        Age = 23,
        //        Ailment = AilmentCategory.Orthopaedics,
        //        TreatmentCommencementDate = System.DateTime.Today,
        //        TreatmentPackageName = "Package 1"
        //    };
            
        //}
        //[Test]
        //public void InitiateClaim_ClaimContainsIncorrectAilment_ReturnsBadRequest()
        //{
        //}

        //[Test]
        //public void InitiateClaim_ClaimContainsInsurerNameThatDoesNotExists_ReturnsBadRequest()
        //{
        //}

        //[Test]
        //public void InitiateClaim_ClaimContainsPatientIdThatDoesNotExists_ReturnsBadRequest()
        //{
        //}

        //[Test]
        //public void InitiateClaim_ClaimContainsPatientNameThatDoesNotExists_ReturnsBadRequest()
        //{
        //}

        //[Test]
        //public void InitiateClaim_ClaimContainsPackageNameThatDoesNotExists_ReturnsBadRequest()
        //{
        //}

        //[Test]
        //public void InitiateClaim_ClaimContainsPackageNameThatDoesNotBelongToPatient_ReturnsBadRequest()
        //{
        //}


    }
}
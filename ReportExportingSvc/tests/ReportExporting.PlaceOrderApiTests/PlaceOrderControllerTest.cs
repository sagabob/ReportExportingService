﻿using FluentAssertions;
using Lamar;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReportExporting.PlaceOrderApi.Controllers;
using ReportExporting.PlaceOrderApi.Handlers;
using Xunit;

namespace ReportExporting.PlaceOrderApiTests;

public class PlaceOrderControllerTest
{
    [Fact]
    public async Task CanPostExportRequestAsync()
    {
        var container = new Container(cfg =>
        {
            cfg.Scan(scanner =>
            {
                scanner.IncludeNamespaceContainingType<PlaceOrderRequest>();
                scanner.WithDefaultConventions();
                scanner.AddAllTypesOf(typeof(ExportRequestHandler));
            });
            cfg.For<IMediator>().Use<Mediator>();
        });

        var mediator = container.GetInstance<IMediator>();

        //Arrange
        var request = Helper.GetFakeReportRequest();

        var placeOrderController = new PlaceOrderController(mediator);

        //Act
        var actionResult = await placeOrderController.PlaceExportOrder(request);

        //Assert
        actionResult.Should().NotBeNull();

        var okResult = actionResult.Result as OkObjectResult;

        okResult?.Value.Should().Be(request);
    }
}
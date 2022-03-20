namespace SilentMike.DietMenu.Core.UnitTests.EPPlus;
using System.IO;
using System.Threading;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilentMike.DietMenu.Core.Application.Ingredients.Queries;
using SilentMike.DietMenu.Core.Infrastructure.EPPlus.QueryHandlers;

[TestClass]
public sealed class ParseIngredientsFromExcelFileHandlerTests
{
    [TestMethod]
    public void ShouldParseComplexCarbohydratesFromExcelFile()
    {
        var payload = File.ReadAllBytes("Ingredients.xlsx");
        var query = new ParseIngredientsFromExcelFile
        {
            Payload = payload,
            TypeInternalName = "ComplexCarbohydrate",
        };
        var handler = new ParseIngredientsFromExcelFileHandler();

        var result = handler.Handle(query, CancellationToken.None).Result;

        result.Should()
            .HaveCount(2)
            .And
            .Contain(i =>
                i.Exchanger == 1m
                && i.InternalName == "26996BEA-9973-54D9-3A39-296170521786"
                && i.Name == "Amarantus")
            .And
            .Contain(i =>
                i.Exchanger == 4.1m
                && i.InternalName == "8BD03DAB-025B-3F5A-03A3-6D67F1DB31B9"
                && i.Name == "Bataty")
            ;
    }

    [TestMethod]
    public void ShouldParseFruitsFromExcelFile()
    {
        var payload = File.ReadAllBytes("Ingredients.xlsx");
        var query = new ParseIngredientsFromExcelFile
        {
            Payload = payload,
            TypeInternalName = "Fruit",
        };
        var handler = new ParseIngredientsFromExcelFileHandler();

        var result = handler.Handle(query, CancellationToken.None).Result;

        result.Should()
            .HaveCount(2)
            .And
            .Contain(i =>
                i.Exchanger == 2.3m
                && i.InternalName == "86B194C9-1F48-29B8-A554-8304C9020CA4"
                && i.Name == "Agrest")
            .And
            .Contain(i =>
                i.Exchanger == 1.8m
                && i.InternalName == "02E6F65A-62E4-2E6F-70A8-8A8AF6E79BEE"
                && i.Name == "Ananas")
            ;
    }
}

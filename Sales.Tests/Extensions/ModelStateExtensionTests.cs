using Microsoft.AspNetCore.Mvc.ModelBinding;
using Sales.Extensions;

namespace Sales.Tests.Extensions;

public class ModelStateExtensionTests
{
    [Fact]
    public void GetErrors_ShouldReturnEmptyList_WhenModelStateIsValid()
    {
        // Arrange
        var modelState = new ModelStateDictionary();

        // Act
        var errors = modelState.GetErrors();

        // Assert
        Assert.Empty(errors);
    }

    [Fact]
    public void GetErrors_ShouldReturnErrors_WhenModelStateHasErrors()
    {
        // Arrange
        var modelState = new ModelStateDictionary();
        modelState.AddModelError("Key1", "Error message 1");
        modelState.AddModelError("Key2", "Error message 2");

        // Act
        var errors = modelState.GetErrors();

        // Assert
        Assert.Equal(2, errors.Count);
        Assert.Contains("Error message 1", errors);
        Assert.Contains("Error message 2", errors);
    }
}

using BLL;
using Carting.Controllers;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests
{

    public class CartControllerTests
    {

        [Fact]
        public void Get_ReturnsItems()
        {
            // Arrange
            var cartServiceMock = new Mock<ICartService>();
            cartServiceMock.Setup(service => service.GetItems()).Returns(new List<Item>());
            var controller = new CartController(cartServiceMock.Object);

            // Act
            var result = controller.Get();

            // Assert
            var okResult = Assert.IsAssignableFrom<IEnumerable<Item>>(result);
            var items = Assert.IsAssignableFrom<IEnumerable<Item>>(okResult);
            Assert.Empty(items);
        }

        [Fact]
        public void Get_ReturnsItem_WhenValidIdProvided()
        {
            // Arrange
            var cartServiceMock = new Mock<ICartService>();
            cartServiceMock.Setup(service => service.FindOne(It.IsAny<int>())).Returns(new Item());
            var controller = new CartController(cartServiceMock.Object);

            // Act
            var result = controller.Get(1); // Provide a valid ID

            // Assert
            Assert.IsType<ActionResult<Item>>(result);
        }

        [Fact]
        public void Get_ReturnsNotFound_WhenInvalidIdProvided()
        {
            // Arrange
            var cartServiceMock = new Mock<ICartService>();
            cartServiceMock.Setup(service => service.FindOne(It.IsAny<int>())).Returns(null as Item);
            var controller = new CartController(cartServiceMock.Object);

            // Act
            var result = controller.Get(999); // Provide an invalid ID

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void Get_ReturnsOkResult_WhenItemFound()
        {
            // Arrange
            var cartServiceMock = new Mock<ICartService>();
            var controller = new CartController(cartServiceMock.Object);
            var itemId = 1; // Sample item ID for testing
            var itemToReturn = new Item { Id = itemId, Name = "Test Item" };

            cartServiceMock.Setup(service => service.FindOne(itemId)).Returns(itemToReturn);

            // Act
            var result = controller.Get(itemId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedItem = Assert.IsType<Item>(okResult.Value);
            Assert.Equal(itemToReturn, returnedItem); // Compare the returned item with the expected item
        }
    }

}
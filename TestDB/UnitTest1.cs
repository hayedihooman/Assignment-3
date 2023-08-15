using NUnit.Framework;
using FarmersMarketAPIAss2.Models;
using Npgsql;


namespace TestDB
{
    [TestFixture]
    public class Tests
    {
        private NpgsqlConnection _connection;
        private Applications _applications;

        private const string ConnectionString = "Host=localhost;Port=5432;Database=FarmersMarket;Username=postgres;Password=12345;";

        [SetUp]
        public void Setup()
        {
            // Create an actual connection.
            _connection = new NpgsqlConnection(ConnectionString);

            // Create an instance of the Applications class.
            _applications = new Applications();
        }

        [Test]
        public void TestAddProduct()
        {
            // Arrange
            var product = new Products
            {
                id = 1,
                name = "Test Product",
                amount = 10.5m,
                price = 5.0m
            };

            // Act
            var response = _applications.AddProduct(_connection, product);

            // Assert
            Assert.AreEqual(200, response.StatusCode);
            Assert.AreEqual("Product added successfully", response.Message);
            Assert.AreEqual(product, response.Product);
        }

        [Test]
        public void TestUpdateProduct()
        {
            // Arrange
            var originalProduct = new Products
            {
                id = 2,
                name = "Test Product2",
                amount = 10.5m,
                price = 5.0m
            };

            // Ensure the connection is closed before starting the test
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
            }

            // Add the original product first
            _applications.AddProduct(_connection, originalProduct);

            var updatedProduct = new Products
            {
                id = 2,
                name = "Updated Product",
                amount = 15.0m,
                price = 7.0m
            };

            // Act
            var response = _applications.UpdateProduct(_connection, updatedProduct.id, updatedProduct);

            // Assert
            Assert.AreEqual(200, response.StatusCode, response.Message);  // Added response.Message for better error context
            Assert.AreEqual("Product updated successfully", response.Message);
            Assert.AreEqual(updatedProduct, response.Product);

            // Cleanup: Ensure the connection is closed after the test
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
            }
        }


        [Test]
        public void TestDeleteProduct()
        {
            // Arrange - Add a product
            var productToDelete = new Products
            {
                id = 3,
                name = "Product to Delete",
                amount = 12.5m,
                price = 6.0m
            };
            _applications.AddProduct(_connection, productToDelete);

            // Act
            var response = _applications.DeleteProduct(_connection, productToDelete.id);

            // Assert
            Assert.AreEqual(200, response.StatusCode);
            Assert.AreEqual("Product deleted successfully", response.Message);
        }

        [Test]
        public void TestGetProduct()
        {
            // Arrange
            int productId = 124567;

            // Act
            var response = _applications.GetProduct(_connection, productId);

            // Assert
            Assert.AreEqual(200, response.StatusCode);
            Assert.AreEqual("Product fetched successfully", response.Message);
            Assert.IsNotNull(response.Product);
        }

        [Test]
        public void TestGetProductByName()
        {
            // Arrange
            string productName = "Apple";

            // Act
            var response = _applications.GetProductByName(_connection, productName);

            // Assert
            Assert.AreEqual(200, response.StatusCode);
            Assert.AreEqual("Product fetched successfully", response.Message);
            Assert.IsNotNull(response.Product);
        }

        [Test]
        public void TestGetProducts()
        {
            // Act
            var response = _applications.GetProducts(_connection);

            // Assert
            Assert.AreEqual(200, response.StatusCode);
            Assert.AreEqual("Products fetched successfully", response.Message);
            Assert.IsNotNull(response.Products);
            Assert.IsTrue(response.Products.Count > 0);
        }


    }
}
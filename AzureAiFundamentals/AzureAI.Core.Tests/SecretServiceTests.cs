using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Security.KeyVault.Secrets;
using AzureAI.Core;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AzureAI.Core.Tests
{
    public class SecretServiceTests
    {
        private readonly Mock<ILogger<SecretService>> _loggerMock;
        private readonly Mock<SecretClient> _clientMock;
        private readonly SecretService _service;

        public SecretServiceTests()
        {
            _loggerMock = new Mock<ILogger<SecretService>>();
            _clientMock = new Mock<SecretClient>(MockBehavior.Strict, new Uri("https://fake.vault"), (Azure.Core.TokenCredential)null);
            _service = new SecretService(_loggerMock.Object, _clientMock.Object);
        }

        [Fact]
        public async Task GetSecretAsync_ReturnsSecretValue()
        {
            // Arrange
            var secretName = "TestSecret";
            var secretValue = "SecretValue";
            var secret = SecretModel(secretName, secretValue);
            _clientMock.Setup(c => c.GetSecretAsync(secretName, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(ResponseFromValue(secret));

            // Act
            var result = await _service.GetSecretAsync(secretName);

            // Assert
            Assert.Equal(secretValue, result);
            _loggerMock.Verify(l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Retrieving secret '{secretName}'")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }

        [Fact]
        public async Task GetSecretsAsync_ReturnsAllSecrets()
        {
            // Arrange
            var secretNames = new[] { "Secret1", "Secret2" };
            var secretValues = new Dictionary<string, string>
            {
                { "Secret1", "Value1" },
                { "Secret2", "Value2" }
            };
            foreach (var kvp in secretValues)
            {
                _clientMock.Setup(c => c.GetSecretAsync(kvp.Key, null, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(ResponseFromValue(SecretModel(kvp.Key, kvp.Value)));
            }

            // Act
            var result = await _service.GetSecretsAsync(secretNames);

            // Assert
            Assert.Equal(secretValues, result);
        }

        [Fact]
        public async Task GetSecretAsync_Throws_LogsError()
        {
            // Arrange
            var secretName = "BadSecret";
            var exception = new Exception("Not found");
            _clientMock.Setup(c => c.GetSecretAsync(secretName, null, It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _service.GetSecretAsync(secretName));
            Assert.Equal("Not found", ex.Message);
            _loggerMock.Verify(l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Error retrieving secret '{secretName}': {exception.Message}")),
                exception,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }

        // Helpers for mocking
        private static KeyVaultSecret SecretModel(string name, string value) =>
            SecretModelWithVersion(name, value, "v1");
        private static KeyVaultSecret SecretModelWithVersion(string name, string value, string version) =>
            new KeyVaultSecret(name, value) { Properties = { Version = version } };
        private static Response<KeyVaultSecret> ResponseFromValue(KeyVaultSecret secret) =>
            Mock.Of<Response<KeyVaultSecret>>(r => r.Value == secret);
    }
}

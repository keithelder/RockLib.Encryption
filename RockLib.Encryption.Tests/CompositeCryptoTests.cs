﻿using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace RockLib.Encryption.Tests
{
    [TestFixture]
    public class CompositeCryptoTests
    {
        [Test]
        public void ConstructorThrowsWhenCryptoListIsNull()
        {
            Action action = () => new CompositeCrypto(null);

            action.ShouldThrow<ArgumentNullException>().WithMessage("Value cannot be null.\r\nParameter name: cryptos");
        }

        [Test]
        public void GetEncryptorSucceedsWithExistingICrypto()
        {
            ICrypto fooCrypto = CreateICrypto("foo");
            ICrypto barCrypto = CreateICrypto("bar");

            var compositeCrypto = new CompositeCrypto(new List<ICrypto> { fooCrypto, barCrypto });

            var fooEncryptor = compositeCrypto.GetEncryptor("foo");
            var barEncryptor = compositeCrypto.GetEncryptor("bar");
            
            fooEncryptor.Should().NotBeNull();
            barEncryptor.Should().NotBeNull();
            fooEncryptor.Should().NotBeSameAs(barEncryptor);
        }

        [Test]
        public void GetEncryptorThrowsWhenICryptoDoesntExist()
        {
            ICrypto fooCrypto = CreateICrypto("foo");
            ICrypto barCrypto = CreateICrypto("bar");

            var compositeCrypto = new CompositeCrypto(new List<ICrypto> { fooCrypto, barCrypto });

            Action action = () => compositeCrypto.GetEncryptor("baz");
            action.ShouldThrow<KeyNotFoundException>()
                .WithMessage("Unable to locate implementation of ICrypto that can locate a credential using credentialName: baz");
        }

        [Test]
        public void GetDecryptorSucceedsWithExistingICrypto()
        {
            ICrypto fooCrypto = CreateICrypto("foo");
            ICrypto barCrypto = CreateICrypto("bar");

            var compositeCrypto = new CompositeCrypto(new List<ICrypto> { fooCrypto, barCrypto });

            var fooEncryptor = compositeCrypto.GetDecryptor("foo");
            var barEncryptor = compositeCrypto.GetDecryptor("bar");

            fooEncryptor.Should().NotBeNull();
            barEncryptor.Should().NotBeNull();
            fooEncryptor.Should().NotBeSameAs(barEncryptor);
        }

        [Test]
        public void GetDecryptorThrowsWhenICryptoDoesntExist()
        {
            ICrypto fooCrypto = CreateICrypto("foo");
            ICrypto barCrypto = CreateICrypto("bar");

            var compositeCrypto = new CompositeCrypto(new List<ICrypto> { fooCrypto, barCrypto });

            Action action = () => compositeCrypto.GetDecryptor("baz");
            action.ShouldThrow<KeyNotFoundException>()
                .WithMessage("Unable to locate implementation of ICrypto that can locate a credential using credentialName: baz");
        }

        [Test]
        public void CanEncryptReturnsTrueForExistingCrypto()
        {
            ICrypto fooCrypto = CreateICrypto("foo");
            ICrypto barCrypto = CreateICrypto("bar");

            var compositeCrypto = new CompositeCrypto(new List<ICrypto> { fooCrypto, barCrypto });

            compositeCrypto.CanEncrypt("foo").Should().BeTrue();
            compositeCrypto.CanEncrypt("bar").Should().BeTrue();
        }

        [Test]
        public void CanEncryptReturnsFalseForNonExistingCrypto()
        {
            ICrypto fooCrypto = CreateICrypto("foo");
            ICrypto barCrypto = CreateICrypto("bar");

            var compositeCrypto = new CompositeCrypto(new List<ICrypto> { fooCrypto, barCrypto });

            compositeCrypto.CanEncrypt("baz").Should().BeFalse();
        }

        [Test]
        public void CanDecryptReturnsTrueForExistingCrypto()
        {
            ICrypto fooCrypto = CreateICrypto("foo");
            ICrypto barCrypto = CreateICrypto("bar");

            var compositeCrypto = new CompositeCrypto(new List<ICrypto> { fooCrypto, barCrypto });

            compositeCrypto.CanDecrypt("foo").Should().BeTrue();
            compositeCrypto.CanDecrypt("bar").Should().BeTrue();
        }

        [Test]
        public void CanDecryptReturnsFalseForNonExistingCrypto()
        {
            ICrypto fooCrypto = CreateICrypto("foo");
            ICrypto barCrypto = CreateICrypto("bar");

            var compositeCrypto = new CompositeCrypto(new List<ICrypto> { fooCrypto, barCrypto });

            compositeCrypto.CanDecrypt("baz").Should().BeFalse();
        }

        [Test]
        public void EncryptSucceedsWithExistingICrypto()
        {
            ICrypto fooCrypto = CreateICrypto("foo");
            ICrypto barCrypto = CreateICrypto("bar");

            var compositeCrypto = new CompositeCrypto(new List<ICrypto> { fooCrypto, barCrypto });

            compositeCrypto.Encrypt("stuff", "foo").Should().Be("EncryptedString : foo");
            compositeCrypto.Encrypt(new byte[0], "foo").Should().BeEquivalentTo(Encoding.UTF8.GetBytes("foo"));
            compositeCrypto.Encrypt("stuff", "bar").Should().Be("EncryptedString : bar");
            compositeCrypto.Encrypt(new byte[0], "bar").Should().BeEquivalentTo(Encoding.UTF8.GetBytes("bar"));
        }

        [Test]
        public void EncryptThrowsWhenICryptoDoesntExist()
        {
            ICrypto fooCrypto = CreateICrypto("foo");
            ICrypto barCrypto = CreateICrypto("bar");

            var compositeCrypto = new CompositeCrypto(new List<ICrypto> { fooCrypto, barCrypto });

            Action action = () => compositeCrypto.Encrypt("stuff", "baz");
            action.ShouldThrow<KeyNotFoundException>()
                .WithMessage("Unable to locate implementation of ICrypto that can locate a credential using credentialName: baz");
        }

        [Test]
        public void DecryptSucceedsWithExistingICrypto()
        {
            ICrypto fooCrypto = CreateICrypto("foo");
            ICrypto barCrypto = CreateICrypto("bar");

            var compositeCrypto = new CompositeCrypto(new List<ICrypto> { fooCrypto, barCrypto });

            compositeCrypto.Decrypt("stuff", "foo").Should().Be("DecryptedString : foo");
            compositeCrypto.Decrypt(new byte[0], "foo").Should().BeEquivalentTo(Encoding.UTF8.GetBytes("foo"));
            compositeCrypto.Decrypt("stuff", "bar").Should().Be("DecryptedString : bar");
            compositeCrypto.Decrypt(new byte[0], "bar").Should().BeEquivalentTo(Encoding.UTF8.GetBytes("bar"));
        }

        [Test]
        public void DecryptThrowsWhenICryptoDoesntExist()
        {
            ICrypto fooCrypto = CreateICrypto("foo");
            ICrypto barCrypto = CreateICrypto("bar");

            var compositeCrypto = new CompositeCrypto(new List<ICrypto> { fooCrypto, barCrypto });

            Action action = () => compositeCrypto.Decrypt("stuff", "baz");
            action.ShouldThrow<KeyNotFoundException>()
                .WithMessage("Unable to locate implementation of ICrypto that can locate a credential using credentialName: baz");
        }

        private static ICrypto CreateICrypto(string cryptoId)
        {
            var cryptoMock = new Mock<ICrypto>();

            cryptoMock.Setup(f => f.CanEncrypt(cryptoId)).Returns(true);
            cryptoMock.Setup(f => f.CanEncrypt(It.Is<string>(s => s != cryptoId))).Returns(false);
            cryptoMock.Setup(f => f.Encrypt(It.IsAny<string>(), It.IsAny<string>())).Returns($"EncryptedString : {cryptoId}");
            cryptoMock.Setup(f => f.Encrypt(It.IsAny<byte[]>(), It.IsAny<string>())).Returns(Encoding.UTF8.GetBytes(cryptoId));
            cryptoMock.Setup(f => f.GetEncryptor(cryptoId)).Returns(new Mock<IEncryptor>().Object);

            cryptoMock.Setup(f => f.CanDecrypt(cryptoId)).Returns(true);
            cryptoMock.Setup(f => f.CanDecrypt(It.Is<string>(s => s != cryptoId))).Returns(false);
            cryptoMock.Setup(f => f.Decrypt(It.IsAny<string>(), It.IsAny<string>())).Returns($"DecryptedString : {cryptoId}");
            cryptoMock.Setup(f => f.Decrypt(It.IsAny<byte[]>(), It.IsAny<string>())).Returns(Encoding.UTF8.GetBytes(cryptoId));
            cryptoMock.Setup(f => f.GetDecryptor(cryptoId)).Returns(new Mock<IDecryptor>().Object);

            return cryptoMock.Object;
        }
    }
}

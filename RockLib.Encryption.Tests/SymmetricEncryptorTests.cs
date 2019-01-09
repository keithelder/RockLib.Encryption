﻿using System.Text;
using FluentAssertions;
using NUnit.Framework;
using RockLib.Encryption.Symmetric;

namespace RockLib.Encryption.Tests
{
    [TestFixture]
    public class SymmetricEncryptorTests
    {
        [Test]
        public void CanEncryptByString()
        {
            var credential = new Credential(
                () => new byte[] { 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15 },
                SymmetricAlgorithm.Aes, 16);

            var symmetricEncryptor = new SymmetricEncryptor(credential, Encoding.UTF8);

            var unencrypted = "This is some string";
            var encrypted = symmetricEncryptor.Encrypt(unencrypted);

            encrypted.Should().NotBeNullOrEmpty();
            encrypted.Should().NotBe(unencrypted);
        }

        [Test]
        public void CanEncryptByByteArray()
        {
            var credential = new Credential(
                () => new byte[] { 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15 },
                SymmetricAlgorithm.Aes, 16);

            var symmetricEncryptor = new SymmetricEncryptor(credential, Encoding.UTF8);

            var unencryptedString = "This is some string";
            var unencrypted = Encoding.UTF8.GetBytes(unencryptedString);
            var encrypted = symmetricEncryptor.Encrypt(unencrypted);
            var encryptedString = Encoding.UTF8.GetString(encrypted);

            encrypted.Should().NotBeEmpty();
            encryptedString.Should().NotBeNullOrEmpty();
            encryptedString.Should().NotBe(unencryptedString);
        }
    }
}

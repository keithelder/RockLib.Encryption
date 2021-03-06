﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RockLib.Encryption.Symmetric
{
    /// <summary>
    /// An implementation of <see cref="ICrypto"/> that uses the symmetric encryption
    /// algorithms that are in the .NET base class library.
    /// </summary>
    public class SymmetricCrypto : ICrypto
    {
        private readonly CredentialCache<Credential> _credentialCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="SymmetricCrypto"/> class.
        /// </summary>
        /// <param name="credentials">
        /// A collection of credentials that will be available for encryption or decryption
        /// operations.
        /// </param>
        /// <param name="encoding">
        /// The <see cref="System.Text.Encoding"/> to be used for string/binary conversions.
        /// </param>
        public SymmetricCrypto(IReadOnlyCollection<Credential> credentials, Encoding encoding = null)
        {
            if (credentials == null)
                throw new ArgumentNullException(nameof(credentials));

            Encoding = encoding ?? Encoding.UTF8;
            _credentialCache = new CredentialCache<Credential>(credentials);
        }

        /// <summary>
        /// Gets the non-default (named) credentials.
        /// </summary>
        public IReadOnlyCollection<Credential> Credentials => _credentialCache.Credentials;

        /// <summary>
        /// Gets the default (unnamed) credential.
        /// </summary>
        public Credential DefaultCredential => _credentialCache.DefaultCredential;

        /// <summary>
        /// Gets the <see cref="System.Text.Encoding"/> to be used for string/binary conversions.
        /// </summary>
        public Encoding Encoding { get; }

        /// <summary>
        /// Encrypts the specified plain text.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <returns>The encrypted value as a string.</returns>
        public string Encrypt(string plainText, string credentialName)
        {
            using (var encryptor = GetEncryptor(credentialName))
                return encryptor.Encrypt(plainText);
        }

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <returns>The decrypted value as a string.</returns>
        public string Decrypt(string cipherText, string credentialName)
        {
            using (var decryptor = GetDecryptor(credentialName))
                return decryptor.Decrypt(cipherText);
        }

        /// <summary>
        /// Encrypts the specified plain text.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <returns>The encrypted value as a byte array.</returns>
        public byte[] Encrypt(byte[] plainText, string credentialName)
        {
            using (var encryptor = GetEncryptor(credentialName))
                return encryptor.Encrypt(plainText);
        }

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <returns>The decrypted value as a byte array.</returns>
        public byte[] Decrypt(byte[] cipherText, string credentialName)
        {
            using (var decryptor = GetDecryptor(credentialName))
                return decryptor.Decrypt(cipherText);
        }

        /// <summary>
        /// Gets an instance of <see cref="IEncryptor"/> for the provided credential name.
        /// </summary>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <returns>An object that can be used for encryption operations.</returns>
        public IEncryptor GetEncryptor(string credentialName) =>
            new SymmetricEncryptor(GetCachedCredential(credentialName), Encoding);

        /// <summary>
        /// Gets an instance of <see cref="IDecryptor"/> for the provided credential name.
        /// </summary>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <returns>An object that can be used for decryption operations.</returns>
        public IDecryptor GetDecryptor(string credentialName) =>
            new SymmetricDecryptor(GetCachedCredential(credentialName), Encoding);

        /// <summary>
        /// Returns a value indicating whether this instance of <see cref="ICrypto"/>
        /// is able to handle the provided credential name for an encrypt operation.
        /// </summary>
        /// <param name="credentialName">
        /// The credential name to check, or null to check if the default credential exists.
        /// </param>
        /// <returns>
        /// True, if this instance can handle the credential name for an encrypt operation.
        /// Otherwise, false.
        /// </returns>
        public bool CanEncrypt(string credentialName) =>
            _credentialCache.TryGetCredential(credentialName, out var dummy);

        /// <summary>
        /// Returns a value indicating whether this instance of <see cref="ICrypto"/>
        /// is able to handle the provided credential name for an decrypt operation.
        /// </summary>
        /// <param name="credentialName">
        /// The credential name to check, or null to check if the default credential exists.
        /// </param>
        /// <returns>
        /// True, if this instance can handle the credential name for an encrypt operation.
        /// Otherwise, false.
        /// </returns>
        public bool CanDecrypt(string credentialName) =>
            CanEncrypt(credentialName);

        private Credential GetCachedCredential(string credentialName) =>
            _credentialCache.TryGetCredential(credentialName, out var credential)
                ? credential
                : throw new KeyNotFoundException($"Unable to locate credential using credentialName: {credentialName}");
    }
}